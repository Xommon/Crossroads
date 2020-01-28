using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float xDirection;
    public float yDirection;

    // Start is called before the first frame update
    void Start()
    {
        xDirection = -0.08f;
        yDirection = 0.04f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xDirection, yDirection, 0);

        if (xDirection < 0.08)
        {
            xDirection += 0.008f;
        }
        else
        {
            xDirection = 0.08f;
        }
    }
}
