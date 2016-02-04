using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointFloater : MonoBehaviour {
	private float step = 0;
	private float aValue;

	private Vector3 originalPosition;

	private Text text;
	private Outline outline;

	public void Start() {
		transform.SetParent(Level.canvas.transform);
	}

	private void Init(Vector3 position, string value, Color color) {
		originalPosition = position;
		text = GetComponent<Text>();
		outline = GetComponent<Outline>();
		text.text = value;
		text.color = color;
	}

	public static void Factory(GameObject go, string value, Color color) {
		Factory(Camera.main.WorldToScreenPoint(go.transform.position), value, color);
	}

	public static void Factory(Vector3 screenPos, string value, Color color) {
		Instantiate<GameObject>(Level.self.pointFloaterPrefab).GetComponent<PointFloater>().Init(screenPos, value, color);
	}

	public void OnGUI() {
		transform.position = Vector3.Lerp(originalPosition, new Vector3(originalPosition.x, originalPosition.y + 50, originalPosition.z), EaseOutCirc(step, 0, 1, 1));

		aValue = 1 - EaseInCirc(step, 0, 1, 1);
		text.color = new Color(text.color.r, text.color.g, text.color.b, aValue);
		outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, aValue);

		step += .01f;

		if (step >= 1) Destroy(gameObject);
	}

	private float EaseOutCirc(float t, float b, float c, float d) {
		t /= d;
		t--;
		return c * Mathf.Sqrt(1 - t * t) + b;
	}

	private float EaseInCirc(float t, float b, float c, float d) {
		t /= d;
		return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
	}
}
