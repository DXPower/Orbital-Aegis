using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ISSHealthBar : MonoBehaviour {
	private RectTransform bar;
	private RectTransform backBar;
	private RectTransform rt;
	private Text text;

	private float normalLenght;
	private float normalHeight;

	void Start() {
		rt = GetComponent<RectTransform>();
		bar = transform.Find("Front").GetComponent<RectTransform>();
		backBar = transform.Find("Back").GetComponent<RectTransform>();
		text = GetComponentInChildren<Text>();
		normalLenght = bar.sizeDelta.x;
		normalHeight = bar.sizeDelta.y;
	}

	void OnGUI() {
		Vector3 pos = ISS.GetScreenPosition();
		pos.y += 50;
		rt.position = pos;
		bar.sizeDelta = new Vector2((ISS.currentHealth / ISS.maxHealth) * normalLenght, normalHeight);
		bar.localPosition = new Vector2(backBar.rect.xMin + (bar.sizeDelta.x / 2) - 1, backBar.localPosition.y);

		text.text = Mathf.Floor(ISS.currentHealth).ToString() + "/" + Mathf.Floor(ISS.maxHealth).ToString();
	}
}
