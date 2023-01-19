using UnityEngine;

namespace AI {
	public class Perceptron : MonoBehaviour {
		public Transform eyes;

		public bool ForwardRaycast(GameObject target, float maxDist, LayerMask layerMask) {
			if (Physics.Raycast(eyes.position, eyes.forward, out RaycastHit hit, maxDist, layerMask)) {
				return hit.transform == target.transform;
			}

			return false;
		}

		public bool VisionCone(GameObject target, float maxDist, float maxAngle, LayerMask layerMask) {
			Vector3 toTarget = target.transform.position - eyes.position;
			if (toTarget.magnitude > maxDist) return false;

			if (Vector3.Angle(eyes.forward, toTarget) <= maxAngle) {
				return !Physics.Raycast(eyes.position, toTarget.normalized, toTarget.magnitude, layerMask);
			}

			return false;
		}
	}
}