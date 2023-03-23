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
        for (int v = 0; v < iceSheetInstance.iceSheetController.meshFilter.mesh.vertices.Length; v += 3)
        {
            Vector3 particlePos = iceSheetInstance.iceSheetController.meshFilter.mesh.vertices[v] + iceSheetInstance.iceSheetController.transform.position;

            Destroy(Instantiate(iceSheetDestroyParticles, particlePos, Quaternion.identity), 3f);
        }

        Destroy(iceSheetInstance.iceSheetController.gameObject);
    }
}

    

public class IceSheetInstance
{
    public IceSheetController iceSheetController;

    public float lifeTime = 0;

    public float maxLifeTime = 10f;
    public IceSheetInstance(IceSheetController iceSheetController, float maxLifeTime)
    {
        this.iceSheetController = iceSheetController;
        this.maxLifeTime = maxLifeTime;
    }
} 
