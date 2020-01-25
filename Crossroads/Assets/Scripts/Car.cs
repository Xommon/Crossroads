using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float acceleration;
    public string direction;
    public bool canGo;
    public bool permissionToGo;
    public bool stopped;
    public float distanceFromIntersection;
    public bool atIntersection;

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
        maxSpeed = 9f;
        acceleration = 0.2f;
        canGo = true;
        stopped = false;
        permissionToGo = false;
        atIntersection = false;

        if (transform.position.x < 0 && transform.position.y >= 0)
        {
            direction = "DR";
        }
        else if (transform.position.x < 0 && transform.position.y < 0)
        {
            direction = "UR";
        }
        else if (transform.position.x >= 0 && transform.position.y < 0)
        {
            direction = "UL";
        }
        else if (transform.position.x >= 0 && transform.position.y >= 0)
        {
            direction = "DL";
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromIntersection = Vector3.Distance(transform.position, Vector3.zero);

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
                speed -= (acceleration * 2);
            }
            else
            {
                speed = 0;
            }
        }

        // Point the car in the correct direction
        if (direction == "DR")
        {
            transform.position += new Vector3((speed / 100), -(speed / 200), 0);
            sr.sprite = downRight;

            if (Vector3.Distance(transform.position, Vector3.zero) < 3.85 && permissionToGo == false)
            {
                canGo = false;

                if (speed == 0)
                {
                    atIntersection = true;
                }
            }
        }
        else if (direction == "UR")
        {
            transform.position += new Vector3((speed / 100), (speed / 200), 0);
            sr.sprite = upRight;

            if (Vector3.Distance(transform.position, Vector3.zero) < 3.7 && permissionToGo == false)
            {
                canGo = false;

                if (speed == 0)
                {
                    atIntersection = true;
                }
            }
        }
        else if (direction == "UL")
        {
            transform.position += new Vector3(-(speed / 100), (speed / 200), 0);
            sr.sprite = upLeft;

            if (Vector3.Distance(transform.position, Vector3.zero) < 3.55 && permissionToGo == false)
            {
                canGo = false;

                if (speed == 0)
                {
                    atIntersection = true;
                }
            }
        }
        else if (direction == "DL")
        {
            transform.position += new Vector3(-(speed / 100), -(speed / 200), 0);
            sr.sprite = downLeft;

            if (Vector3.Distance(transform.position, Vector3.zero) < 2.6 && permissionToGo == false)
            {
                canGo = false;

                if (speed == 0)
                {
                    atIntersection = true;
                }
            }
        }

        // Allow car to go through intersection when clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Car>().atIntersection == true)
                {
                    hit.collider.gameObject.GetComponent<Car>().permissionToGo = true;
                    hit.collider.gameObject.GetComponent<Car>().canGo = true;
                }
            }
        }
    }
}
