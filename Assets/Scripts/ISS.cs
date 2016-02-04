using UnityEngine;
using System.Collections;
using Points;

public class ISS : MonoBehaviour {
	public GameObject topPosition;
	public GameObject bottomPosition;
	private static GameObject issGameObject;

	public static float shakeIntensity;
	public static float shakeDecay;
	public float shake_decay;
	public float shake_intensity;
	private float step = 0f;

	private int direction = 1;

	public const float maxHealth = 100;
	private static float _currentHealth;
	public static float currentHealth {
		get { return _currentHealth; }
		set {
			_currentHealth = value;
			if (_currentHealth < 0) _currentHealth = 0;
			if (_currentHealth > maxHealth) _currentHealth = maxHealth;
		}
	}

	void Start() {
		issGameObject = gameObject;
		currentHealth = maxHealth;
		shakeIntensity = shake_intensity;
		shakeDecay = shake_decay;
	}

	void FixedUpdate() {
		if (Level.gameRunning) {
			transform.position = Vector3.Lerp(bottomPosition.transform.position, topPosition.transform.position, EaseInOutQuad(step, 0, 1, 1));

			step += .001f * direction;

			if (step >= 1f || step <= 0f) direction *= -1;
		}
	}

	private float EaseInOutQuad(float t, float b, float c, float d) {
		t /= d / 2;
		if (t < 1) return c / 2 * t * t + b;
		t--;
		return -c / 2 * (t * (t - 2) - 1) + b;
	}

	public static void Hit(int damage, GameObject source) {
		currentHealth -= damage;
		PointManager.AddPoints(PointType.issHit, -damage, Camera.main.WorldToScreenPoint(source.transform.position) + new Vector3(50, 0, 0), Color.red);
		CameraController.Shake(shakeIntensity, shakeDecay, Camera.main.transform.position);

		if (currentHealth <= 0) Level.self.FailLevel();
	}

	public static Vector3 GetScreenPosition() {
		return Camera.main.WorldToScreenPoint(issGameObject.transform.Find("iss").position);
	}
}
