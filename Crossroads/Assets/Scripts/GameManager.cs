using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject car;
    public GameObject truck;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(car, new Vector3(10.86f, -5.94f, 2), Quaternion.identity);
        Instantiate(car, new Vector3(-8.93f, -5.36f, 2), Quaternion.identity);
        Instantiate(car, new Vector3(-10.68f, 3.96f, 2), Quaternion.identity);
        Instantiate(car, new Vector3(9.942678f, 5.034575f, 2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
