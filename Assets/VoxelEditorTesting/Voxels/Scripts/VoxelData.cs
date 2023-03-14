using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData {
    public VoxelItem voxelItem;

    public Vector3Int position;
    public Vector3Int rotation;

    public VoxelData(VoxelItem voxelItem, Vector3Int position, Vector3Int rotation) {
        this.voxelItem = voxelItem;
        this.position = position;
        this.rotation = rotation;
    }
}