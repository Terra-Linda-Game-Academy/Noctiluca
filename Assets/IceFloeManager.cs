using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFloeManager : MonoBehaviour
{


    public GameObject iceSheetPrefab;

    public List<IceSheetInstance> iceSheetInstances = new List<IceSheetInstance>();


    public GameObject iceSheetDestroyParticles;

    public static IceFloeManager Instance;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public IceSheetInstance CreateIceSheet(float maxLifeTime)
    {
        IceSheetInstance iceSheetInstance = new IceSheetInstance(Instantiate(iceSheetPrefab).GetComponent<IceSheetController>(), maxLifeTime);
        iceSheetInstances.Add(iceSheetInstance);
        return iceSheetInstance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        foreach(IceSheetInstance iceSheet in iceSheetInstances)
        {
            if(!iceSheet.growing)
                iceSheet.lifeTime += Time.fixedDeltaTime;

            if(iceSheet.lifeTime > iceSheet.maxLifeTime)
            {
                DestroyIceSheet(iceSheet);
            }
        }

        iceSheetInstances.RemoveAll((iceSheet) => (iceSheet.lifeTime > iceSheet.maxLifeTime));


    }

    void DestroyIceSheet(IceSheetInstance iceSheetInstance)
    {
        for (int v = 0; v < iceSheetInstance.iceSheetController.meshFilter.mesh.vertices.Length; v += iceSheetInstance.iceSheetController.meshFilter.mesh.vertices.Length/25)
        {
            Vector3 localPosition = iceSheetInstance.iceSheetController.meshFilter.mesh.vertices[v];
            Vector3 localRotatedPosition = RotatePointAroundPivot(localPosition, iceSheetInstance.iceSheetController.transform.position, iceSheetInstance.iceSheetController.transform.rotation.eulerAngles);
            Vector3 particlePos = localRotatedPosition + iceSheetInstance.iceSheetController.transform.position;

            Destroy(Instantiate(iceSheetDestroyParticles, particlePos, Quaternion.identity), 3f);
        }

        Destroy(iceSheetInstance.iceSheetController.gameObject);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles)* dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
     }
}

    

public class IceSheetInstance
{
    public IceSheetController iceSheetController;

    public float lifeTime = 0;

    public float maxLifeTime = 10f;

    public bool growing = true;
    public IceSheetInstance(IceSheetController iceSheetController, float maxLifeTime)
    {
        this.iceSheetController = iceSheetController;
        this.maxLifeTime = maxLifeTime;
    }
} 
