using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public string direction;
    public bool canGo;
    public bool stopped;
    public float distanceFromIntersection;

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
        canGo = true;
        stopped = false;

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
                speed += 0.2f;
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
                speed -= 0.4f;
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

            if (Vector3.Distance(transform.position, Vector3.zero) < 3.82)
            {
                canGo = false;
            }
        }
        else if (direction == "UR")
        {
            transform.position += new Vector3((speed / 100), (speed / 200), 0);
            sr.sprite = upRight;

            if (Vector3.Distance(transform.position, Vector3.zero) < 3.7)
            {
                canGo = false;
            }
        }
        else if (direction == "UL")
        {
            transform.position += new Vector3(-(speed / 100), (speed / 200), 0);
            sr.sprite = upLeft;

            if (Vector3.Distance(transform.position, Vector3.zero) < 3.6)
            {
                canGo = false;
            }
        }
        else if (direction == "DL")
        {
            transform.position += new Vector3(-(speed / 100), -(speed / 200), 0);
            sr.sprite = downLeft;

            if (Vector3.Distance(transform.position, Vector3.zero) < 2.8)
            {
                canGo = false;
            }
        }
    }
}
