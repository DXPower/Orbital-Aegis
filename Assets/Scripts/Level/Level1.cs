using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Points;

public class Level1 : Level {
	public const int maxTime = 35;
	public static int timeLeft = maxTime;

	private Text text;

	void Start() {
		base.Init();
		text = GetComponent<Text>();
		self = this;
		StartCoroutine(Counter());
	}

	void OnGUI() {
		if (text == null) text = GetComponent<Text>();
		text.text = "Protect the ISS for " + timeLeft + "s";
	}

	IEnumerator Counter() {
		do {
			yield return new WaitForSeconds(1);
			timeLeft--;
		} while (timeLeft > 0);

		CompleteLevel();
	}

	protected override void AddPointTypes() {
		PointManager.AddPointTypes(new PointType[] { PointType.tractor, PointType.issHit, PointType.issHealth, PointType.junkCollision });
	}

	public override void CompleteLevel() {
		base.CompleteLevel();

		PointManager.AddPoints(PointType.issHealth, (int) Mathf.Floor(ISS.currentHealth) * 10);
		LevelComplete.SetPointsText(CreatePointsText());
	}

	public override void RetryLevel() {
		base.RetryLevel();
		JunkFactory.ClearJunk();
		ISS.currentHealth = ISS.maxHealth;
		ShipController.self.transform.position = Vector3.zero;
		timeLeft = maxTime;
	}
}
