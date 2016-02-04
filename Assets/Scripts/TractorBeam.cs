using UnityEngine;
using System.Collections;

public class TractorBeam : MonoBehaviour {
	public GameObject tracer;
	public static GameObject tractoredObject;

	public Vector3 relativeRotation;

	public Material tractoredMaterial;

	private LineRenderer beam;

	public int pushForce;
	public int tractorBeamDistance = 6;

	private float originalDistance = 0;
	
	private bool isOnCooldown = false;
	private bool beamOn = false;

	void Start() {
		beam = GetComponent<LineRenderer>();
	}

	void Update() {
		if (Level.gameRunning && !Tutorial.beamLock) {
			beamOn = false;
			if (Input.GetMouseButton(0) && !isOnCooldown) {
				beamOn = true;

				if (Input.GetMouseButtonDown(1) && tractoredObject != null && !Tutorial.fireLock) {
					if (Tutorial.fireCheck) Tutorial.AdvanceTutorial();

					tractoredObject.GetComponent<Junk>().SetMaterial(null).Fire().rigidbody.velocity += ((tractoredObject.transform.position - transform.position).normalized * pushForce);
					tractoredObject = null;
					beamOn = false;
					StartCoroutine(Cooldown());
				}
			} else if (tractoredObject != null) {
				tractoredObject.GetComponent<Junk>().SetMaterial(null).StopTractor();
				tractoredObject = null;
			}

			if (Input.GetMouseButtonUp(0) && Tutorial.checkBeam) Tutorial.AdvanceTutorial();
		}
	}

	void FixedUpdate() {
		if (tractoredObject == null && beamOn) {
			Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.right));
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, tractorBeamDistance) && hit.collider.CompareTag("Junk") && hit.collider.gameObject != tractoredObject) {
				tractoredObject = hit.collider.gameObject;
				tractoredObject.GetComponent<Junk>().StartTractor().SetMaterial(tractoredMaterial);
			}
		}

		PositionBeam();
	}


	private void PositionBeam() {
		if (beamOn) {
			if (!beam.enabled) beam.enabled = true;
			if (tractoredObject != null) beam.SetPositions(new Vector3[] { transform.position + transform.right, transform.position + transform.right * Vector3.Distance(transform.position, tractoredObject.transform.position) });
			else {
				beam.SetPositions(new Vector3[] { transform.position + transform.right, transform.position + transform.right * tractorBeamDistance });
			}
		} else {
			if (beam.enabled) beam.enabled = false;
		}
	}

	IEnumerator Cooldown() {
		isOnCooldown = true;
		yield return new WaitForSeconds(1f);
		isOnCooldown = false;
	}
}
