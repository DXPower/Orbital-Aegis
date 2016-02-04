using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private static Vector3 originalPosition;

	private static float shakeDecay;
	private static float shakeIntensity;

	void Update() {
		if (shakeIntensity > 0) {
			transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
			shakeIntensity -= shakeDecay;
		}
	}

	public static void Shake(float intensity, float decay, Vector3 originalPosition) {
		CameraController.originalPosition = originalPosition;
		shakeIntensity = intensity;
		shakeDecay = decay;
	}
}
