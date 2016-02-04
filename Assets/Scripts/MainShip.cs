using UnityEngine;
using System.Collections;

public class MainShip : MonoBehaviour {
	public const int maxEnergy = 100;

	public  int _energy = maxEnergy;
	public static int energy {
		get { return ship._energy; }
		set {
			ship._energy = value;

			if (ship._energy > maxEnergy) ship._energy = maxEnergy;
			if (ship._energy < 0) ship._energy = 0;
		}
	}

	private static MainShip ship;

	void Start() {
		ship = this;
		_energy = maxEnergy;
		StartCoroutine(EnergyRegen());
	}

	public IEnumerator EnergyRegen() {
		while (true) {
			energy++;

			yield return new WaitForSeconds(.01f);
		}
	}
}
