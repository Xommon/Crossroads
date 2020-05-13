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
    public bool spawning;
    public float startingX;
    public float startingZ;
    public bool onGround;
    public float direction;
    
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
        speed = 5;
        objectInWay = null;
        collided = false;
        spawning = true;
        startingX = transform.position.x;
        intersection = FindObjectOfType<Intersection>();

        // Create list of wheels
        foreach (Transform child in transform)
        {
            wheels.Add(child.gameObject);
        }
    }

    void Update()
    {
        // Move the vehicle to the board
        if (spawning)
        {
            transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
            transform.position = new Vector3(startingX, transform.position.y, transform.position.z);
            if (transform.rotation.x < 0)
            {
                transform.Rotate(13, 0, 0, Space.Self);
            }
            else
            {
                transform.position = new Vector3(0.37f, 0, -6.87f);
                transform.rotation = Quaternion.identity;
                spawning = false;
            }
        }
        else
        {
            transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

        // Move the vehicle
        //transform.position += new Vector3(0, 0, speed / 150);
        if (onGround)
        {
            transform.position += transform.forward * (speed / 150);
        }

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
        if (!onGround)
        {
            canGo = false;
        }
        else if (canGo == false && permissionToGo == false && pastIntersection == false && atIntersection == false && collided == false)
        {
            canGo = true;
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

        // Calculate direction of vehicle
        //direction = gameObject.transform.rotation.y;
        /*if (direction >= 360)
        {
            direction -= 360;
        }
        else if (direction < 0)
        {
            direction += 360;
        }*/

        if (gameObject.transform.rotation.y > 360)
        {
            gameObject.transform.Rotate(-360, 0, 0, Space.Self);
        }
        if (gameObject.transform.rotation.y < 22.5f || gameObject.transform.rotation.y >= 337.5f)
        {
            // Down
            direction = 0;
        }
        else if (gameObject.transform.rotation.y < 67.5f && gameObject.transform.rotation.y >= 22.5f)
        {
            // DownRight
            direction = 315;
        }
        else if (gameObject.transform.rotation.y < 112.5f && gameObject.transform.rotation.y >= 67.5f)
        {
            // Right
            direction = 270;
        }
        else if (gameObject.transform.rotation.y < 157.5f && gameObject.transform.rotation.y >= 112.5f)
        {
            // UpRight
            direction = 225;
        }
        else if (gameObject.transform.rotation.y < 202.5f && gameObject.transform.rotation.y >= 157.5f)
        {
            // Up
            direction = 180;
        }
        else if (gameObject.transform.rotation.y < 247.5f && gameObject.transform.rotation.y >= 202.5f)
        {
            // UpLeft
            direction = 135;
        }
        else if (gameObject.transform.rotation.y < 292.5f && gameObject.transform.rotation.y >= 247.5f)
        {
            // Left
            direction = 90;
        }
        else if (gameObject.transform.rotation.y < 337.5f && gameObject.transform.rotation.y >= 292.5f)
        {
            // DownLeft
            direction = 45;
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

    private void OnTriggerEnter(Collider collision)
    {
         // Collision with intersection
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

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Intersection")
        {
            atIntersection = false;
            pastIntersection = true;
            turnSignal = "";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if vehicle is on the ground
        if (collision.transform.tag == "Terrain")//  && (transform.rotation.z > 50.0f || transform.rotation.z < -50.0f))
        {
            onGround = true;
        }

        // Vehicle crashes
        if (collision.gameObject.transform.tag == "Vehicle")
        {
            collided = true;
            collidedWithObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if vehicle is on the ground
        if (collision.transform.tag == "Terrain")// && (transform.rotation.z > 50.0f || transform.rotation.z < -50.0f))
        {
            onGround = false;
        }
    }
}
