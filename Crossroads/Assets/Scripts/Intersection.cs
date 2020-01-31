using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public int amountOfLanes;
    public GameManager gameManager;

    public SpriteRenderer NETurnSignalSR;
    public SpriteRenderer SETurnSignalSR;
    public SpriteRenderer SWTurnSignalSR;
    public SpriteRenderer NWTurnSignalSR;

    // Start is called before the first frame update
    void Start()
    {
        NETurnSignalSR.sprite = null;
        SETurnSignalSR.sprite = null;
        SWTurnSignalSR.sprite = null;
        NWTurnSignalSR.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
