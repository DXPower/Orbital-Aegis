using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	public float thrustForce;
	public float torqueForce;
	public float drag;
	public float angularDrag;
	public float maxVelocity;
	public float maxAngularVelocity;

	private float originalDrag;

	private bool hasEnded = false;

	private Vector3 thrustVector = Vector3.zero;
	private Vector3 torqueVector = Vector3.zero;

	public static GameObject root;

	private new Rigidbody rigidbody;

	public static ShipController self;

	void Start() {
		self = this;
		root = GameObject.Find("PlayerShipRoot");

		rigidbody = GetComponent<Rigidbody>();
		originalDrag = drag;
		rigidbody.drag = originalDrag;
		rigidbody.angularDrag = angularDrag;
		rigidbody.maxAngularVelocity = maxAngularVelocity;

		
	}

	void Update() {
		if (Level.gameRunning) {
			thrustVector = Vector3.zero;
			torqueVector = Vector3.zero;

			if (Input.GetKey(KeyCode.W)) {
				thrustVector += new Vector3(thrustForce, 0, 0);
			}

			if (Input.GetKey(KeyCode.S) && !Tutorial.sLock) {
				thrustVector += new Vector3(-thrustForce * .5f, 0, 0);
			}
			/*
			if (Input.GetKey(KeyCode.A)) {
				torqueVector += new Vector3(0, 0, torqueForce);
			}

			if (Input.GetKey(KeyCode.D)) {
				torqueVector += new Vector3(0, 0, -torqueForce);
			}*/

			if (!Tutorial.turnLock) LookAtMouse();
			//	ApplyThrustVectors();
		} else if (!hasEnded) SlowDown();
	}

	private void SlowDown() {
		rigidbody.drag = 5;
		drag = 5;
		angularDrag = 3;
	}

	public static void NormalSpeed() {
		self.drag = self.originalDrag;
		self.rigidbody.angularDrag = self.angularDrag;
	}

	private void LookAtMouse() {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));

		//Rotates toward the mouse
		rigidbody.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg);
	}

	void FixedUpdate() {
		if (Level.gameRunning) {
			ApplyThrustVectors();
			Vector3 v = rigidbody.velocity;
			LimitVelocity(ref v, maxVelocity);
			rigidbody.velocity = v;
			root.transform.rotation = transform.rotation;
			root.transform.position = transform.position;
		}
	}

	private void ApplyThrustVectors() {
		if (thrustVector != Vector3.zero) {
			rigidbody.AddRelativeForce(thrustVector * Time.fixedDeltaTime);
			rigidbody.drag = 0;
		} else {
			rigidbody.drag = drag;
		}

		if (torqueVector != Vector3.zero) {
			rigidbody.AddRelativeTorque(torqueVector * Time.fixedDeltaTime);
			rigidbody.angularDrag = 0;
		} else {
			rigidbody.angularDrag = angularDrag;
		}	
	}

	public static void LimitVelocity(ref Vector3 velocity, float max) {
		if (velocity.sqrMagnitude > Mathf.Pow(max, 2)) {
			velocity = velocity.normalized * max;
		}
	}

	void OnTriggerEnter(Collider c) {
		if (c.CompareTag("Goal") && Goal.activationType == Goal.ActivationType.Player) {
			SlowDown();
			Tutorial.AdvanceTutorial();
		}
	}
}
