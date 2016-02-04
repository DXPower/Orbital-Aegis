using UnityEngine;
using System.Collections;

public static class Goal {
	public enum ActivationType {
		Player,
		Junk,
		Fired,
		None
	}

	public static ActivationType activationType;
}
