using UnityEngine;

namespace Util {
    // Integer numeric types
    
    [CreateAssetMenu(fileName = "SByte Runtime Variable", menuName = "Utilities/Runtime Variables/SByte", order = 0)]
    public class SByteRuntimeVar : RuntimeVar<sbyte> { }
    
    [CreateAssetMenu(fileName = "Byte Runtime Variable", menuName = "Utilities/Runtime Variables/Byte", order = 1)]
    public class ByteRuntimeVar : RuntimeVar<byte> { }
    
    
    [CreateAssetMenu(fileName = "Short Runtime Variable", menuName = "Utilities/Runtime Variables/Short", order = 2)]
    public class ShortRuntimeVar : RuntimeVar<short> { }
    
    
    [CreateAssetMenu(fileName = "UShort Runtime Variable", menuName = "Utilities/Runtime Variables/UShort", order = 3)]
    public class UShortRuntimeVar : RuntimeVar<ushort> { }
    
    
    [CreateAssetMenu(fileName = "Int Runtime Variable", menuName = "Utilities/Runtime Variables/Int", order = 4)]
    public class IntRuntimeVar : RuntimeVar<int> { }
    
    
    [CreateAssetMenu(fileName = "UInt Runtime Variable", menuName = "Utilities/Runtime Variables/UInt", order = 5)]
    public class UIntRuntimeVar : RuntimeVar<uint> { }
    
    
    [CreateAssetMenu(fileName = "Long Runtime Variable", menuName = "Utilities/Runtime Variables/Long", order = 6)]
    public class LongRuntimeVar : RuntimeVar<long> { }
    
    
    [CreateAssetMenu(fileName = "ULong Runtime Variable", menuName = "Utilities/Runtime Variables/ULong", order = 7)]
    public class ULongRuntimeVar : RuntimeVar<ulong> { }
    
    // Floating-point numeric types
    
    
    [CreateAssetMenu(fileName = "Float Runtime Variable", menuName = "Utilities/Runtime Variables/Float", order = 8)]
    public class FloatRuntimeVar : RuntimeVar<float> { }
    
    
    [CreateAssetMenu(fileName = "Double Runtime Variable", menuName = "Utilities/Runtime Variables/Double", order = 9)]
    public class DoubleRuntimeVar : RuntimeVar<double> { }
    
    // Text types
    
    
    [CreateAssetMenu(fileName = "Char Runtime Variable", menuName = "Utilities/Runtime Variables/Char", order = 10)]
    public class CharRuntimeVar : RuntimeVar<char> { }
    
    
    [CreateAssetMenu(fileName = "String Runtime Variable", menuName = "Utilities/Runtime Variables/String", order = 11)]
    public class StringRuntimeVar : RuntimeVar<string> { }
    
    // Unity struct types
    
    
    [CreateAssetMenu(fileName = "Vector2 Runtime Variable", menuName = "Utilities/Runtime Variables/Vector2", order = 12)]
    public class Vector2RuntimeVar : RuntimeVar<Vector2> { }
    
    
    [CreateAssetMenu(fileName = "Vector2Int Runtime Variable", menuName = "Utilities/Runtime Variables/Vector2Int", order = 13)]
    public class Vector2IntRuntimeVar : RuntimeVar<Vector2Int> { }
    
    
    [CreateAssetMenu(fileName = "Vector3 Runtime Variable", menuName = "Utilities/Runtime Variables/Vector3", order = 14)]
    public class Vector3RuntimeVar : RuntimeVar<Vector3> { }
    
    
    [CreateAssetMenu(fileName = "Vector3Int Runtime Variable", menuName = "Utilities/Runtime Variables/Vector3Int", order = 15)]
    public class Vector3IntRuntimeVar : RuntimeVar<Vector3Int> { }
    
    
    [CreateAssetMenu(fileName = "Vector4 Runtime Variable", menuName = "Utilities/Runtime Variables/Vector4", order = 16)]
    public class Vector4RuntimeVar : RuntimeVar<Vector4> { }
    
    
    [CreateAssetMenu(fileName = "Quaternion Runtime Variable", menuName = "Utilities/Runtime Variables/Quaternion", order = 17)]
    public class QuaternionRuntimeVar : RuntimeVar<Quaternion> { }


    //implement more as necessary, labelling types appropriately and adding a CreateAssetMenu attribute
}