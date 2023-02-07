using UnityEngine;

public class SnakeLocomoter : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 90f;
    public Transform targetDestination;
    public float waveFrequency = 1f;
    public float waveAmplitude = 1f;
    public float accuracy = 0.1f;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {


        Vector3 direction = (targetDestination.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20f))
        {
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.up), Time.deltaTime * rotationSpeed * 3);
        }

        
        

        float distance = Vector3.Distance(transform.position, targetDestination.position);
        if (distance > accuracy)
        {
            float sinValue = Mathf.Sin((Time.time - startTime) * waveFrequency) * waveAmplitude;
            transform.position = transform.position + transform.forward * speed * Time.deltaTime + transform.right * sinValue;
        }
        else
        {
            transform.position = targetDestination.position;
        }
    }
}
