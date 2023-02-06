using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed = 280;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();

    float countUp = 0f;

    //public float speed = 5f;
    public float rotationSpeed = 90f;
    public Transform targetDestination;
    public float waveFrequency = 1f;
    public float waveAmplitude = 1f;
    public float accuracy = 0.1f;

    private float startTime;

    public GameObject moverObject;



    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        CreateBodyParts();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        SnakeMovement();
    }

    void SnakeMovement()
    {
        //replace getcomponnet not efficient
        snakeBody[0].GetComponent<Rigidbody>().velocity = moverObject.GetComponent<Rigidbody>().velocity;

        Vector3 direction = (targetDestination.position - moverObject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        moverObject.transform.rotation = Quaternion.Slerp(moverObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        //if (UnityEngine.Input.GetAxis("Horizontal") != 0)
        //    moverObject.transform.Rotate(new Vector3(0, -turnSpeed * Time.deltaTime * UnityEngine.Input.GetAxis("Horizontal"),0));

        //Vector3 direction = (targetDestination.position - moverObject.transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(moverObject.GetComponent<Rigidbody>().velocity.normalized);
        //moverObject.transform.rotation = lookRotation;

        float distance = Vector3.Distance(moverObject.transform.position, targetDestination.position);
        if (distance > accuracy)
        {
            float sinValue = Mathf.Sin((Time.time - startTime) * waveFrequency) * waveAmplitude;
            moverObject.GetComponent<Rigidbody>().velocity = moverObject.transform.forward * speed * Time.deltaTime + moverObject.transform.right * sinValue;
        }

        if (snakeBody.Count > 1)
        {
            snakeBody[0].transform.LookAt(moverObject.transform);
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if(snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>())
            {
                temp1.AddComponent<MarkerManager>();
            }
            if (!temp1.GetComponent<Rigidbody>())
            {
                temp1.AddComponent<Rigidbody>();
                temp1.GetComponent<Rigidbody>().useGravity = false;
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);

            moverObject.transform.position = snakeBody[0].transform.position += snakeBody[0].transform.forward * 2f;
        }

        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();
        if(countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if(countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position , markM.markerList[0].rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            if (!temp.GetComponent<Rigidbody>())
            {
                temp.AddComponent<Rigidbody>();
                temp.GetComponent<Rigidbody>().useGravity = false;
            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }
}
