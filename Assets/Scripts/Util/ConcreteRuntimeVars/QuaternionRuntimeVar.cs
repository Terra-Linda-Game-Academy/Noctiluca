using UnityEngine;

namespace Util.ConcreteRuntimeVars {
    [CreateAssetMenu(fileName = "Quaternion Runtime Variable", menuName = "Utilities/Runtime Variables/Quaternion", order = 17)]
    public class QuaternionRuntimeVar : RuntimeVar<Quaternion> { }
}