using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class VoxelTools {
        // public static void SaveSceneVoxelsToFile(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix, string path) {
        //     // Create a new StringBuilder to store the data
        //     StringBuilder data = new StringBuilder();

        //     // Loop through each voxel position in the voxelMatrix
        //     foreach (Vector3Int position in voxelMatrix.Keys) {
        //         // Get the list of voxel data at the current position
        //         List<VoxelData> voxelDataList = voxelMatrix[position];
        //         if (voxelDataList.Count < 1)
        //             continue;

        //         // Create a new StringBuilder to store the voxel data
        //         StringBuilder voxelDataString = new StringBuilder();

        //         // Loop through each voxel data and append its id and rotation to the voxelDataString StringBuilder
        //         foreach (VoxelData voxelData in voxelDataList) {
        //             voxelDataString.Append(voxelData.voxelitem.id);
        //             Vector3 rotation = new Vector3(
        //                 Mathf.RoundToInt(voxelData.rotation.x / 90) * 90, // round x to nearest multiple of 90
        //                 Mathf.RoundToInt(voxelData.rotation.y / 90) * 90, // round y to nearest multiple of 90
        //                 Mathf.RoundToInt(voxelData.rotation.z / 90) * 90  // round z to nearest multiple of 90
        //             );
        //             voxelDataString.Append("(");
        //             voxelDataString.Append(rotation.x);
        //             voxelDataString.Append(",");
        //             voxelDataString.Append(rotation.y);
        //             voxelDataString.Append(",");
        //             voxelDataString.Append(rotation.z);
        //             voxelDataString.Append(")");
        //             voxelDataString.Append("-");
        //         }

        //         // Remove the last comma from the voxelDataString StringBuilder
        //         if (voxelDataString.Length > 0) {
        //             voxelDataString.Remove(voxelDataString.Length - 1, 1);
        //         }

        //         // Append the position and voxel data to the data StringBuilder in the desired format
        //         data.Append("(");
        //         data.Append(position.x);
        //         data.Append(",");
        //         data.Append(position.y);
        //         data.Append(",");
        //         data.Append(position.z);
        //         data.Append("):");
        //         data.Append(voxelDataString.ToString());
        //         data.Append("\n");
        //         // Don't add \n if it is the last line
        //     }

        //     // Write the data StringBuilder to the file at the specified path
        //     File.WriteAllText(path, data.ToString());
        //     AssetDatabase.Refresh();
        // }


        //  public static Dictionary<Vector3Int, List<VoxelData>> LoadSceneVoxelsFromFile(string path) {
        //         // Create a new Dictionary to store the voxelMatrix
        //         Dictionary<Vector3Int, List<VoxelData>> voxelMatrix = new Dictionary<Vector3Int, List<VoxelData>>();

        //         // Read the contents of the file at the specified path
        //         string fileContents = File.ReadAllText(path);

        //         // Split the file contents into lines
        //         string[] lines = fileContents.Split('\n');

        //         // Loop through each line
        //         foreach (string line in lines) {
        //             if (line == "")
        //                 continue;

        //             // Split the line into position and voxel data
        //             string[] parts = line.Split(':');
        //             string[] positionParts = parts[0].Replace("(", "").Replace(")", "").Split(',');
        //             Vector3Int position = new Vector3Int(int.Parse(positionParts[0]), int.Parse(positionParts[1]), int.Parse(positionParts[2]));
        //             string[] voxelDataStrings = parts[1].Split('-');

        //             // Create a new List to store the voxel data at the current position
        //             List<VoxelData> voxelDataList = new List<VoxelData>();

        //             // Loop through each voxel data string and add the corresponding VoxelData to the voxelDataList
        //             foreach (string voxelDataString in voxelDataStrings) {
        //                 if (voxelDataString == "")
        //                     continue;
        //                 string[] dataParts = voxelDataString.Split('(');
        //                 int id = int.Parse(dataParts[0]);
        //                 string[] rotationParts = dataParts[1].Replace("(", "").Replace(")", "").Split(',');
        //                 Vector3 rotation = new Vector3(int.Parse(rotationParts[0]), int.Parse(rotationParts[1]), int.Parse(rotationParts[2]));
        //                 VoxelItem voxelItem = GetVoxelItemByID(id);
        //                 VoxelData voxelData = new VoxelData(voxelItem, rotation);
        //                 voxelDataList.Add(voxelData);
        //             }

        //             // Add the voxelDataList to the voxelMatrix at the current position
        //             voxelMatrix.Add(position, voxelDataList);
        //         }

        //         return voxelMatrix;
        //     }

        [System.Serializable]
    private struct VoxelDataWrapper
    {
        public int id;
        public Vector3 rotation;
    }

    [System.Serializable]
    private struct VoxelPositionWrapper
    {
        public int x;
        public int y;
        public int z;
    }

    [System.Serializable]
    private struct VoxelGroupWrapper
    {
        public VoxelPositionWrapper position;
        public List<VoxelDataWrapper> voxels;
    }

    [System.Serializable]
    private struct VoxelGroupListWrapper
    {
        public List<VoxelGroupWrapper> voxelGroups;
    }

    public static void SaveSceneVoxelsToFile(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix, string path)
    {

        if (voxelMatrix == null) {
            Debug.LogError("voxelMatrix parameter is null");
            return;
        }

        if (string.IsNullOrEmpty(path)) {
            Debug.LogError("path parameter is null or empty");
            return;
        }

        VoxelGroupListWrapper voxelGroups = new VoxelGroupListWrapper();

        voxelGroups.voxelGroups = new List<VoxelGroupWrapper>();

        foreach (KeyValuePair<Vector3Int, List<VoxelData>> entry in voxelMatrix)
        {
            //Debug.Log("Entry");
            if (entry.Value.Count == 0)
                continue;

            VoxelPositionWrapper position = new VoxelPositionWrapper
            {
                x = entry.Key.x,
                y = entry.Key.y,
                z = entry.Key.z
            };

            List<VoxelDataWrapper> voxels = new List<VoxelDataWrapper>();
            foreach (VoxelData voxelData in entry.Value)
            {
                VoxelDataWrapper voxel = new VoxelDataWrapper
                {
                    id = voxelData.voxelItem.id,
                    rotation = voxelData.rotation
                };
                voxels.Add(voxel);
            }

            
            voxelGroups.voxelGroups.Add(new VoxelGroupWrapper
            {
                position = position,
                voxels = voxels
            });
        }

        string json = JsonUtility.ToJson(voxelGroups, true);
        File.WriteAllText(path, json);
        UnityEditor.AssetDatabase.Refresh();
    }

    public static Vector3Int GetVector3Int(Vector3 vec3) {
        return new Vector3Int((int)vec3.x,(int)vec3.y,(int)vec3.z);
    }

    public static Dictionary<Vector3Int, List<VoxelData>> LoadSceneVoxelsFromFile(string path)
    {
        string json = File.ReadAllText(path);
        VoxelGroupListWrapper voxelGroups = JsonUtility.FromJson<VoxelGroupListWrapper>(json);

        Dictionary<Vector3Int, List<VoxelData>> voxelMatrix = new Dictionary<Vector3Int, List<VoxelData>>();

        foreach (VoxelGroupWrapper voxelGroup in voxelGroups.voxelGroups)
        {
            Vector3Int position = new Vector3Int(voxelGroup.position.x, voxelGroup.position.y, voxelGroup.position.z);

            List<VoxelData> voxels = new List<VoxelData>();
            foreach (VoxelDataWrapper voxelDataWrapper in voxelGroup.voxels)
            {
                VoxelItem voxelItem = GetVoxelItemByID(voxelDataWrapper.id);
                Vector3 rotation = voxelDataWrapper.rotation;
                voxels.Add(new VoxelData(voxelItem, position, GetVector3Int(rotation)));
            }

            voxelMatrix.Add(position, voxels);
        }

        return voxelMatrix;
    }


        public static Dictionary<Vector3Int, List<VoxelData>> CenterVoxelBuild(Dictionary<Vector3Int, List<VoxelData>> currentBuild) {
            Vector3 averagePosition = Vector3.zero;
            foreach (Vector3Int position in currentBuild.Keys) {
                averagePosition += position;
            }
            averagePosition /= currentBuild.Count;
            Vector3Int roundedPosition = new Vector3Int(Mathf.RoundToInt(averagePosition.x), Mathf.RoundToInt(averagePosition.y), Mathf.RoundToInt(averagePosition.z));
            Vector3Int displacement = Vector3Int.zero - roundedPosition;

            Dictionary<Vector3Int, List<VoxelData>> centeredBuild = new Dictionary<Vector3Int, List<VoxelData>>();
            foreach (Vector3Int position in currentBuild.Keys) {
                List<VoxelData> voxels = currentBuild[position];
                Vector3Int centeredPosition = position + displacement;
                centeredBuild.Add(centeredPosition, voxels);
            }

            return centeredBuild;
        }
        public static void LoadVoxelBuildFromFile(string path, Vector3 location, Vector3 orientation) {
            Dictionary<Vector3Int, List<VoxelData>> voxelMatrix = CenterVoxelBuild(LoadSceneVoxelsFromFile(path));
            LoadVoxelBuildFromFile(voxelMatrix, location, orientation);
        }
         
        //Editor Defines categories so it must always be open
        public static void LoadVoxelBuildFromFile(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix, Vector3 location, Vector3 orientation)
        {
            GameObject outputGameObject = new GameObject("VoxelRoom");
            foreach (Vector3Int position in voxelMatrix.Keys) {
                List<VoxelData> voxelDataList = voxelMatrix[position];
                foreach (VoxelData voxelData in voxelDataList) {
                        
                    GameObject prefab = voxelData.voxelItem.prefab;
                    VoxelItem voxelItem = voxelData.voxelItem;
                    if (prefab != null) {
                        GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        GameObject roomPiece = new GameObject("RoomPiece");
                        roomPiece.transform.SetParent(outputGameObject.transform);
                        instantiatedPrefab.transform.SetParent(roomPiece.transform);

                        instantiatedPrefab.transform.localPosition = voxelItem.offset;
                        instantiatedPrefab.transform.localScale = voxelItem.scale;
                        instantiatedPrefab.transform.rotation = Quaternion.Euler(Vector3.zero);

                        roomPiece.transform.position = position;
                        roomPiece.transform.rotation = Quaternion.Euler(voxelData.rotation);
                    }
                }
            }
            outputGameObject.transform.position = location;
            outputGameObject.transform.rotation = Quaternion.Euler(orientation);
        }

            public static VoxelItem GetVoxelItemByID(int id) {
                foreach(VoxelCategory category in VoxelPalette.categories) {
                    foreach(VoxelItem voxelItem in category.items) {
                        if(voxelItem.id == id)
                            return voxelItem;
                    }
                }
                return null;
            }

            public static int GetNewID() {
                List<int> takenIds = new List<int>();
                foreach(VoxelCategory cv in VoxelPalette.categories) {
                    foreach(VoxelItem voxel in cv.items) {
                        takenIds.Add(voxel.id);
                    }
                }
                int id = 0;
                while(true) {
                    id = UnityEngine.Random.Range(0,10000);
                    if(!takenIds.Contains(id)){
                        return id;
                    }
                    //id++;
                }
            }







        // Helper method to get the closest rotation to the nearest multiple of 90 degrees
        private static Vector3 GetClosestRotation(Vector3 rotation) {
            Vector3 closestRotation = new Vector3(Mathf.Round(rotation.x / 90f) * 90f, Mathf.Round(rotation.y / 90f) * 90f, Mathf.Round(rotation.z / 90f) * 90f);
            return closestRotation;
        }


    // Loop through each voxel data string and add the corresponding Voxel

        // public static Dictionary<Vector3, List<VoxelItem>> LoadSceneVoxelsFromFile(string path) {
        //     // Create a new Dictionary to store the voxelMatrix
        //     Dictionary<Vector3, List<VoxelItem>> voxelMatrix = new Dictionary<Vector3, List<VoxelItem>>();
            
        //     // Read the contents of the file at the specified path
        //     string fileContents = File.ReadAllText(path);
            
        //     // Split the file contents into lines
        //     string[] lines = fileContents.Split('\n');
            
        //     // Loop through each line
        //     foreach (string line in lines) {
        //         if(line == "")
        //             continue;
        //         // Split the line into position and voxel item ids
        //         string[] parts = line.Split(':');
        //         string[] positionParts = parts[0].Replace("(", "").Replace(")", "").Split(',');
        //         Vector3 position = new Vector3(int.Parse(positionParts[0]), int.Parse(positionParts[1]), int.Parse(positionParts[2]));
        //         string[] voxelItemIds = parts[1].Split(',');
                
        //         // Create a new List to store the voxel items at the current position
        //         List<VoxelItem> voxelItems = new List<VoxelItem>();
                
        //         // Loop through each voxel item id and add the corresponding VoxelItem to the voxelItems list
        //         foreach (string voxelItemId in voxelItemIds) {
        //             VoxelItem voxelItem = VoxelPaletteWindow.GetVoxelItemByID(int.Parse(voxelItemId));
        //             if (voxelItem != null) {
        //                 voxelItems.Add(voxelItem);
        //             }
        //         }
                
        //         // Add the voxelItems list to the voxelMatrix dictionary at the current position
        //         voxelMatrix[position] = voxelItems;
        //     }
            
        //     return voxelMatrix;
        // }



        public static Dictionary<Vector3Int, List<VoxelData>> ConvertSceneVoxels(Dictionary<Vector3Int, List<SceneVoxel>> sceneVoxelMatrix) {
            Dictionary<Vector3Int, List<VoxelData>> outputMatrix = new Dictionary<Vector3Int, List<VoxelData>>();

            foreach(Vector3Int key in sceneVoxelMatrix.Keys) {
                outputMatrix.Add(key, sceneVoxelMatrix[key].ConvertAll<VoxelData>((x) => new VoxelData(x.voxelItem, GetVector3Int(x.transform.position), GetVector3Int(x.transform.rotation.eulerAngles))));
            }
            return outputMatrix;
        }
    }

    