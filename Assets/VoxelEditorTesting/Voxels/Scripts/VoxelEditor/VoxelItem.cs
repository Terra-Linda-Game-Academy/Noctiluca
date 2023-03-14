using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoxelItemUpdatedDelegate(VoxelItem voxelItem, string variableName);

[CreateAssetMenu(fileName = "VoxelItem", menuName = "Voxel Editing/New Voxel")]
public class VoxelItem : ScriptableObject
{
    public event VoxelItemUpdatedDelegate OnVoxelItemUpdated;

    [SerializeField] private VoxelCategoryType _voxelCategory;
    [SerializeField] private Texture2D _icon;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private  int _id;
    [SerializeField] private GameObject _prefab;

    [SerializeField] private Vector3 _scale = Vector3.one;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    public VoxelCategoryType voxelCategory
    {
        get { return _voxelCategory; }
        set
        {
            if(_voxelCategory != value) {
                _voxelCategory = value;
                TriggerUpdateEvent("voxelCategory");
            }
                
            
        }
    }

    public Texture2D icon
    {
        get { return _icon; }
        set
        {
            if(_icon != value) {
                _icon = value;
                TriggerUpdateEvent("icon");
            }
                
            
        }
    }

    public Color color
    {
        get { return _color; }
        set
        {
            if(_color != value) {
                _color = value;
                TriggerUpdateEvent("color");
            }
                
            
        }
    }

    public int id
    {
        get { return _id; }
        set
        {
            if(_id != value) {
                _id = value;
                TriggerUpdateEvent("id");
            }
                
            
        }
    }

    public GameObject prefab
    {
        get { return _prefab; }
        set
        {
            if(_prefab != value) {
                _prefab = value;
                TriggerUpdateEvent("prefab");
            }
                
        }
    }

    public Vector3 scale
    {
        get { return _scale; }
        set
        {
            if(_scale != value) {
                _scale = value;
                TriggerUpdateEvent("scale");
            }
                
            
        }
    }
    public Vector3 offset
    {
        get { return _offset; }
        set
        {
            if(_offset != value) {
                _offset = value;
                TriggerUpdateEvent("offset");
            }
                
            
        }
    }

    private void TriggerUpdateEvent(string varaibleName)
    {
        if (OnVoxelItemUpdated != null)
        {
            OnVoxelItemUpdated(this, varaibleName);
        }
    }
}
