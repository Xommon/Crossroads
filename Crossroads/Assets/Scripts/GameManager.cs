﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject car;
    public GameObject truck;
    public float gameTime;

    public List<GameObject> queue = new List<GameObject>();
    public List<GameObject> neQueue = new List<GameObject>();
    public List<GameObject> seQueue = new List<GameObject>();
    public List<GameObject> nwQueue = new List<GameObject>();
    public List<GameObject> swQueue = new List<GameObject>();

    // Time
    public float seconds;
    public int minutes;
    public string time;

    public int vehicleSpawnTimer;

    // Percent
    public bool PercentChance(float percent)
    {
        if (percent > 100)
        {
            percent = 100;
        }
        else if (percent < 0)
        {
            percent = 0;
        }

        float selection = Random.Range(0.0f, 100.0f);
        if (selection <= percent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTime = 1;
    }

    void Update()
    {
        // Game Time
        Time.timeScale = gameTime;

        if (seconds.ToString().Length > 1)
        {
            if (seconds.ToString().Substring(1, 1) == ".")
            {
                time = minutes + ":0" + seconds.ToString().Substring(0, 1);
            }
            else
            {
                time = minutes + ":" + seconds.ToString().Substring(0, 2);
            }
        }
        
        // Tick the clock
        if (seconds < 60)
        {
            seconds += Time.deltaTime;
            if (vehicleSpawnTimer > 100)
            {
                vehicleSpawnTimer = 0;
                CreateNewVehicle();
            }
            else
            {
                vehicleSpawnTimer += Mathf.RoundToInt(seconds);
            }
        }
        else
        {
            minutes++;
            seconds = 0;
        }

        // DEBUG KEYS
        if (Input.GetKeyDown("r"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void CreateNewVehicle()
    {
        Vector3 newPosition = Vector3.zero;
        GameObject newVehicle;

        if (PercentChance(50))
        {
            newVehicle = car;
        }
        else
        {
            newVehicle = car;
        }

        if (PercentChance(25))
        {
            if (seQueue.Count < 7)
            {
                newPosition = new Vector3(10.86f, -5.94f, 2);
                seQueue.Add(newVehicle);

                Instantiate(newVehicle, newPosition, Quaternion.identity);
            }
        }
        else if (PercentChance(33.33f))
        {
            if (swQueue.Count < 8)
            {
                newPosition = new Vector3(-8.93f, -5.36f, 2);
                swQueue.Add(newVehicle);

                Instantiate(newVehicle, newPosition, Quaternion.identity);
            }
        }
        else if (PercentChance(50))
        {
            if (nwQueue.Count < 8)
            {
                newPosition = new Vector3(-10.68f, 3.96f, 2);
                nwQueue.Add(newVehicle);

                Instantiate(newVehicle, newPosition, Quaternion.identity);
            }
        }
        else
        {
            if (neQueue.Count < 8)
            {
                newPosition = new Vector3(9.942678f, 5.034575f, 2);
                neQueue.Add(newVehicle);

                Instantiate(newVehicle, newPosition, Quaternion.identity);
            }
        }
    }
}
