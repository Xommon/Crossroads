using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameManager gameManager;
    public float speed;
    public float maxSpeed;
    public float acceleration;
    public float direction;
    public float rotation;
    public float emergencyStop;
    public bool canGo;
    public bool permissionToGo;
    public bool pastIntersection;
    public float distanceFromIntersection;
    public bool atIntersection;
    public Intersection intersection;
    public GameObject objectInWay;
    public float distanceFromObjectInWay;
    public string turning;
    public bool turningRight;
    public bool turningLeft;
    public float xDirection;
    public float yDirection;
    public float delay;
    public bool collided;
    public GameObject collidedWithObject;
    public LayerMask Raycastable;

    // Sprites
    public SpriteRenderer sr;
    public Sprite downRight;
    public Sprite upRight;
    public Sprite downLeft;
    public Sprite upLeft;
    public Sprite right;
    public Sprite left;
    public Sprite up;
    public Sprite down;

    // Start is called before the first frame update
    void Start()
    {
        speed = maxSpeed;
        canGo = true;
        pastIntersection = false;
        permissionToGo = false;
        atIntersection = false;
        emergencyStop = 1;
        objectInWay = null;
        turningRight = false;
        turningLeft = false;
        collided = false;
        intersection = FindObjectOfType<Intersection>();
        delay = 0;

        if (transform.position.x < 0 && transform.position.y >= 0)
        {
            // Down Right
            xDirection = 2;
            yDirection = -1;
            direction = 315;
        }
        else if (transform.position.x < 0 && transform.position.y < 0)
        {
            // Up Right
            xDirection = 2;
            yDirection = 1;
            direction = 45;
        }
        else if (transform.position.x >= 0 && transform.position.y < 0)
        {
            // Up Left
            xDirection = -2;
            yDirection = 1;
            direction = 135;
        }
        else if (transform.position.x >= 0 && transform.position.y >= 0)
        {
            // Down Left
            xDirection = -2;
            yDirection = -1;
            direction = 225;
        }

        rotation = direction;
    }

    // Update is called once per frame
    void Update()
    {
        // The vehicle cannot move if at the intersection and not given permission to go OR has been involved in a car accident
        if ((atIntersection && !permissionToGo)/* || (collided == true && (atIntersection || pastIntersection))*/)
        {
            canGo = false;
        }

        // Keep the car moving
        if (canGo == false && permissionToGo == false && pastIntersection == false && atIntersection == false && collided == false)
        {
            canGo = true;
        }

        // Stop the car before it hits obstacles
        Debug.DrawRay(transform.position + new Vector3(0.25f * xDirection, 0.25f * yDirection, 0), new Vector3(xDirection, yDirection, 0), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.25f * xDirection, 0.25f * yDirection, 0), new Vector3(xDirection, yDirection, 0), 3.0f, Raycastable);
        if (hit && hit.transform.gameObject.tag != "ClickArea") // Don't edit
        {
            Debug.Log(direction + hit.transform.gameObject.name);
            Intersection intersection = hit.transform.GetComponent<Intersection>();
            Car car = hit.transform.GetComponent<Car>();
            objectInWay = hit.transform.gameObject;
        }

        // Emergency stop for objects in way
        if (objectInWay != null)
        {
            if (objectInWay.transform.tag == "Intersection")
            {
                // Distance from an intersection
                distanceFromObjectInWay = Vector3.Distance(transform.position, objectInWay.transform.position) - 1f;
            }
            else
            {
                // Distance from other vehicle/pedestrian/etc
                distanceFromObjectInWay = Vector3.Distance(transform.position, objectInWay.transform.position) - 0.2f;
            }

            if (atIntersection == false && pastIntersection == false)
            {
                if (distanceFromObjectInWay >= 3f)
                {
                    canGo = true;
                }
                else if (distanceFromObjectInWay < 3f && distanceFromObjectInWay >= 2.25f)
                {
                    if (speed >= 12)
                    {
                        emergencyStop = 1.25f; //  * (Time.deltaTime * 60)
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
                else if (distanceFromObjectInWay < 2.25f && distanceFromObjectInWay >= 1.15f)
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
                else if (distanceFromObjectInWay < 1.15f && distanceFromObjectInWay >= 0)
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

        // Move and Stop the car
        if (canGo == true)
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
                speed -= (acceleration * 1.5f * emergencyStop);
            }
            else
            {
                speed = 0;
            }
        }

        // Point the car in the correct direction and use the right sprites
        if (direction >= 360)
        {
            direction -= 360;
        }
        else if (direction < 0)
        {
            direction += 360;
        }

        if (direction < 22.5f || direction >= 337.5f)
        {
            // Right
            sr.sprite = right;
        }
        else if (direction < 67.5f && direction >= 22.5f)
        {
            // UpRight
            sr.sprite = upRight;
        }
        else if (direction < 112.5f && direction >= 67.5f)
        {
            // Up
            sr.sprite = up;
        }
        else if (direction < 157.5f && direction >= 112.5f)
        {
            // UpLeft
            sr.sprite = upLeft;
        }
        else if (direction < 202.5f && direction >= 157.5f)
        {
            // Left
            sr.sprite = left;
        }
        else if (direction < 247.5f && direction >= 202.5f)
        {
            // DownLeft
            sr.sprite = downLeft;
        }
        else if (direction < 292.5f && direction >= 247.5f)
        {
            // Down
            sr.sprite = down;
        }
        else if (direction < 337.5f && direction >= 292.5f)
        {
            // DownRight
            sr.sprite = downRight;
        }

        // Movement of the car
        transform.position += new Vector3((speed / 3 * Time.deltaTime) * (xDirection / 2), (speed / 6 * Time.deltaTime) * (yDirection), 0);

        // Turn right/left
        if (turningRight == true && permissionToGo == true)
        {
            if (rotation == 45)
            {
                // UpRight(2, 1) to DownRight(2, -1)
                delay++;
                if (delay > 40)
                {
                    if (yDirection > -1)
                    {
                        yDirection -= 7.0f * Time.deltaTime * (maxSpeed / 12);
                        direction -= 6.0f;
                    }
                    else
                    {
                        yDirection = -1;
                        turningRight = false;
                    }
                }
            }
            else if (rotation == 135)
            {
                // UpLeft(-2, 1) to UpRight(2, 1)
                delay++;
                if (delay > 40)
                {
                    if (xDirection < 2)
                    {
                        xDirection += 14.0f * Time.deltaTime * (maxSpeed / 12);
                        direction -= 6.0f;
                    }
                    else
                    {
                        xDirection = 2;
                        turningRight = false;
                    }
                }
            }
            else if (rotation == 225)
            {
                // DownLeft(-2, -1) to UpLeft(-2, 1)
                delay++;
                if (delay > 40)
                {
                    if (yDirection < 1)
                    {
                        yDirection += 7.0f * Time.deltaTime * (maxSpeed / 12);
                        direction -= 6.0f;
                    }
                    else
                    {
                        yDirection = 1;
                        turningRight = false;
                    }
                }
            }
            else if (rotation == 315)
            {
                // DownRight(2, -1) to DownLeft(-2, -1)
                delay++;
                if (delay > 40)
                {
                    if (xDirection > -2)
                    {
                        xDirection -= 14.0f * Time.deltaTime * (maxSpeed / 12);
                        direction -= 6.0f;
                    }
                    else
                    {
                        xDirection = -2;
                        turningRight = false;
                    }
                }
            }
        }

        if (turningLeft == true && permissionToGo == true)
        {
            if (rotation == 45)
            {
                // UpRight(2, 1) to UpLeft(-2, 1)
                delay++;
                if (delay > 41)
                {
                    if (xDirection > -2)
                    {
                        xDirection -= 5.5f * Time.deltaTime * (maxSpeed / 12);
                        direction += 3.0f;

                        if (direction > 135)
                        {
                            direction = 135;
                        }
                    }
                    else
                    {
                        direction = 135;
                        xDirection = -2;
                        turningLeft = false;
                    }
                }
            }
            else if (rotation == 135)
            {
                // UpLeft(-2, 1) to DownLeft(-2, -1)
                delay++;
                if (delay > 41)
                {
                    if (yDirection > -1)
                    {
                        yDirection -= 2.75f * Time.deltaTime * (maxSpeed / 12);
                        direction += 3.0f;

                        if (direction > 225)
                        {
                            direction = 225;
                        }
                    }
                    else
                    {
                        direction = 225;
                        yDirection = -1;
                        turningLeft = false;
                    }
                }
            }
            else if (rotation == 225)
            {
                // DownLeft(-2, -1) to DownRight(2, -1)
                delay++;
                if (delay > 41)
                {
                    if (xDirection < 2)
                    {
                        xDirection += 5.5f * Time.deltaTime * (maxSpeed / 12);
                        direction++;

                        if (direction > 315)
                        {
                            direction = 315;
                        }
                    }
                    else
                    {
                        direction = 315;
                        xDirection = 2;
                        turningLeft = false;
                    }
                }
            }
            else if (rotation == 315)
            {
                // DownRight(2, -1) to UpRight(2, 1)
                delay++;
                if (delay > 41)
                {
                    if (yDirection < 1)
                    {
                        yDirection += 2.75f * Time.deltaTime * (maxSpeed / 12);
                        direction++;

                        if (direction > 45 && direction < 50)
                        {
                            direction = 45;
                        }
                    }
                    else
                    {
                        direction = 45;
                        yDirection = 1;
                        turningLeft = false;
                    }
                }
            }
        }

        // Destroy car if outside of view
        if ((transform.position.x < -15 || transform.position.x > 15) && pastIntersection == true)
        {
            gameManager.queue.Remove(gameObject.gameObject);
            Destroy(gameObject);
        }
    }

    // Mark the car as either inside of or outside of the intersection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Intersection" && permissionToGo == false)
        {
            speed = 0;
            atIntersection = true;

            gameManager.queue.Add(this.gameObject);

            // Choose which direction the car intends to turn
            if (turning == "")
            {
                if (gameManager.PercentChance(50))
                {
                    // 50%
                    turning = "forward";
                }
                else if (gameManager.PercentChance(50))
                {
                    // 25%
                    turning = "right";
                    turningRight = true;
                }
                else
                {
                    // 25%
                    turning = "left";
                    turningLeft = true;
                }
            }
        }
        else if (collision.transform.tag == "Intersection" && permissionToGo == true)
        {
            if (turning == "right")
            {
                turningRight = true;
                turningLeft = false;
            }
            else if (turning == "left")
            {
                turningLeft = true;
                turningRight = false;
            }

            if (rotation == 135)
            {
                intersection.SETurnSignalSR.sprite = null;
            }
            else if (rotation == 315)
            {
                intersection.NWTurnSignalSR.sprite = null;
            }
            else if (rotation == 45)
            {
                intersection.SWTurnSignalSR.sprite = null;
            }
            else if (rotation == 225)
            {
                intersection.NETurnSignalSR.sprite = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Intersection")
        {
            atIntersection = false;
            pastIntersection = true;
            turning = "";
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
