﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(car, new Vector3(10.54f, -5.74f, 2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
