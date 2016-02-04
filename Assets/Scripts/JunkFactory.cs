using UnityEngine;
using System.Collections;

public class JunkFactory : MonoBehaviour {
	public GameObject junkPrefab;
	public GameObject topClamp;
	public GameObject bottomClamp;
	public GameObject toTopClamp;
	public GameObject toBottomClamp;
	public static GameObject junkParent;
	public GameObject _junkParent;

	public float junkVelocityMin;
	public float junkVelocityMax;
	public float junkRotationalVelocityMin;
	public float junkRotationalVelocityMax;
	public float junkSizeMin;
	public float junkSizeMax;
	public float creationDelay;

	public bool factoryEnabled;

	void Start() {
		junkParent = _junkParent;
		//junkPrefab = Level.self.junkPrefab;
		if (factoryEnabled) StartCoroutine(CreatorLoop());
	}

	public IEnumerator CreatorLoop() {
		while (true) {
			if (Level.gameRunning) {
				float f = Random.Range(0f, 1f);
				float f2 = Random.Range(0f, 1f);
				GameObject junk = (GameObject) Instantiate(junkPrefab, Vector3.Lerp(topClamp.transform.position, bottomClamp.transform.position, f), junkPrefab.transform.rotation);
				junk.GetComponent<Rigidbody>().velocity = (Vector3.Lerp(toTopClamp.transform.position, toBottomClamp.transform.position, f2) - junk.transform.position).normalized * Random.Range(junkVelocityMin, junkVelocityMax);
				junk.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, Random.Range(junkRotationalVelocityMin, junkRotationalVelocityMax));
				float r = Random.Range(junkSizeMin, junkSizeMax);
				junk.transform.localScale = new Vector3(r, r, r);
				junk.GetComponent<Rigidbody>().mass = r;
				junk.transform.SetParent(junkParent.transform);
			}

			yield return new WaitForSeconds(creationDelay);
		}
	}

	public static void ClearJunk() {
		foreach (Transform child in junkParent.transform) {
			Destroy(child.gameObject);
		}

		Destroy(TractorBeam.tractoredObject);
	}
}
