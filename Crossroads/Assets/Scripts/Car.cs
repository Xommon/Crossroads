using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public string direction;
    public bool canGo;

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

        if (direction == "DR")
        {
            transform.position += new Vector3((speed / 100), -(speed / 200), 0);
            sr.sprite = downRight;
        }
        else if (direction == "UR")
        {
            transform.position += new Vector3((speed / 100), (speed / 200), 0);
            sr.sprite = upRight;
        }
        else if (direction == "UL")
        {
            transform.position += new Vector3(-(speed / 100), (speed / 200), 0);
            sr.sprite = upLeft;
        }
        else if (direction == "DL")
        {
            transform.position += new Vector3(-(speed / 100), -(speed / 200), 0);
            sr.sprite = downLeft;
        }
    }
}
