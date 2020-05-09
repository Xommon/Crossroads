using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float startSpeed;
    public float maxSpeed;
    public float speed;
    public float turning;
    public float acceleration;
    public bool canGo;
    public List<GameObject> wheels = new List<GameObject>();

    void Start()
    {
        canGo = true;

        // Create list of wheels
        foreach (Transform child in transform)
        {
            wheels.Add(child.gameObject);
        }
    }
    
    void Update()
    {
        // Move the car
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
                speed -= (acceleration * 1.5f); //* emergencyStop
            }
            else
            {
                speed = 0;
            }
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
