using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Points;

public abstract class Level : MonoBehaviour {
	public static bool gameRunning = false;

	public static GameObject canvas;
	public GameObject pointFloaterPrefab;
	public GameObject junkPrefab;

	public static Level self;
	 
	protected void Init() {
		canvas = GameObject.Find("Canvas");
		AddPointTypes();
		StartLevel();
	}

	protected string CreatePointsText() {
		Dictionary<PointType, int> points = PointManager.GetPoints();
		Dictionary<string, int> pointsText = new Dictionary<string, int>();
		string result = "";
		int total = 0;

		// Multiple types of points in code, only a few are shown to the player. They are combined here.
		foreach (KeyValuePair<PointType, int> pair in points) {
			string s = PointManager.GetPointTypeString(pair.Key);

			if (pointsText.ContainsKey(s)) pointsText[s] += pair.Value;
			else pointsText.Add(s, pair.Value);
		}

		// Sorted alphabetically
		var sorted = from pair in pointsText
					 orderby pair.Key ascending
					 select pair;

		// And shown, with the totals calculated.
		foreach (KeyValuePair<string, int> pair in sorted) {
			result += pair.Key + ": " + pair.Value + "\n";
			total += pair.Value;
		}

		result += "Total: " + total;

		return result;
	}

	public virtual void StartLevel() {
		//LevelComplete.SetActive(false);
		gameRunning = true;
	}

	public virtual void CompleteLevel() {
		LevelComplete.SetActive(true);
		gameRunning = false;
	}

	public virtual void FailLevel() {
		LevelFail.SetActive(true);
		gameRunning = false;
	}

	public virtual void RetryLevel() {
		LevelFail.SetActive(false);
		LevelComplete.SetActive(false);
		//PointManager.ResetPoints();
		gameRunning = true;
	}

	protected abstract void AddPointTypes();
}
