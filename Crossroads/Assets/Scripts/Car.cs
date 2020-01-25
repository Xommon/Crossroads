using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public bool go;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        maxSpeed = 7f;
        go = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (go == true)
        {
            if (speed < maxSpeed)
            {
                // Accelerate
                speed += 0.1f;
            }
            else
            {
                speed = maxSpeed;
            }

            transform.position += new Vector3((speed / 100), -(speed / 200), 0);
        }
    }
}
