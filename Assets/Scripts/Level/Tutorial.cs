using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : Level {
	private Text text;

	public static bool sLock = false;
	public static bool turnLock = false;
	public static bool beamLock = false;
	public static bool checkBeam = false;
	public static bool fireLock = false;
	public static bool fireCheck = false;

	public GameObject goal;
	public GameObject wText;
	public GameObject sText;
	public GameObject turnText;
	public GameObject beamText;
	public GameObject tractorText;
	public GameObject fireText;
	private GameObject tutorialJunk;

	public List<Vector3> goalPositions = new List<Vector3>();

	public int currentPosition = 0;

	// Use this for initialization
	void Start () {
		base.Init();
		self = this;
		text = GetComponent<Text>();

		sLock = true;
		turnLock = true;
		beamLock = true;
		fireLock = true;
		SetGoalPosition(goal, goalPositions[currentPosition]);
		wText.SetActive(true);
		Goal.activationType = Goal.ActivationType.Player;
	}

	void OnGUI() {

	}

	void Update() {
		if (tutorialJunk != null) {
			if (!tutorialJunk.transform.Find("sprite").GetComponent<Renderer>().isVisible && !tutorialJunk.GetComponent<Junk>().GetIsTractored()) {
				tutorialJunk.transform.position = Vector3.zero;
				tutorialJunk.GetComponent<Rigidbody>().velocity = Vector3.zero;
				tutorialJunk.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			}
		}
	}

	public static void AdvanceTutorial() {
		Tutorial t = (Tutorial) self;

		if (sLock) {
			sLock = false;
			DoSwitching(t.wText, t.sText, t.goal, GetGoalPosition(++t.currentPosition), true, t); // Move forward complete , now move backward
		} else if (turnLock) {
			turnLock = false;
			DoSwitching(t.sText, t.turnText, t.goal, GetGoalPosition(++t.currentPosition), true, t); // Move backward complete, now move backward
		} else if (beamLock) {
			if (t.currentPosition == 3) { // Rotate ship complete, now turn on the tractor beam
				beamLock = false;
				DoSwitching(t.turnText, t.beamText, t.goal, Vector3.zero, true, t);
				t.text.text = "Turn on the tractor beam";
				checkBeam = true;
				Goal.activationType = Goal.ActivationType.None;
			} else { // Rotate ship complete
				DoSwitching(null, null, t.goal, GetGoalPosition(++t.currentPosition), true, t);
				Goal.activationType = Goal.ActivationType.Player;
			}
		} else if (fireLock) {
			if (t.currentPosition == 3) { // Turned on tractor beam, now move junk into goal
				DoSwitching(t.beamText, t.tractorText, t.goal, GetGoalPosition(++t.currentPosition), true, t);
				t.tutorialJunk = (GameObject) Instantiate(t.junkPrefab, new Vector3(10, 0, 0), t.junkPrefab.transform.rotation);
				t.text.text = "Use the Tractor Beam to move the Space Junk into the highlighted area";
				checkBeam = false;
				Goal.activationType = Goal.ActivationType.Junk;
			} else { // Moved junk into goal, now firing junk away
				DoSwitching(t.tractorText, t.fireText, t.goal, Vector3.zero, true, t);
				Instantiate(t.junkPrefab, new Vector3(0, 0, 0), t.junkPrefab.transform.rotation);
				fireLock = false;
				fireCheck = true;
				t.text.text = "Use the Tractor Beam to Fire the Space Junk away";
				Goal.activationType = Goal.ActivationType.None;
			}
		} else { 
			if (t.currentPosition == 4) { // Fired junk away, now fire into goal
				t.StartCoroutine(t.WaitToSwitch(t));
			} else { // Fired into goal, show end screen
				LevelComplete.SetActive(true);
			}
		}
	}

	private static Vector3 GetGoalPosition(int i) {
		try {
			return ((Tutorial) self).goalPositions[i];
		} catch (System.Exception e) {
			return Vector3.zero;
		}
	}

	private static void DoSwitching(GameObject disable, GameObject enable, GameObject goal, Vector3 goalPos, bool slowPlayerShip, Tutorial t) {
		if (disable != null) disable.SetActive(false);
		if (enable != null) enable.SetActive(true);
		if (goal != null) {
			if (goalPos != Vector3.zero) {
				goal.SetActive(true);
				SetGoalPosition(goal, goalPos);
			} else goal.SetActive(false);
		}

		if (slowPlayerShip) t.StartCoroutine(t.SlowShipDown());
	}

	IEnumerator SlowShipDown() {
		yield return new WaitForSeconds(.6f);
		ShipController.NormalSpeed();
	}

	IEnumerator WaitToSwitch(Tutorial t) {
		yield return new WaitForSeconds(1f);
		t.tutorialJunk = (GameObject) Instantiate(t.junkPrefab, new Vector3(0, -5, 0), t.junkPrefab.transform.rotation);
		DoSwitching(null, null, t.goal, GetGoalPosition(++t.currentPosition), true, t);
		t.text.text = "Use the Tractor Beam to Fire the Space Junk into the highlighted area";
		fireCheck = false;
		Goal.activationType = Goal.ActivationType.Fired;

	}

	private static void SetGoalPosition(GameObject goal, Vector3 pos) {
		goal.transform.position = pos; 
	}

	protected override void AddPointTypes() {

	}

	
}
