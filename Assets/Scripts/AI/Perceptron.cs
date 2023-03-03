using UnityEngine;

namespace AI
{
    public class Perceptron : MonoBehaviour
    {
        public Transform eyes;

        public bool ForwardRaycast(GameObject target, float maxDist, LayerMask layerMask)
        {
            if (Physics.Raycast(eyes.position, eyes.forward, out RaycastHit hit, maxDist, layerMask))
            {
                return hit.transform == target.transform;
            }

            return false;
        }

        public bool VisionCone(GameObject target, float maxDist, float maxAngle, LayerMask layerMask)
        {
            Vector3 toTarget = target.transform.position - eyes.position;
            if (toTarget.magnitude > maxDist) return false;

            if (Vector3.Angle(eyes.forward, toTarget) <= maxAngle)
            {
                return !Physics.Raycast(eyes.position, toTarget.normalized, toTarget.magnitude, layerMask);
            }

			return false;
		}

		public bool InCircleRange(GameObject target, float radius) =>
			InCircleRangeOfPoint(target, transform.position, radius);


		public bool InCircleRangeOfPoint(GameObject target, Vector3 point, float radius) {
			return (target.transform.position - point).magnitude <= radius;
		}

		public bool InRectRange(GameObject target, Vector3 radii) => InRectRange(target, radii.x, radii.y, radii.z);

		public bool InRectRange(GameObject target, float rx, float ry, float rz) =>
			InRectRangeOfPoint(target, transform.position, rx, ry, rz);

		public bool InRectRangeOfPoint(GameObject target, Vector3 point, Vector3 radii) =>
			InRectRangeOfPoint(target, point, radii.x, radii.y, radii.z);

		public bool InRectRangeOfPoint(GameObject target, Vector3 point, float rx, float ry, float rz) {
			Vector3 targetPos = target.transform.position;

			return Mathf.Abs(targetPos.x - point.x) <= rx
			    && Mathf.Abs(targetPos.y - point.y) <= ry
			    && Mathf.Abs(targetPos.z - point.z) <= rz;
		}
	}
}