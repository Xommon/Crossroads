using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public int amountOfLanes;
    public GameManager gameManager;
    public List<string> rightOfWay = new List<string>();

    public SpriteRenderer NTurnSignalSR;
    public SpriteRenderer ETurnSignalSR;
    public SpriteRenderer STurnSignalSR;
    public SpriteRenderer WTurnSignalSR;

    public Vehicle NVehicleAtIntersection;
    public Vehicle EVehicleAtIntersection;
    public Vehicle SVehicleAtIntersection;
    public Vehicle WVehicleAtIntersection;

    public Vehicle vehicle;

    // Sprites
    public Sprite turnForward;
    public Sprite turnRight;
    public Sprite turnLeft;

    // Start is called before the first frame update
    void Start()
    {
        rightOfWay.Clear();
        NTurnSignalSR.sprite = null;
        ETurnSignalSR.sprite = null;
        STurnSignalSR.sprite = null;
        WTurnSignalSR.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Change turn signal based on Vehicle at intersection
        if (NVehicleAtIntersection != null)
        {
            if (NVehicleAtIntersection.turnSignal == "forward")
            {
                NTurnSignalSR.sprite = turnForward;
            }
            else if (NVehicleAtIntersection.turnSignal == "right")
            {
                NTurnSignalSR.sprite = turnRight;
            }
            else if (NVehicleAtIntersection.turnSignal == "left")
            {
                NTurnSignalSR.sprite = turnLeft;
            }
            else
            {
                NTurnSignalSR.sprite = null;
            }
        }

        if (EVehicleAtIntersection != null)
        {
            if (EVehicleAtIntersection.turnSignal == "forward")
            {
                ETurnSignalSR.sprite = turnForward;
            }
            else if (EVehicleAtIntersection.turnSignal == "right")
            {
                ETurnSignalSR.sprite = turnRight;
            }
            else if (EVehicleAtIntersection.turnSignal == "left")
            {
                ETurnSignalSR.sprite = turnLeft;
            }
            else
            {
                ETurnSignalSR.sprite = null;
            }
        }

        if (SVehicleAtIntersection != null)
        {
            if (SVehicleAtIntersection.turnSignal == "forward")
            {
                STurnSignalSR.sprite = turnForward;
            }
            else if (SVehicleAtIntersection.turnSignal == "right")
            {
                STurnSignalSR.sprite = turnRight;
            }
            else if (SVehicleAtIntersection.turnSignal == "left")
            {
                STurnSignalSR.sprite = turnLeft;
            }
            else
            {
                STurnSignalSR.sprite = null;
            }
        }

        if (WVehicleAtIntersection != null)
        {
            if (WVehicleAtIntersection.turnSignal == "forward")
            {
                WTurnSignalSR.sprite = turnForward;
            }
            else if (WVehicleAtIntersection.turnSignal == "right")
            {
                WTurnSignalSR.sprite = turnRight;
            }
            else if (WVehicleAtIntersection.turnSignal == "left")
            {
                WTurnSignalSR.sprite = turnLeft;
            }
            else
            {
                WTurnSignalSR.sprite = null;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Vehicle>() != null)
        {
            // Tell the intersection when a Vehicle is there
            if (collision.gameObject.GetComponent<Vehicle>().permissionToGo == false)
            {
                if (collision.gameObject.GetComponent<Vehicle>().direction == 0)
                {
                    NVehicleAtIntersection = collision.gameObject.GetComponent<Vehicle>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(NVehicleAtIntersection);
                }
                else if (collision.gameObject.GetComponent<Vehicle>().direction == 90)
                {
                    EVehicleAtIntersection = collision.gameObject.GetComponent<Vehicle>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(EVehicleAtIntersection);
                }
                else if (collision.gameObject.GetComponent<Vehicle>().direction == 180)
                {
                    SVehicleAtIntersection = collision.gameObject.GetComponent<Vehicle>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(SVehicleAtIntersection);
                }
                else if (collision.gameObject.GetComponent<Vehicle>().direction == 270)
                {
                    WVehicleAtIntersection = collision.gameObject.GetComponent<Vehicle>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(WVehicleAtIntersection);
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Vehicle>().direction == 0)
                {
                    NVehicleAtIntersection = null;
                }
                else if (collision.gameObject.GetComponent<Vehicle>().direction == 90)
                {
                    EVehicleAtIntersection = null;
                }
                else if (collision.gameObject.GetComponent<Vehicle>().direction == 180)
                {
                    SVehicleAtIntersection = null;
                }
                else if (collision.gameObject.GetComponent<Vehicle>().direction == 270)
                {
                    WVehicleAtIntersection = null;
                }
            }
        }
    }
}
