using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Levels {
    public static class TerrainMesh {
        public static Mesh Generate(Room room) {
            List<Vertex> vertices = new List<Vertex>();
            List<ushort> topIndices = new List<ushort>();
            List<ushort> sideIndices = new List<ushort>();

            //start at -1 because we need to consider the boundaries of the room
            for (int x = 0; x < room.Size.x; x++) { //todo: change back to <= when making full generation
                for (int z = 0; z < room.Size.z; z++) {
                    Room.Tile tile = room.GetTileAt(x, z);
                    if (tile.flags.HasFlag(Room.TileFlags.Wall | Room.TileFlags.Pit)) break;

                    void AddIndices(List<ushort> indices, int c, params ushort[] offsets) {
                        foreach (var o in offsets) indices.Add((ushort) (c + o));
                    }
                    AddIndices(topIndices, vertices.Count,0, 2, 1,  1, 2, 3);
                    vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.up, Vector2.zero));

                    Room.Tile tileNX = room.GetTileAt(x - 1, z);
                    if (tileNX.flags.HasFlag(Room.TileFlags.Wall)) {
                        AddIndices(sideIndices, vertices.Count,0, 1, 2,  1, 3, 2);
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, room.Size.y, z), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, room.Size.y, z + 1), Vector3.right, Vector2.zero));
                    } else if (tileNX.flags.HasFlag(Room.TileFlags.Pit)) {
                        AddIndices(sideIndices, vertices.Count, 0, 2, 1,  1, 2, 3);
                        vertices.Add(new Vertex(new Vector3(x, 0, z), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, 0, z + 1), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.left, Vector2.zero));
                    } else if (tileNX.Height < tile.Height) {
                        AddIndices(sideIndices, vertices.Count,0, 2, 1,  1, 2, 3);
                        vertices.Add(new Vertex(new Vector3(x, tileNX.Height, z), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tileNX.Height, z + 1), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.left, Vector2.zero));
                    }
                    
                    Room.Tile tilePX = room.GetTileAt(x + 1, z);
                    if (tilePX.flags.HasFlag(Room.TileFlags.Wall)) {
                        AddIndices(sideIndices, vertices.Count,0, 2, 1,  1, 2, 3);
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, room.Size.y, z), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.left, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, room.Size.y, z + 1), Vector3.left, Vector2.zero));
                    } else if (tilePX.flags.HasFlag(Room.TileFlags.Pit)) {
                        AddIndices(sideIndices, vertices.Count, 0, 1, 2, 1, 3, 2);
                        vertices.Add(new Vertex(new Vector3(x + 1, 0, z), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, 0, z + 1), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.right, Vector2.zero));
                    } else if (tilePX.Height < tile.Height) {
                        AddIndices(sideIndices, vertices.Count,0, 1, 2,  1, 3, 2);
                        vertices.Add(new Vertex(new Vector3(x + 1, tilePX.Height, z), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tilePX.Height, z + 1), Vector3.right, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.right, Vector2.zero));
                    }
                    
                    Room.Tile tileNZ = room.GetTileAt(x, z - 1);
                    if (tileNZ.flags.HasFlag(Room.TileFlags.Wall)) {
                        AddIndices(sideIndices, vertices.Count,0, 2, 1,  1, 2, 3);
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, room.Size.y, z), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, room.Size.y, z), Vector3.forward, Vector2.zero));
                        
                    } else if (tileNZ.flags.HasFlag(Room.TileFlags.Pit)) {
                        AddIndices(sideIndices, vertices.Count, 0, 1, 2, 1, 3, 2);
                        vertices.Add(new Vertex(new Vector3(x, 0, z), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, 0, z), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.back, Vector2.zero));
                    } else if (tileNZ.Height < tile.Height) {
                        AddIndices(sideIndices, vertices.Count,0, 1, 2,  1, 3, 2);
                        vertices.Add(new Vertex(new Vector3(x, tileNZ.Height, z), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tileNZ.Height, z), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z), Vector3.back, Vector2.zero));
                    }
                    
                    Room.Tile tilePZ = room.GetTileAt(x, z + 1);
                    if (tilePZ.flags.HasFlag(Room.TileFlags.Wall)) {
                        AddIndices(sideIndices, vertices.Count,0, 1, 2,  1, 3, 2);
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, room.Size.y, z + 1), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.back, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, room.Size.y, z + 1), Vector3.back, Vector2.zero));
                    } else if (tilePZ.flags.HasFlag(Room.TileFlags.Pit)) {
                        AddIndices(sideIndices, vertices.Count, 0, 2, 1, 1, 2, 3);
                        vertices.Add(new Vertex(new Vector3(x, 0, z + 1), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, 0, z + 1), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.forward, Vector2.zero));
                    } else if (tilePZ.Height < tile.Height) {
                        AddIndices(sideIndices, vertices.Count,0, 2, 1,  1, 2, 3);
                        vertices.Add(new Vertex(new Vector3(x, tilePZ.Height, z + 1), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x, tile.Height, z + 1), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tilePZ.Height, z + 1), Vector3.forward, Vector2.zero));
                        vertices.Add(new Vertex(new Vector3(x + 1, tile.Height, z + 1), Vector3.forward, Vector2.zero));
                    }
                }
            }

            int vertexCount = vertices.Count,
                topIndexCount = topIndices.Count,
                sideIndexCount = sideIndices.Count;

            var mesh = new Mesh { bounds = new Bounds { min = Vector3.zero, max = room.Size } };

            mesh.SetVertexBufferParams(vertexCount,
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
            );
            mesh.SetVertexBufferData(
                vertices, 0, 0, vertexCount, 0, 
                MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );
            
            mesh.SetIndexBufferParams(topIndexCount + sideIndexCount, IndexFormat.UInt16);
            mesh.SetIndexBufferData(
                topIndices, 0, 0, topIndexCount, 
                MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );
            mesh.SetIndexBufferData(
                sideIndices, 0, topIndexCount, sideIndexCount, 
                MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds
            );

            mesh.subMeshCount = 2;
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, topIndexCount));
            mesh.SetSubMesh(1, new SubMeshDescriptor(topIndexCount, sideIndexCount));

            mesh.Optimize();
            return mesh;
        }

        /*private struct GenerationParams {
            public int x, z;
            public Room.Tile t0, t1, t2, t3;
            public List<Vertex> vertices;
            public List<ushort> indices;
            public float roomHeight;
        }*/

        /*private static void GenerateTile(GenerationParams gen) {
            byte tileType = (byte) (
                (byte) gen.t0.flags | 
                (byte) gen.t1.flags << 2 | 
                (byte) gen.t2.flags << 4 | 
                (byte) gen.t3.flags << 6
            );

            List<Vertex> vertices = gen.vertices;
            List<ushort> indices = gen.indices;
            
            int c = vertices.Count;
            void AddIndices(params ushort[] offsets) {
                foreach(ushort o in offsets) indices.Add((ushort) (c + o));
            }

            //todo: oh my
            switch (tileType) {
                //todo: add all cases and a better system for handling them!!!
                //only terrain
                case 0b00000000: 
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 2, 1,  1, 2, 3);
                    break;
                //out corners
                case 0b00000001:
                    /*vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z - 0.5f), Vector3.up, Vector2.zero));

                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 1, 2,  1, 2, 3,  4, 5, 6,  4, 5, 7,  5, 7, 8);
                    break;#1#
                case 0b00000100:
                    /*vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 1, 2,  1, 2, 3,  4, 5, 6,  4, 5, 7,  5, 7, 8);#1#
                    break;
                case 0b00010000:
                    /*vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 1, 2,  1, 2, 3,  4, 5, 6,  4, 5, 7,  5, 7, 8);
                    break;#1#
                case 0b01000000:
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 2, 1,  1, 2, 3,  5, 7, 8,  4, 7, 5,  6, 8, 7);
                    break;
                //sides
                case 0b00000101:
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 2, 1,  1, 2, 3,  4, 6, 5,  6, 7, 5);
                    break;
                case 0b01000100:
                    /*vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t1.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.roomHeight, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.roomHeight, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t1.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 2, 1,  1, 2, 3,  4, 6, 5,  6, 7, 5);#1#
                    break;
                case 0b01010000:
                case 0b00010001:
                    break;
                //in corners
                case 0b01010100:
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 1, 2,  1, 3, 2,  4, 5, 6);
                    break;
                case 0b01010001:
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 2, 1,  1, 2, 3,  4, 6, 5);
                    break;
                case 0b01000101:
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 2, 1,  1, 2, 3,  4, 6, 5);
                    break;
                case 0b00010101:
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.roomHeight, gen.z), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.roomHeight, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    AddIndices(0, 1, 2,  1, 3, 2,  4, 5, 6);
                    break;
                //crosses (both are out)
                case 0b00010100:
                case 0b01000001:
                //base case
                case 0b01010101: break;
                    
                    
                /*case 0b00000000:
                    int c = vertices.Count;
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t0.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t1.Height, gen.z - 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x - 0.5f, gen.t2.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    vertices.Add(new Vertex(new Vector3(gen.x + 0.5f, gen.t3.Height, gen.z + 0.5f), Vector3.up, Vector2.zero));
                    
                    indices.Add((ushort) (c + 0));
                    indices.Add((ushort) (c + 2));
                    indices.Add((ushort) (c + 1));
                    indices.Add((ushort) (c + 1));
                    indices.Add((ushort) (c + 2));
                    indices.Add((ushort) (c + 3));
                    break;
                case 0b00000001:
                case 0b00000010:
                case 0b00000100:
                case 0b00000101:
                case 0b00000110:
                case 0b00001000:
                case 0b00001001:
                case 0b00001010:
                case 0b00010000:
                case 0b00010001:
                case 0b00010010:
                case 0b00010100:
                case 0b00010101:
                case 0b00010110:
                case 0b00011000:
                case 0b00011001:
                case 0b00011010:
                case 0b00100000:
                case 0b00100001:
                case 0b00100010:
                case 0b00100100:
                case 0b00100101:
                case 0b00100110:
                case 0b00101000:
                case 0b00101001:
                case 0b00101010:
                case 0b01000000:
                case 0b01000001:
                case 0b01000010:
                case 0b01000100:
                case 0b01000101:
                case 0b01000110:
                case 0b01001000:
                case 0b01001001:
                case 0b01001010:
                case 0b01010000:
                case 0b01010001:
                case 0b01010010:
                case 0b01010100:
                case 0b01010101: break;
                case 0b01010110:
                case 0b01011000:
                case 0b01011001:
                case 0b01011010:
                case 0b01100000:
                case 0b01100001:
                case 0b01100010:
                case 0b01100100:
                case 0b01100101:
                case 0b01100110:
                case 0b01101000:
                case 0b01101001:
                case 0b01101010:
                case 0b10000000:
                case 0b10000001:
                case 0b10000010:
                case 0b10000100:
                case 0b10000101:
                case 0b10000110:
                case 0b10001000:
                case 0b10001001:
                case 0b10001010:
                case 0b10010000:
                case 0b10010001:
                case 0b10010010:
                case 0b10010100:
                case 0b10010101:
                case 0b10010110:
                case 0b10011000:
                case 0b10011001:
                case 0b10011010:
                case 0b10100000:
                case 0b10100001:
                case 0b10100010:
                case 0b10100100:
                case 0b10100101:
                case 0b10100110:
                case 0b10101000:
                case 0b10101001:
                case 0b10101010: break;#1#
            }
        }

        

        private static void Plane(GenerationParams gen) {
            
        }

        private static void InCorner(GenerationParams gen) {
            
        }

        private static void OutCorner(GenerationParams gen) {
            
        }

        private static void Side(GenerationParams gen, bool top, bool bottom) {
            
        }

        private static void Cross(GenerationParams gen, bool _in, bool _out) { //todo: params don't cover case 0b00010110 or case 0b00101001
            
        }*/
    }
}