using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelPalette
{

    public static List<VoxelCategory> categories = new List<VoxelCategory>();

    public static VoxelCategory LogicCategory;
    public static VoxelCategory BlockCategory;
    public static VoxelCategory EntityCategory;
    public static void InitCategories()
    {

        VoxelPalette.categories.Clear();;

        //logicItems.Add(new VoxelItem("Exit", Resources.Load<Texture2D>("icons/server-icon") , "1"));

        LogicCategory = new VoxelCategory(
            "Logic",
            Resources.Load<Texture2D>("icons/logic-icon"),
            VoxelCategoryType.LOGIC,
            new List<VoxelItem>()
        );

        //blockItems.Add(new VoxelItem("Cobblestone", Resources.Load<Texture2D>("icons/voxel-icon") , "1"));

        BlockCategory = new VoxelCategory(
            "Blocks",
            Resources.Load<Texture2D>("icons/voxel-icon"),
            VoxelCategoryType.BLOCKS,
            new List<VoxelItem>()
        );

        EntityCategory = new VoxelCategory(
            "Entities",
            Resources.Load<Texture2D>("icons/entity-icon"),
            VoxelCategoryType.ENTITIES,
            new List<VoxelItem>()
        );

        InitItems();
        
        VoxelPalette.categories.Add(LogicCategory);
        VoxelPalette.categories.Add(BlockCategory);
        VoxelPalette.categories.Add(EntityCategory);

        
    }


    private static void InitItems()
    {
        
        foreach (VoxelCategoryType value in VoxelCategoryType.GetValues(typeof(VoxelCategoryType))) {
            VoxelItem[] voxels = Resources.LoadAll<VoxelItem>("voxels/"+value.ToString());
            switch(value) {
                case VoxelCategoryType.LOGIC:
                    LogicCategory.items.AddRange(voxels);
                    break;
                case VoxelCategoryType.BLOCKS:
                    BlockCategory.items.AddRange(voxels);
                    break;
                case VoxelCategoryType.ENTITIES:
                    EntityCategory.items.AddRange(voxels);
                    break;
            }
        }
    }

   
}
