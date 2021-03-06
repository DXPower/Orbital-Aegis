﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelComplete : MonoBehaviour {
	private static GameObject staticSelf;

	private static Text pointsText;

	void Start() {
		staticSelf = gameObject;
		pointsText = transform.Find("Points").GetComponent<Text>();
		staticSelf.SetActive(false);
	}

	public static void SetActive(bool f) {
		staticSelf.SetActive(f);
	}

	public static void SetPointsText(string text) {
		pointsText.text = text;
	}
}
