using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class DungeonRoom {
    public Dictionary<Vector3Int, List<VoxelData>> voxelMatrix;
    //0 - 3
    public int rotation = 0;

    public Vector3Int position = Vector3Int.zero;


    public DungeonRoom(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix) {
        this.voxelMatrix = voxelMatrix;
    }
}

public class DungeonGenerator : MonoBehaviour
{
    public TextAsset[] rooms;

    private List<GameObject> instantiatedRooms = new List<GameObject>();

    private List<DungeonRoom> dungeonRooms = new List<DungeonRoom>();


    int exitId = 8492;
    int entranceId = 0;


    public int numberOfRooms = 15;


    private void LoadDungeonRoom(DungeonRoom dungeonRoom) {
        VoxelTools.LoadVoxelBuildFromFile(dungeonRoom.voxelMatrix, dungeonRoom.position, Vector3.zero);
    }

    private void Start()
    {
        // Generate the first room
        VoxelPalette.InitCategories();

        foreach(TextAsset textAsset in rooms) {
            dungeonRooms.Add(new DungeonRoom(VoxelTools.LoadSceneVoxelsFromFile(AssetDatabase.GetAssetPath(textAsset))));
        }
        
        

        DungeonRoom lastRoom = dungeonRooms[UnityEngine.Random.Range(0, rooms.Length)];

       // int rN = ((90 / 90) - (0 / 90)  + 4) % 4;
        //Debug.Log("rotaion: " + rN);

        /*
        List<List<VoxelData>> door = GetChunks(lastRoom.voxelMatrix, entranceId);
        List<Vector3Int> doorPos = door[UnityEngine.Random.Range(0, door.Count)].ConvertAll<Vector3Int>((x) => x.position);

        //lastRoom

        LoadDungeonRoom(lastRoom);

        lastRoom.voxelMatrix = RotateMatrixY(lastRoom.voxelMatrix, doorPos[0]);
        LoadDungeonRoom(lastRoom);

        lastRoom.voxelMatrix = RotateMatrixY(lastRoom.voxelMatrix, doorPos[0]);
        LoadDungeonRoom(lastRoom);

        lastRoom.voxelMatrix = RotateMatrixY(lastRoom.voxelMatrix, doorPos[0]);
        LoadDungeonRoom(lastRoom);
        */




        for (int room = 0; room < numberOfRooms; room++) {

            DungeonRoom newRoom = null;
            int attempts = 0;
            while (newRoom == null || newRoom == lastRoom) {
                newRoom = dungeonRooms[UnityEngine.Random.Range(0, rooms.Length)];
                attempts++;

                if (attempts > 10) {
                    Debug.LogError("Failed to find a suitable new room after 10 attempts");
                    return;
                }
            }

            List<List<VoxelData>> room1exits = GetChunks(lastRoom.voxelMatrix, exitId);
            List<List<VoxelData>> room2entrances = GetChunks(newRoom.voxelMatrix, entranceId);
            List<Vector3Int> room1exit = room1exits[UnityEngine.Random.Range(0, room1exits.Count)].ConvertAll<Vector3Int>((x) => x.position);
            List<Vector3Int> room2entrance = room2entrances[0].ConvertAll<Vector3Int>((x) => x.position);
            if (MatchShapes(room1exit, room2entrance)) {
                newRoom.position = lastRoom.position - GetShapeDisplacement(room1exit, room2entrance);

                VoxelData exitVoxel = lastRoom.voxelMatrix[room1exit[0]].Find((x) => x.voxelItem.id == exitId);
                VoxelData entranceVoxel = newRoom.voxelMatrix[room2entrance[room2entrance.Count-1]].Find((x) => x.voxelItem.id == entranceId);

                int rotationsNeeded = ((exitVoxel.rotation.y / 90) - (entranceVoxel.rotation.y / 90) + 4) % 4;
                for(int i = 0; i < rotationsNeeded; i++)
                    newRoom.voxelMatrix = RotateMatrixY(newRoom.voxelMatrix, room2entrance[0]);

                    LoadDungeonRoom(newRoom);
                    lastRoom = newRoom;

               // if (!CheckBoundingBoxCollision(newRoom.voxelMatrix, lastRoom.voxelMatrix))
               // {
                //    LoadDungeonRoom(newRoom);
                //    lastRoom = newRoom;
                //}
                    
            }
        }
        





        // if(!CheckBoundingBoxCollision(dungeonRooms[0].voxelMatrix, dungeonRooms[1].voxelMatrix))
        //  {
        //     LoadDungeonRoom(newRoom);
        //     break;
        //  }

        // foreach(List<VoxelData> chunk in ) {
        //     foreach(VoxelData voxelData in chunk) {
        //         GameObject b= GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //         b.transform.position = voxelData.position;
        //         b.transform.localScale = new Vector3(2,2,2);
        //     }
        // }
    }




    public static Dictionary<Vector3Int, List<VoxelData>> RotateMatrixY(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix, Vector3Int rotationPoint)
    {
        Dictionary<Vector3Int, List<VoxelData>> rotatedMatrix = new Dictionary<Vector3Int, List<VoxelData>>();
        foreach (Vector3Int point in voxelMatrix.Keys)
        {
            // Subtract the rotation point from the point to make it relative to the origin
            Vector3Int relativePoint = point - rotationPoint;
            
            // Rotate the point 90 degrees on the y axis by swapping the x and z coordinates and negating the new z coordinate
            Vector3Int rotatedPoint = new Vector3Int(relativePoint.z, relativePoint.y, -relativePoint.x);
            
            // Add the rotation point back to the rotated point to move it back to its original position
            Vector3Int finalPoint = rotatedPoint + rotationPoint;
            
            // Copy the voxel data to the new position
            rotatedMatrix[finalPoint] = new List<VoxelData>(voxelMatrix[point]);
            
            // Update the position of the voxel data
            foreach (VoxelData voxelData in rotatedMatrix[finalPoint])
            {
                voxelData.position = finalPoint;
                voxelData.rotation.y += 90;
            }
        }
        
        return rotatedMatrix;
    }

    public bool CheckBoundingBoxCollision(Dictionary<Vector3Int, List<VoxelData>> shape1, Dictionary<Vector3Int, List<VoxelData>> shape2)
    {
        // Calculate the bounding boxes of the shapes
        Vector3Int shape1Min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int shape1Max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        Vector3Int shape2Min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int shape2Max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        foreach (Vector3Int point in shape1.Keys)
        {
            if(shape1[point].FindAll((x) => (x.voxelItem.id==entranceId || x.voxelItem.id==exitId)).Count > 0)
                continue;
            shape1Min = Vector3Int.Min(shape1Min, point);
            shape1Max = Vector3Int.Max(shape1Max, point);
        }

        foreach (Vector3Int point in shape2.Keys)
        {
            if(shape2[point].FindAll((x) => (x.voxelItem.id==entranceId || x.voxelItem.id==exitId)).Count > 0)
                continue;
            shape2Min = Vector3Int.Min(shape2Min, point);
            shape2Max = Vector3Int.Max(shape2Max, point);
        }

        // Check for collision
        if (shape1Max.x < shape2Min.x || shape1Min.x > shape2Max.x) return false;
        if (shape1Max.y < shape2Min.y || shape1Min.y > shape2Max.y) return false;
        if (shape1Max.z < shape2Min.z || shape1Min.z > shape2Max.z) return false;

        return true;
    }


    public Vector3Int GetShapeDisplacement(List<Vector3Int> shape1, List<Vector3Int> shape2)
    {
        Vector3Int[] sortedShape1 = shape1.OrderBy(v => v.x).ThenBy(v => v.y).ThenBy(v => v.z).ToArray();
        Vector3Int[] sortedShape2 = shape2.OrderBy(v => v.x).ThenBy(v => v.y).ThenBy(v => v.z).ToArray();

        return sortedShape2[0] - sortedShape1[0];
    }

    public static bool MatchShapes(List<Vector3Int> shape1, List<Vector3Int> shape2)
    {
        // Get the size of each shape
        Vector3Int size1 = GetShapeSize(shape1);
        Vector3Int size2 = GetShapeSize(shape2);

        // Check if the sizes match
        if (size1 != size2)
        {
            return false;
        }

        // Remove displacement from each shape
        List<Vector3Int> normalizedShape1 = NormalizeShape(shape1);
        List<Vector3Int> normalizedShape2 = NormalizeShape(shape2);

        // Check if each point in shape1 exists in shape2
        foreach (Vector3Int point1 in normalizedShape1)
        {
            bool foundMatch = false;
            foreach (Vector3Int point2 in normalizedShape2)
            {
                if (point1.x == point2.x && point1.y == point2.y && point1.z == point2.z)
                {
                    foundMatch = true;
                    break;
                }
            }
            if (!foundMatch)
            {
                return false;
            }
        }

        // Check if each point in shape2 exists in shape1
        foreach (Vector3Int point2 in normalizedShape2)
        {
            bool foundMatch = false;
            foreach (Vector3Int point1 in normalizedShape1)
            {
                if (point2.x == point1.x && point2.y == point1.y && point2.z == point1.z)
                {
                    foundMatch = true;
                    break;
                }
            }
            if (!foundMatch)
            {
                return false;
            }
        }

        // The shapes match
        return true;
    }

    private static Vector3Int GetShapeSize(List<Vector3Int> shape)
    {
        Vector3Int min = shape[0];
        Vector3Int max = shape[0];

        // Find the minimum and maximum points of the shape
        foreach (Vector3Int point in shape)
        {
            if (point.x < min.x) min.x = point.x;
            if (point.y < min.y) min.y = point.y;
            if (point.z < min.z) min.z = point.z;

            if (point.x > max.x) max.x = point.x;
            if (point.y > max.y) max.y = point.y;
            if (point.z > max.z) max.z = point.z;
        }

        // Calculate the size of the shape
        return new Vector3Int(max.x - min.x + 1, max.y - min.y + 1, max.z - min.z + 1);
    }

    private static List<Vector3Int> NormalizeShape(List<Vector3Int> shape)
    {
        List<Vector3Int> normalizedShape = new List<Vector3Int>();

        // Find the displacement of the shape
        Vector3Int displacement = shape[0];
        foreach (Vector3Int point in shape)
        {
            if (point.x < displacement.x) displacement.x = point.x;
            if (point.y < displacement.y) displacement.y = point.y;
            if (point.z < displacement.z) displacement.z = point.z;
        }

        // Normalize the shape by subtracting the displacement from each point
        foreach (Vector3Int point in shape)
        {
            normalizedShape.Add(point - displacement);
        }

        return normalizedShape;
    }

    public List<List<VoxelData>> GetChunks(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix, int id)
    {
        List<List<VoxelData>> chunks = new List<List<VoxelData>>();
        List<Vector3Int> foundBlocks = new List<Vector3Int>();
        
        foreach (KeyValuePair<Vector3Int, List<VoxelData>> voxelGroup in voxelMatrix)
        {
            foreach (VoxelData voxelData in voxelGroup.Value)
            {
                if (voxelData.voxelItem.id == id && !foundBlocks.Contains(voxelGroup.Key))
                {
                    List<VoxelData> chunk = GetConnectedBlocks(voxelMatrix, voxelGroup.Key, id);
                    chunks.Add(chunk);
                    foreach (VoxelData connectedBlock in chunk)
                    {
                        foundBlocks.Add(connectedBlock.position);
                    }
                    break;
                }
            }
        }

        return chunks;
    }


    public List<VoxelData> GetConnectedBlocks(Dictionary<Vector3Int, List<VoxelData>> voxelMatrix, Vector3Int startingPoint, int targetId)
    {
        List<VoxelData> connectedBlocks = new List<VoxelData>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        // Add the starting point to the queue
        queue.Enqueue(startingPoint);

        // Process the queue until it's empty
        while (queue.Count > 0)
        {
            // Get the next point from the queue
            Vector3Int currentPoint = queue.Dequeue();

            // Skip if we've already visited this point
            if (visited.Contains(currentPoint)) continue;

            // Mark the current point as visited
            visited.Add(currentPoint);

            // Get the voxel data at the current point
            if (voxelMatrix.TryGetValue(currentPoint, out List<VoxelData> voxelGroup))
            {
                foreach (VoxelData voxelData in voxelGroup)
                {
                    // Check if the voxel has the target id
                    if (voxelData.voxelItem.id == targetId)
                    {
                        connectedBlocks.Add(voxelData);

                        // Add the adjacent blocks to the queue
                        foreach (Vector3Int direction in adjacentDirections)
                        {
                            Vector3Int adjacentPoint = currentPoint + direction;
                            if (!visited.Contains(adjacentPoint))
                            {
                                queue.Enqueue(adjacentPoint);
                            }
                        }
                    }
                }
            }
        }

        //Debug.Log("Returning Length of" + connectedBlocks.Count);

        return connectedBlocks;
    }

    private Vector3Int[] adjacentDirections = {
        new Vector3Int(0, 1, 0), // up
        new Vector3Int(0, -1, 0), // down
        new Vector3Int(1, 0, 0), // right
        new Vector3Int(-1, 0, 0), // left
        new Vector3Int(0, 0, 1), // forward
        new Vector3Int(0, 0, -1), // back
    };


    


}