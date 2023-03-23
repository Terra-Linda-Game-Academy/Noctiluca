using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectile;

    public float shootSpeed = 10f;

    public int count = 5;

    public float randomness = 0.1f;

    public Transform spawnTransform;

    private bool shooting = false;
    public float shootTime = 0.5f;
    private float shootTimer = 0f;

    private IceSheetInstance currentIceSheet = null;


    public float maxIceSheetLife = 10f;





    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if(UnityEngine.Input.GetMouseButton(0))
        {
            if(!shooting)
            {
                shooting = true;
                currentIceSheet = IceFloeManager.Instance.CreateIceSheet(maxIceSheetLife);
            }
            if (shootTimer >= shootTime)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject instantiatedProjectile = Instantiate(projectile, spawnTransform.position, spawnTransform.rotation);
                    instantiatedProjectile.GetComponent<IceProjectile>().targetIceSheet = currentIceSheet.iceSheetController;
                    Rigidbody rigidbody = instantiatedProjectile.GetComponent<Rigidbody>();
                    rigidbody.AddForce((spawnTransform.forward + new Vector3(UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness))) * shootSpeed, ForceMode.Impulse);
                }
                shootTimer = 0f;
            }
            shootTimer += Time.deltaTime;
            
        } else
        {
            shooting = false;
        }

    }
}
