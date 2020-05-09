using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    // References
    public GameManager gameManager;

    // Vehicle information
    public string vehicleType;
    public float startSpeed;
    public float maxSpeed;
    public float speed;
    public float turning;
    public float acceleration;
    
    // Collision detection
    public GameObject objectInWay;
    public GameObject collidedWithObject;
    public bool collided;
    public bool canGo;
    public bool permissionToGo;
    public float range;
    public float emergencyStop;
    public float distanceFromObjectInWay;

    // Interserction interaction
    public Intersection intersection;
    public bool pastIntersection;
    public float distanceFromIntersection;
    public bool atIntersection;


    public List<GameObject> wheels = new List<GameObject>();

    void Start()
    {
        // Vehicle variables
        pastIntersection = false;
        permissionToGo = false;
        atIntersection = false;
        emergencyStop = 1;
        objectInWay = null;
        collided = false;

        // Create list of wheels
        foreach (Transform child in transform)
        {
            wheels.Add(child.gameObject);
        }
    }
    
    void Update()
    {
        // Move the car
        //transform.position += new Vector3(0, 0, speed / 150);
        transform.position += transform.forward * (speed / 150);
        if (canGo)
        {
            if (speed < maxSpeed)
            {
                // Accelerate
                speed += acceleration;
            }
            else
            {
                speed = maxSpeed;
            }
        }
        else
        {
            if (speed > 0)
            {
                // Decelerate
                speed -= (acceleration * 1.5f); //* emergencyStop
            }
            else
            {
                speed = 0;
            }
        }

        // Keep the car moving
        if (canGo == false && permissionToGo == false && pastIntersection == false && atIntersection == false && collided == false)
        {
            canGo = true;
        }

        // Stop the car before it collides with an obstacle
        RaycastHit hit;
        range = 1000f;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, range) && hit.transform.tag != "Terrain")
        {
            Debug.Log(hit.transform.name);
            Intersection intersection = hit.transform.GetComponent<Intersection>();
            Car car = hit.transform.GetComponent<Car>();
            objectInWay = hit.transform.gameObject;
        }

        // The vehicle cannot move if at the intersection and not given permission to go OR has been involved in a car accident
        if ((atIntersection && !permissionToGo)/* || (collided == true && (atIntersection || pastIntersection))*/)
        {
            canGo = false;
        }

        // Turn and spin the wheels
        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].transform.Rotate(speed / 2, 0 , 0, Space.Self);
        }

        /*for (int i = 0; i < 2; i++)
        {
            wheels[i].transform.rotation = new Quaternion(transform.rotation.x, turning, transform.rotation.y, transform.rotation.w);
        }*/

        // Cap values
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        else if (speed < -3.0f)
        {
            speed = -3.0f;
        }

        if (turning > 50)
        {
            turning = 50;
        }
        else if (turning < -50)
        {
            turning = -50;
        }
    }
}
