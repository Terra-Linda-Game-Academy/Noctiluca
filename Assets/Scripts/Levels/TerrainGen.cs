using System.Runtime.InteropServices;
using UnityEngine;

namespace Levels {
    public static class TerrainGen {
        [StructLayout(LayoutKind.Sequential)]
        private struct GeneratedVertex {
            public Vector3 position;
            public Vector3 normal;
            public Vector2 uv;
        }

        private const int GeneratedVertStride = sizeof(float) * (3 + 3 + 2);
        private const int GeneratedIndexStride = sizeof(int);
        private const int HeightMapStride = sizeof(sbyte);

        private static ComputeShader shader = Resources.Load<ComputeShader>("GenerateTerrain");

        public static Mesh Run(Vector3Int roomDimensions, sbyte[] heightMap) {
            int tileCount = 2;

            GeneratedVertex[] generatedVertices = new GeneratedVertex[tileCount * 4];
            int[] generatedTris = new int[tileCount * 6];

            GraphicsBuffer vertBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured, 
                generatedVertices.Length, 
                GeneratedVertStride
            );

            GraphicsBuffer triBuffer = new GraphicsBuffer(
                GraphicsBuffer.Target.Structured,
                generatedTris.Length,
                GeneratedIndexStride
            );

            ComputeBuffer heightBuffer = new ComputeBuffer(
                heightMap.Length,
                HeightMapStride
            );

            int idTerrainKernel = shader.FindKernel("generateTerrain");
            
            shader.SetBuffer(idTerrainKernel, "_GeneratedVertices", vertBuffer);
            shader.SetBuffer(idTerrainKernel, "_GeneratedTris", triBuffer);
            
            shader.SetVector( "_RoomDimensions", new Vector4(roomDimensions.x, roomDimensions.y, roomDimensions.z));            
            shader.SetBuffer(idTerrainKernel, "_HeightMap", heightBuffer);
            
            heightBuffer.SetData(heightMap);
            
            shader.Dispatch(idTerrainKernel, roomDimensions.x, roomDimensions.y, 1);
            
            vertBuffer.GetData(generatedVertices); //todo: AsyncGPUReadback
            triBuffer.GetData(generatedTris);
            
            return null;
        }
    }
}