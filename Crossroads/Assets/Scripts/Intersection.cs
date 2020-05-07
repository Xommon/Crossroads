using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public int amountOfLanes;
    public GameManager gameManager;
    public List<string> rightOfWay = new List<string>();

    public SpriteRenderer NETurnSignalSR;
    public SpriteRenderer SETurnSignalSR;
    public SpriteRenderer SWTurnSignalSR;
    public SpriteRenderer NWTurnSignalSR;

    public Car NECarAtIntersection;
    public Car SECarAtIntersection;
    public Car SWCarAtIntersection;
    public Car NWCarAtIntersection;

    public Car car;

    // Sprites
    public Sprite turnForward;
    public Sprite turnRight;
    public Sprite turnLeft;

    // Start is called before the first frame update
    void Start()
    {
        rightOfWay.Clear();
        NETurnSignalSR.sprite = null;
        SETurnSignalSR.sprite = null;
        SWTurnSignalSR.sprite = null;
        NWTurnSignalSR.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Change turn signal based on car at intersection
        if (NECarAtIntersection != null)
        {
            if (NECarAtIntersection.turning == "forward")
            {
                NETurnSignalSR.sprite = turnForward;
            }
            else if (NECarAtIntersection.turning == "right")
            {
                NETurnSignalSR.sprite = turnRight;
            }
            else if (NECarAtIntersection.turning == "left")
            {
                NETurnSignalSR.sprite = turnLeft;
            }
            else
            {
                NETurnSignalSR.sprite = null;
            }
        }

        if (SECarAtIntersection != null)
        {
            if (SECarAtIntersection.turning == "forward")
            {
                SETurnSignalSR.sprite = turnForward;
            }
            else if (SECarAtIntersection.turning == "right")
            {
                SETurnSignalSR.sprite = turnRight;
            }
            else if (SECarAtIntersection.turning == "left")
            {
                SETurnSignalSR.sprite = turnLeft;
            }
            else
            {
                SETurnSignalSR.sprite = null;
            }
        }

        if (SWCarAtIntersection != null)
        {
            if (SWCarAtIntersection.turning == "forward")
            {
                SWTurnSignalSR.sprite = turnForward;
            }
            else if (SWCarAtIntersection.turning == "right")
            {
                SWTurnSignalSR.sprite = turnRight;
            }
            else if (SWCarAtIntersection.turning == "left")
            {
                SWTurnSignalSR.sprite = turnLeft;
            }
            else
            {
                SWTurnSignalSR.sprite = null;
            }
        }

        if (NWCarAtIntersection != null)
        {
            if (NWCarAtIntersection.turning == "forward")
            {
                NWTurnSignalSR.sprite = turnForward;
            }
            else if (NWCarAtIntersection.turning == "right")
            {
                NWTurnSignalSR.sprite = turnRight;
            }
            else if (NWCarAtIntersection.turning == "left")
            {
                NWTurnSignalSR.sprite = turnLeft;
            }
            else
            {
                NWTurnSignalSR.sprite = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Car>() != null)
        {
            // Tell the intersection when a car is there
            if (collision.gameObject.GetComponent<Car>().permissionToGo == false)
            {
                if (collision.gameObject.GetComponent<Car>().direction == 45)
                {
                    SWCarAtIntersection = collision.gameObject.GetComponent<Car>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(SWCarAtIntersection);
                }
                else if (collision.gameObject.GetComponent<Car>().direction == 135)
                {
                    SECarAtIntersection = collision.gameObject.GetComponent<Car>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(SECarAtIntersection);
                }
                else if (collision.gameObject.GetComponent<Car>().direction == 225)
                {
                    NECarAtIntersection = collision.gameObject.GetComponent<Car>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(NECarAtIntersection);
                }
                else if (collision.gameObject.GetComponent<Car>().direction == 315)
                {
                    NWCarAtIntersection = collision.gameObject.GetComponent<Car>();
                    gameManager.GetComponent<RightOfWay>().getVehicle(NWCarAtIntersection);
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Car>().direction == 45)
                {
                    SWCarAtIntersection = null;
                }
                else if (collision.gameObject.GetComponent<Car>().direction == 135)
                {
                    SECarAtIntersection = null;
                }
                else if (collision.gameObject.GetComponent<Car>().direction == 225)
                {
                    NECarAtIntersection = null;
                }
                else if (collision.gameObject.GetComponent<Car>().direction == 315)
                {
                    NWCarAtIntersection = null;
                }
            }
        }
    }
}
