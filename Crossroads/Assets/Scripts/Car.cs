using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameManager gameManager;
    public float speed;
    public float maxSpeed;
    public float acceleration;
    public Vector3 direction;
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

    // Sprites
    public SpriteRenderer sr;
    public Sprite downRight;
    public Sprite upRight;
    public Sprite downLeft;
    public Sprite upLeft;

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

        if (transform.position.x < 0 && transform.position.y >= 0)
        {
            // Down Right
            direction = new Vector3(2, -1, 0);
        }
        else if (transform.position.x < 0 && transform.position.y < 0)
        {
            // Up Right
            direction = new Vector3(2, 1, 0);
        }
        else if (transform.position.x >= 0 && transform.position.y < 0)
        {
            // Up Left
            direction = new Vector3(-2, 1, 0);
        }
        else if (transform.position.x >= 0 && transform.position.y >= 0)
        {
            // Down Left
            direction = new Vector3(-2, -1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (atIntersection && !permissionToGo)
        {
            canGo = false;
        }

        // Stop the car before it hits obstacles
        Debug.DrawRay(transform.position + new Vector3(0.25f * direction.x, 0.25f * direction.y, 0), direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.25f * direction.x, 0.25f * direction.y, 0), direction, 3.0f);
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

        // Point the car in the correct direction
        if (direction == new Vector3(2, -1, 0))
        {
            transform.position += new Vector3((speed / 100), -(speed / 200), 0);
            sr.sprite = downRight;
        }
        else if (direction == new Vector3(2, 1, 0))
        {
            transform.position += new Vector3((speed / 100), (speed / 200), 0);
            sr.sprite = upRight;
        }
        else if (direction == new Vector3(-2, 1, 0))
        {
            transform.position += new Vector3(-(speed / 100), (speed / 200), 0);
            sr.sprite = upLeft;
        }
        else if (direction == new Vector3(-2, -1, 0))
        {
            transform.position += new Vector3(-(speed / 100), -(speed / 200), 0);
            sr.sprite = downLeft;
        }

        // Allow car to go through intersection when clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hitMouse = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hitMouse.collider != null)
            {
                if (hitMouse.collider.gameObject.GetComponent<Car>().atIntersection == true)
                {
                    hitMouse.collider.gameObject.GetComponent<Car>().permissionToGo = true;
                    hitMouse.collider.gameObject.GetComponent<Car>().canGo = true;

                    if (turning == "left")
                    {
                        hitMouse.collider.gameObject.GetComponent<Car>().Left();
                    }
                    else if (turning == "right")
                    {
                        hitMouse.collider.gameObject.GetComponent<Car>().Right();
                    }
                }
            }
        }
        
        // Destroy car if outside of view
        if ((transform.position.x < -13 || transform.position.x > 13) && pastIntersection == true)
        {
            gameManager.queue.Remove(gameObject.gameObject);
            if (direction == new Vector3(2, -1, 0))
            {
                gameManager.nwQueue.Remove(gameObject.gameObject);
            }
            else if (direction == new Vector3(2, 1, 0))
            {
                gameManager.swQueue.Remove(gameObject.gameObject);
            }
            else if (direction == new Vector3(-2, 1, 0))
            {
                gameManager.seQueue.Remove(gameObject.gameObject);
            }
            else if (direction == new Vector3(-2, -1, 0))
            {
                gameManager.neQueue.Remove(gameObject.gameObject);
            }
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

            gameManager.queue.Add(gameObject.gameObject);

            // Choose which direction the car intends to turn
            if (turning != "forward" || turning != "right" || turning != "left")
            {
                if (gameManager.PercentChance(50))
                {
                    // 50%
                    turning = "forward";
                    turnSignal.sprite = turnForward;
                }
                else if (gameManager.PercentChance(50))
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Intersection")
        {
            atIntersection = false;
            pastIntersection = true;
        }
    }

    public Vector2 SpeedMultiple(float multiple)
    {
        return new Vector2(multiple, ((multiple + 3) / 4));
    }

    public void Right()
    {
        if (direction == new Vector3(2, -1, 0))
        {
            //DownRight to DownLeft
            direction = new Vector3(-2, -1, 0);
        }
        else if (direction == new Vector3(-2, -1, 0))
        {
            //DownLeft to UpLeft
            direction = new Vector3(-2, 1, 0);
        }
        else if (direction == new Vector3(-2, 1, 0))
        {
            //UpLeft to UpRight
            direction = new Vector3(2, 1, 0);
        }
        else if (direction == new Vector3(2, 1, 0))
        {
            //UpRight to DownRight
            direction = new Vector3(2, -1, 0);
        }
    }

    public void Left()
    {
        if (direction == new Vector3(2, -1, 0))
        {
            //DownRight to UpRight
            direction = new Vector3(2, 1, 0);
        }
        else if (direction == new Vector3(-2, -1, 0))
        {
            //DownLeft to DownRight
            direction = new Vector3(2, -1, 0);
        }
        else if (direction == new Vector3(-2, 1, 0))
        {
            //UpLeft to DownLeft
            direction = new Vector3(-2, -1, 0);
        }
        else if (direction == new Vector3(2, 1, 0))
        {
            //UpRight to UpLeft
            direction = new Vector3(-2, 1, 0);
        }
    }
}
