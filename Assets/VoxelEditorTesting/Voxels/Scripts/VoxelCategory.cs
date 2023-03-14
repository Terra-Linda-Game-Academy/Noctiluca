using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoxelCategory {
    public string name;
    public Texture2D icon;
    public List<VoxelItem> items;

    public VoxelCategoryType voxelCategoryType;

    public VoxelCategory(string name, Texture2D icon, VoxelCategoryType voxelCategoryType, List<VoxelItem> items) {
        this.name = name;
        this.icon = icon;
        this.voxelCategoryType = voxelCategoryType;
        this.items = items;
    }   
}