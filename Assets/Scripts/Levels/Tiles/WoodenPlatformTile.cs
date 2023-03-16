using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class WoodenPlatformTile : SimpleTile
    {
        public override string Name => name;
        [SerializeField] private string name;

        public override Vector3Int Position => position;
        [SerializeField] private Vector3Int position;

        public Vector2Int Size => size;
        [SerializeField] private Vector2Int size;

        public float Height => height;
        [SerializeField] private float height;

        public override void Init(GameObject obj)
        {
            obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            Mesh mesh = meshFilter.mesh;
            
            int length = size.x;
            int width = size.y;

            Vector3[] c = new Vector3[8];

            c[0] = new Vector3(-length * .5f, -width * .5f, height * .5f);
            c[1] = new Vector3(length * .5f, -width * .5f, height * .5f);
            c[2] = new Vector3(length * .5f, -width * .5f, -height * .5f);
            c[3] = new Vector3(-length * .5f, -width * .5f, -height * .5f);

            c[4] = new Vector3(-length * .5f, width * .5f, height * .5f);
            c[5] = new Vector3(length * .5f, width * .5f, height * .5f);
            c[6] = new Vector3(length * .5f, width * .5f, -height * .5f);
            c[7] = new Vector3(-length * .5f, width * .5f, -height * .5f);


            Vector3[] vertices = new Vector3[] {
	        c[7], c[4], c[0], c[3], // Left
	        c[4], c[5], c[1], c[0], // Front
	        c[6], c[7], c[3], c[2], // Back
	        c[5], c[6], c[2], c[1], // Right
	        c[7], c[6], c[5], c[4]  // Top
        };

            //5) Define each vertex's Normal
            Vector3 up = Vector3.up;
            Vector3 forward = Vector3.forward;
            Vector3 back = Vector3.back;
            Vector3 left = Vector3.left;
            Vector3 right = Vector3.right;

            Vector3[] normals = new Vector3[] {
	        left, left, left, left,             // Left
	        forward, forward, forward, forward,	// Front
	        back, back, back, back,             // Back
	        right, right, right, right,         // Right
	        up, up, up, up	                    // Top
        };

            Vector2 uv00 = new Vector2(0f, 0f);
            Vector2 uv10 = new Vector2(1f, 0f);
            Vector2 uv01 = new Vector2(0f, 1f);
            Vector2 uv11 = new Vector2(1f, 1f);

            Vector2[] uvs = new Vector2[] {
	        uv11, uv01, uv00, uv10, // Left
	        uv11, uv01, uv00, uv10, // Front
	        uv11, uv01, uv00, uv10, // Back	        
	        uv11, uv01, uv00, uv10, // Right 
	        uv11, uv01, uv00, uv10  // Top
            };

            int[] triangles = new int[] {
	        7, 5, 4,        7, 6, 5,        // Left
	        11, 9, 8,       11, 10, 9,      // Front
	        15, 13, 12,     15, 14, 13,     // Back
	        19, 17, 16,     19, 18, 17,	    // Right
	        23, 21, 20,     23, 22, 21,     // Top
            };

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.Optimize();
        }
    }
}
