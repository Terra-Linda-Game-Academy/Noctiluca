using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Levels {
    public static class TerrainGen {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex {
            public Vector3 position;
            public Vector3 normal;
            public Vector2 uv;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Triangle {
            public Vertex a;
            public Vertex b;
            public Vertex c;
        }

        private const int GeneratedTriStride = sizeof(float) * 3 * (3 + 3 + 2);
        private const int HeightMapStride = sizeof(int);

        private static ComputeShader shader = Resources.Load<ComputeShader>("GenerateTerrain");

        public static Mesh Run(Vector3Int roomDimensions, sbyte[] heightMap) {
            int triCount = 2 * roomDimensions.x * roomDimensions.z;
            
            GraphicsBuffer triBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Append,
                triCount,
                GeneratedTriStride
            );

            GraphicsBuffer heightBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                heightMap.Length,
                HeightMapStride
            );

            int idTerrainKernel = shader.FindKernel("generateTerrain");
            
            shader.SetBuffer(idTerrainKernel, "Triangles", triBuffer);
            
            shader.SetVector( "RoomDimensions", new Vector4(roomDimensions.x, roomDimensions.y, roomDimensions.z));            
            shader.SetBuffer(idTerrainKernel, "HeightMap", heightBuffer);

            heightBuffer.SetData(Array.ConvertAll(heightMap, e => (int) e));
            
            shader.Dispatch(
                idTerrainKernel, 
                Mathf.CeilToInt(roomDimensions.x / 16.0f), 
                1,
                Mathf.CeilToInt(roomDimensions.z / 16.0f)
            );
            heightBuffer.Release();

            var dataReq = AsyncGPUReadback.Request(triBuffer);
            dataReq.WaitForCompletion();
            NativeArray<Triangle> tris = dataReq.GetData<Triangle>();
            triBuffer.Release();
            
            
            Vector3[] verts = new Vector3[triCount * 3];
            Vector3[] normals = new Vector3[triCount * 3];
            int[] indices = new int[triCount * 3];

            for (int i = 0; i < triCount; i++) {
                int threeI = i * 3;
                verts[threeI] = tris[i].a.position;
                verts[threeI + 1] = tris[i].b.position;
                verts[threeI + 2] = tris[i].c.position;

                normals[threeI] = tris[i].a.normal;
                normals[threeI + 1] = tris[i].b.normal;
                normals[threeI + 2] = tris[i].c.normal;
                
                indices[threeI] = threeI;
                indices[threeI + 1] = threeI + 1;
                indices[threeI + 2] = threeI + 2;
            }

            Mesh mesh = new Mesh();
            
            mesh.SetVertices(verts);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);

            Debug.Log($"Finished Generating mesh with {triCount} triangles!");
            
            return mesh;
        }
    }
}