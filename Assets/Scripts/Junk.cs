using UnityEngine;
using System.Collections;
using Points;

public class Junk : MonoBehaviour {
	private bool isTractored = false;
	private bool hasEnded = false;
	private bool claimedPoint = false;
	private bool hasHit = false;
	private bool wasFired = false;

	public new Rigidbody rigidbody;

	private Renderer r;

	private Material originalMaterial;

	private Vector3 previousPosition = Vector3.zero;
	private Vector3 currentVelocity = Vector3.zero;

	void Start() {
		rigidbody = GetComponent<Rigidbody>();
		r = GetComponent<Renderer>();
		//originalMaterial = r.material;
		r.enabled = false;
		
	}

	void FixedUpdate() {
		if (!Level.gameRunning && !hasEnded) SlowDown();

		if (isTractored) {
			currentVelocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
			previousPosition = transform.position;
		}
	}

	private void SlowDown() {
		rigidbody.drag = 5;
		rigidbody.angularDrag = 3;
		hasEnded = true;
	}

	public Junk Fire() {
		wasFired = true;
		StopTractor();
		return this;
	}

	public Junk StartTractor() {
		rigidbody.velocity = Vector3.zero;
		rigidbody.isKinematic = true;
		transform.SetParent(ShipController.root.transform);
		r.enabled = true;
		isTractored = true;
		wasFired = false;

		if (claimedPoint) return this;

		PointManager.AddPoints(PointType.tractor, Random.Range(1, 100) <= 75 ? 1 : 2, gameObject, Color.green);
		claimedPoint = true;

		return this;
	}

	public Junk StopTractor() {
		isTractored = false;
		rigidbody.isKinematic = false;
		transform.SetParent(JunkFactory.junkParent.transform);
		ShipController.LimitVelocity(ref currentVelocity, 10);
		rigidbody.velocity = currentVelocity;
		r.enabled = false;
		return this;
	}

	public bool GetIsTractored() {
		return isTractored;
	}

	public Junk SetMaterial(Material material) {
		//r.material = material == null ? originalMaterial : material;
		r.material = material;
		return this;
	}

	void OnTriggerEnter(Collider c) {
		if (c.CompareTag("JunkDestroyer")) {
			Destroy(gameObject);
		} else if (c.CompareTag("ISS") && !isTractored && !hasHit) {
			hasHit = true;
			ISS.Hit((int) Mathf.Floor(rigidbody.mass * 10), gameObject);
			Destroy(gameObject);
		} else if (c.CompareTag("Goal")) {
			if (Goal.activationType == Goal.ActivationType.Junk || (Goal.activationType == Goal.ActivationType.Fired && wasFired)) {
				Tutorial.AdvanceTutorial();
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Junk") && wasFired) {
			PointManager.AddPoints(PointType.junkCollision, 5, Camera.main.WorldToScreenPoint(Vector3.Lerp(collision.transform.position, collision.collider.transform.position, .5f)), Color.green);
		}

		wasFired = false;
	}
}
