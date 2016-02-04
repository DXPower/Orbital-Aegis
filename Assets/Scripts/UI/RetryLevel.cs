using UnityEngine;
using System.Collections;

public class RetryLevel : MonoBehaviour {
	public void Click() {
		Level.self.RetryLevel();
	}
}
