﻿using System.Collections;
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
    public GameObject intersection;
    public GameObject objectInWay;
    public float distanceFromObjectInWay;
    public string turning;
    public SpriteRenderer turnSignal;
    public Sprite turnForward;
    public Sprite turnRight;
    public Sprite turnLeft;
    public bool turningRight;
    public bool turningLeft;
    Vector3 newDirection = new Vector3();
    public float xDirection;
    public float yDirection;

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
        speed = 0;
        canGo = true;
        pastIntersection = false;
        permissionToGo = false;
        atIntersection = false;
        emergencyStop = 1;
        objectInWay = null;
        turnSignal.sprite = null;
        turningRight = false;
        turningLeft = false;

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
        if (atIntersection && !permissionToGo)
        {
            canGo = false;
        }

        turnSignal.transform.rotation = new Quaternion(0, 0, direction, 0);

        // Stop the car before it hits obstacles
        Debug.DrawRay(transform.position + new Vector3(0.25f * xDirection, 0.25f * yDirection, 0), new Vector3(xDirection, yDirection, 0), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.25f * xDirection, 0.25f * yDirection, 0), new Vector3(xDirection, yDirection, 0), 3.0f);
        if (hit) // Don't edit
        {
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
                else if (distanceFromObjectInWay < 3f && distanceFromObjectInWay >= 2.5f)
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
                else if (distanceFromObjectInWay < 2.5f && distanceFromObjectInWay >= 1.5f)
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
                else if (distanceFromObjectInWay < 1.5f && distanceFromObjectInWay >= 0)
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

        if (direction < 22.5f && direction >= 337.5f)
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

        transform.position += new Vector3((speed / 3 * Time.deltaTime) * (xDirection / 2), (speed / 6 * Time.deltaTime) * (yDirection), 0);

        // Allow car to go through intersection when clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hitMouse = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hitMouse.collider != null)
            {
                if (hitMouse.collider.gameObject.GetComponent<Car>() != null)
                {
                    if (hitMouse.collider.gameObject.GetComponent<Car>().atIntersection == true)
                    {
                        hitMouse.collider.gameObject.GetComponent<Car>().permissionToGo = true;
                        hitMouse.collider.gameObject.GetComponent<Car>().canGo = true;

                        if (turning == "left")
                        {
                            turningLeft = true;
                            turningRight = false;
                        }
                        else if (turning == "right")
                        {
                            turningRight = true;
                            turningLeft = false;
                        }
                    }
                }
            }
        }

        // Turn right/left
        if (turningRight == true && permissionToGo == true)
        {
            if (rotation == 45)
            {
                // UpRight(2, 1) to DownRight(2, -1)
                if (yDirection > -1)
                {
                    yDirection -= 1.25f * Time.deltaTime;
                    direction--;
                }
                else
                {
                    yDirection = -1;
                    turningRight = false;
                }
            }
            else if (rotation == 135)
            {
                // UpLeft(-2, 1) to UpRight(2, 1)
                if (xDirection < 2)
                {
                    xDirection += 2.5f * Time.deltaTime;
                    direction--;
                }
                else
                {
                    xDirection = 2;
                    turningRight = false;
                }
            }
            else if (rotation == 225)
            {
                // DownLeft(-2, -1) to UpLeft(-2, 1)
                if (yDirection < 1)
                {
                    yDirection += 1.25f * Time.deltaTime;
                    direction--;
                }
                else
                {
                    yDirection = 1;
                    turningRight = false;
                }
            }
            else if (rotation == 315)
            {
                // DownRight(2, -1) to DownLeft(-2, -1)
                if (xDirection > -2)
                {
                    xDirection -= 2.5f * Time.deltaTime;
                    direction--;
                }
                else
                {
                    xDirection = -2;
                    turningRight = false;
                }
            }
        }

        if (turningLeft == true)
        {
            //Vector3 newDirection = new Vector3();

            /*if (direction == new Vector3(2, -1, 0))
            {
                //DownRight to UpRight
                newDirection = new Vector3(2, 1, 0);
            }
            else if (direction == new Vector3(-2, -1, 0))
            {
                //DownLeft to DownRight
                newDirection = new Vector3(2, -1, 0);
            }
            else if (direction == new Vector3(-2, 1, 0))
            {
                //UpLeft to DownLeft
                newDirection = new Vector3(-2, -1, 0);
            }
            else if (direction == new Vector3(2, 1, 0))
            {
                //UpRight to UpLeft
                newDirection = new Vector3(-2, 1, 0);
            }

            if (direction.x < newDirection.x)
            {
                direction += new Vector3(0.2f, 0, 0);
            }
            else if (direction.x > newDirection.x)
            {
                direction -= new Vector3(0.2f, 0, 0);
            }

            if (direction.y < newDirection.y)
            {
                direction += new Vector3(0, 0.1f, 0);
            }
            else if (direction.y > newDirection.y)
            {
                direction -= new Vector3(0, 0.1f, 0);
            }

            if (direction == newDirection)
            {
                turningLeft = false;
            }*/
        }

        // Destroy car if outside of view
        if ((transform.position.x < -13 || transform.position.x > 13) && pastIntersection == true)
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
                if (gameManager.PercentChance(0))
                {
                    // 50%
                    turning = "forward";
                    turnSignal.sprite = turnForward;
                }
                else if (gameManager.PercentChance(100))
                {
                    // 25%
                    turning = "right";
                    turnSignal.sprite = turnRight;
                }
                else
                {
                    // 25%
                    turning = "left";
                    turnSignal.sprite = turnLeft;
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Intersection")
        {
            atIntersection = false;
            pastIntersection = true;

            turnSignal.sprite = null;
            turning = "";
        }
    }
}
