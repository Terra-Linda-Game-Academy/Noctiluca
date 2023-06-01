using UnityEngine;

namespace Util {
	public static class AABB {
		public static bool Overlap(Vector2 min1, Vector2 max1, Vector2 min2, Vector2 max2) {
			return min1.x <= max2.x && max1.x >= min2.x && min1.y <= max2.y && max1.y >= min2.y;
		}
	}
}