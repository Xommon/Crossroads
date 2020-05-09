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
    public string turnSignal;
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

    // Lists
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
        // Move the vehicle
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
                speed -= (acceleration * 1.5f) * emergencyStop;
            }
            else
            {
                speed = 0;
            }
        }

        // Keep the vehicle moving if the wheels are on the ground
        if (transform.rotation.z > 50 || transform.rotation.z < -50)
        {
            canGo = false;
            //Debug.Log("false");
        }
        else if (canGo == false && permissionToGo == false && pastIntersection == false && atIntersection == false && collided == false)
        {
            canGo = true;
            //Debug.Log("true");
        }

        // Detect object in the way
        RaycastHit hit;
        range = 3f;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.forward * range, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.forward, out hit, range) && hit.transform.tag != "Terrain")
        {
            Intersection intersection = hit.transform.GetComponent<Intersection>();
            Vehicle vehicle = hit.transform.GetComponent<Vehicle>();
            objectInWay = hit.transform.gameObject;
        }
        
        // Emergency stop for objects in way
        if (objectInWay != null)
        {
            if (objectInWay.transform.tag == "Intersection")
            {
                // Distance from an intersection
                distanceFromObjectInWay = Vector3.Distance(transform.position, objectInWay.transform.position) - 1.5f;
            }
            else
            {
                // Distance from other vehicle/pedestrian/etc
                distanceFromObjectInWay = Vector3.Distance(transform.position, objectInWay.transform.position) - 0.2f;
            }

            if (atIntersection == false && pastIntersection == false)
            {
                if (distanceFromObjectInWay >= 3.5f)
                {
                    canGo = true;
                }
                else if (distanceFromObjectInWay < 3.5f && distanceFromObjectInWay >= 2.25f)
                {
                    if (speed >= 12)
                    {
                        emergencyStop = 1.25f;
                        canGo = false;
                    }
                    else if (speed < 12 && speed >= 9)
                    {
                        emergencyStop = 1f;
                        canGo = false;
                    }
                    else if (speed < 9 && speed >= 6)
                    {
                        canGo = true;
                    }
                }
                else if (distanceFromObjectInWay < 2.25f && distanceFromObjectInWay >= 1.25f)
                {
                    if (speed >= 12)
                    {
                        emergencyStop = 1.5f;
                        canGo = false;
                    }
                    else if (speed < 12 && speed >= 9)
                    {
                        emergencyStop = 1.25f;
                        canGo = false;
                    }
                    else if (speed < 9 && speed >= 6)
                    {
                        emergencyStop = 1f;
                        canGo = false;
                    }
                    else if (speed < 6)
                    {
                        canGo = true;
                    }
                }
                else if (distanceFromObjectInWay < 1.25f && distanceFromObjectInWay >= 0)
                {
                    if (speed >= 12)
                    {
                        emergencyStop = 2f;
                        canGo = false;
                    }
                    else if (speed < 12 && speed >= 9)
                    {
                        emergencyStop = 1.5f;
                        canGo = false;
                    }
                    else if (speed < 9 && speed >= 6)
                    {
                        emergencyStop = 1.25f;
                        canGo = false;
                    }
                    else if (speed < 6)
                    {
                        emergencyStop = 1f;
                        canGo = false;
                    }
                    else
                    {
                        canGo = true;
                    }
                }
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Intersection" && permissionToGo == false)
        {
            speed = 0;
            atIntersection = true;

            // Choose which direction the car intends to turn
            if (turnSignal == "")
            {
                if (gameManager.PercentChance(50))
                {
                    // 50%
                    turnSignal = "forward";
                }
                else if (gameManager.PercentChance(50))
                {
                    // 25%
                    turnSignal = "right";
                    //turningRight = true;
                }
                else
                {
                    // 25%
                    turnSignal = "left";
                    //turningLeft = true;
                }
            }
        }
        else if (collision.transform.tag == "Intersection" && permissionToGo == true)
        {
            if (turnSignal == "right")
            {
                //turningRight = true;
                //turningLeft = false;
            }
            else if (turnSignal == "left")
            {
                //turningLeft = true;
                //turningRight = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Intersection")
        {
            atIntersection = false;
            pastIntersection = true;
            turnSignal = "";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.tag == "Vehicle")
        {
            collided = true;
            collidedWithObject = collision.gameObject;
        }
    }
}
