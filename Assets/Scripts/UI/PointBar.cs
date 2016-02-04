using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Points;

public class PointBar : MonoBehaviour {
	private Text text;

	void Start() {
		text = GetComponent<Text>();
	}

	void OnGUI() {
		text.text = "Points: " + PointManager.GetTotalPoints();
	}
}
