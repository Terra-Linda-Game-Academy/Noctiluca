using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels {
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class TerrainTest : MonoBehaviour {
        private MeshFilter meshFilter;

        [SerializeField] private Vector3Int dimensions = new Vector3Int(10, 10, 10);
        private sbyte[] heightMap;

        private void Awake() {
            meshFilter = GetComponent<MeshFilter>();
            
            int newHeightMapLength = dimensions.x * dimensions.z;
            heightMap = new sbyte[newHeightMapLength];
            for (int i = 0; i < newHeightMapLength; i++) {
                //heightMap[i] = (sbyte) Random.Range(0, 10);
                heightMap[i] = (sbyte) (i % 32);
            }
            
            meshFilter.sharedMesh = TerrainGen.Run(dimensions, heightMap);
        }

        /*private void Update() {
            foreach (Vector3 vert in meshFilter.sharedMesh.vertices)
            {
                Debug.DrawLine(vert, vert + Vector3.one);
            }
        }*/
    }
}