using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vehicle car;
    //public Car truck;
    public float gameTime;
    public Intersection intersection;
    public List<Vehicle> vehicles = new List<Vehicle>();
    public int vehicleCounter;
    public int playerCounter;
    public int selection;

    public List<Vehicle> queue = new List<Vehicle>();
    public List<Vehicle> neQueue = new List<Vehicle>();
    public List<Vehicle> seQueue = new List<Vehicle>();
    public List<Vehicle> nwQueue = new List<Vehicle>();
    public List<Vehicle> swQueue = new List<Vehicle>();
    public List<Object> testQueue = new List<Object>();

    // Time
    public float seconds;
    public float allSeconds;
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

    private void Awake()
    {
        Application.targetFrameRate = 300;
        vehicleCounter = 1;
        playerCounter = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTime = 1;
        intersection = FindObjectOfType<Intersection>();
        vehicles.Add(car);
        //vehicles.Add(truck);
    }

    void FixedUpdate()
    {
        // Game Time
        Time.timeScale = gameTime;

        // Format the seconds for the time
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

        allSeconds += Time.deltaTime;

        // DEBUG KEYS
        if (Input.GetKeyDown("r"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        // Allow car to go through intersection when clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hitMouse = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hitMouse.collider != null)
            {
                if (hitMouse.collider.gameObject.transform.tag == "ClickArea")
                {
                    if (hitMouse.collider.gameObject.transform.name == "SWClickArea" && intersection.SWCarAtIntersection != null)
                    {
                        intersection.SWCarAtIntersection.permissionToGo = true;
                        intersection.SWCarAtIntersection.canGo = true;

                        for (int i = 0; i < gameObject.GetComponent<RightOfWay>().rightOfWay.Count - 1; i++)
                        {
                            if (gameObject.GetComponent<RightOfWay>().rightOfWay[i].y == playerCounter)
                            {
                                selection = playerCounter;
                                break;
                            }
                            else
                            {
                                selection = -1;
                            }
                        }

                        if (selection > -1)
                        {
                            Debug.Log("Success!");
                        }
                        else
                        {
                            Debug.Log("Failure!");
                        }
                    }
                    else if(hitMouse.collider.gameObject.transform.name == "SEClickArea" && intersection.SECarAtIntersection != null)
                    {
                        intersection.SECarAtIntersection.permissionToGo = true;
                        intersection.SECarAtIntersection.canGo = true;

                        if (seQueue[0].gameObject.name == playerCounter.ToString())
                        {
                            Debug.Log("Success!");
                        }
                        else
                        {
                            Debug.Log("Failure!");
                        }
                    }
                    else if(hitMouse.collider.gameObject.transform.name == "NEClickArea" && intersection.NECarAtIntersection != null)
                    {
                        intersection.NECarAtIntersection.permissionToGo = true;
                        intersection.NECarAtIntersection.canGo = true;

                        if (neQueue[0].gameObject.name == playerCounter.ToString())
                        {
                            Debug.Log("Success!");
                        }
                        else
                        {
                            Debug.Log("Failure!");
                        }
                    }
                    else if(hitMouse.collider.gameObject.transform.name == "NWClickArea" && intersection.NWCarAtIntersection != null)
                    {
                        intersection.NWCarAtIntersection.permissionToGo = true;
                        intersection.NWCarAtIntersection.canGo = true;

                        if (nwQueue[0].gameObject.name == playerCounter.ToString())
                        {
                            Debug.Log("Success!");
                        }
                        else
                        {
                            Debug.Log("Failure!");
                        }
                    }
                }
            }
        }
    }

    // Spawn a new vehicle
    public void CreateNewVehicle()
    {
        Vector3 newPosition = Vector3.zero;
        int select = Random.Range(0, vehicles.Count);
        Vehicle newVehicle = vehicles[select];

        if (PercentChance(25))
        {
            if (seQueue.Count < 7)
            {
                newPosition = new Vector3(10.86f, -5.94f, 2);
                seQueue.Add(newVehicle);
                queue.Add(newVehicle);

                Vehicle createVehicle = Instantiate(newVehicle, newPosition, Quaternion.identity);
                createVehicle.GetComponent<Vehicle>().name = vehicleCounter.ToString();
                vehicleCounter++;
            }
        }
        else if (PercentChance(33.33f))
        {
            if (swQueue.Count < 8)
            {
                newPosition = new Vector3(-8.93f, -5.36f, 2);
                swQueue.Add(newVehicle);
                queue.Add(newVehicle);

                Vehicle createVehicle = Instantiate(newVehicle, newPosition, Quaternion.identity);
                createVehicle.GetComponent<Vehicle>().name = vehicleCounter.ToString();
                vehicleCounter++;
            }
        }
        else if (PercentChance(50))
        {
            if (nwQueue.Count < 8)
            {
                newPosition = new Vector3(-10.68f, 3.96f, 2);
                nwQueue.Add(newVehicle);
                queue.Add(newVehicle);

                Vehicle createVehicle = Instantiate(newVehicle, newPosition, Quaternion.identity);
                createVehicle.GetComponent<Vehicle>().name = vehicleCounter.ToString();
                vehicleCounter++;
            }
        }
        else
        {
            if (neQueue.Count < 8)
            {
                newPosition = new Vector3(9.942678f, 5.034575f, 2);
                neQueue.Add(newVehicle);
                queue.Add(newVehicle);

                Vehicle createVehicle = Instantiate(newVehicle, newPosition, Quaternion.identity);
                createVehicle.GetComponent<Vehicle>().name = vehicleCounter.ToString();
                vehicleCounter++;
            }
        }
    }
}
