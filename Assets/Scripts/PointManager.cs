using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Points {
	public enum PointType {
		tractor,
		junkCollision,
		issHit,
		issHealth
	}

	public static class PointManager {
		private static Dictionary<PointType, int> points = new Dictionary<PointType, int>();

		public static void AddPointTypes(PointType[] pts) {
			foreach (PointType pt in pts) {
				points.Add(pt, 0);
			}
		}

		private static void AddPointsToDictionary(PointType pt, int value) {
			if (points.ContainsKey(pt)) {
				points[pt] += value;
			}
		}

		public static void AddPoints(PointType pt, int value) {
			AddPointsToDictionary(pt, value);
		}

		public static void AddPoints(PointType pt, int value, GameObject go, Color color) {
			string sign = value < 0 ? "" : "+";
			PointFloater.Factory(go, sign + value, color);
			AddPointsToDictionary(pt, value);
		}

		public static void AddPoints(PointType pt, int value, Vector3 screenPos, Color color) {
			string sign = value < 0 ? "" : "+";
			PointFloater.Factory(screenPos, sign + value, color);
			AddPointsToDictionary(pt, value);
		}

		public static void ResetPoints() {
			foreach (PointType pt in points.Keys) {
				points[pt] = 0;
			}
		}

		public static string GetPointTypeString(PointType pt) {
			switch (pt) {
				case PointType.tractor:
					return "Extra";
				case PointType.junkCollision:
					return "Extra";
				case PointType.issHealth:
					return "Objectives";
				case PointType.issHit:
					return "Penalties";
				default:
					return "Extra";
			}
		}

		public static int GetTotalPoints() {
			int total = 0;

			foreach (int o in points.Values) {
				total += o;
			}

			return total;
		}

		public static Dictionary<PointType, int> GetPoints() {
			return points;
		}
	}
}
