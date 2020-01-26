using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float acceleration;
    public Vector3 direction;
    public float emergencyStop;
    public bool canGo;
    public bool permissionToGo;
    public bool stopped;
    public float distanceFromIntersection;
    public bool atIntersection;
    public Collider2D frontBumper;
    public Collider2D backBumper;
    public GameObject intersection;

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
        maxSpeed = 12f;
        acceleration = 0.5f;
        canGo = true;
        stopped = false;
        permissionToGo = false;
        atIntersection = false;
        emergencyStop = 1;

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
        // Stop the car before it hits obstacles
        Debug.DrawRay(transform.position + new Vector3(0.25f * direction.x, 0.25f * direction.y, 0), direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.25f * direction.x, 0.25f * direction.y, 0), direction, 0.75f);
        if (hit) // Don't edit
        {
            Intersection intersection = hit.transform.GetComponent<Intersection>();
            Car car = hit.transform.GetComponent<Car>();

            if (intersection != null && permissionToGo == false)
            {
                canGo = false;

                if (speed == 0)
                {
                    atIntersection = true;
                }
            }
            else if (car != null)
            {
                canGo = false;
            }
            else
            {
                canGo = true;
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
                speed -= (acceleration * 2 * emergencyStop);
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
                    hitMouse.collider.gameObject.GetComponent<Car>().atIntersection = false;
                }
            }
        }
    }
}
