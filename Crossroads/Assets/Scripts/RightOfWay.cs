using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightOfWay : MonoBehaviour
{
    public GameManager gameManager;
    public List<Vector3> rightOfWay = new List<Vector3>(); // (x = Vehicle ID Number, y = Order of Right of Way, z = Direction)
    public int vehicleCount;
    public float previousTime;

    /*
    * 1: car arrives
    * 2: time recorded
    * 3: second car arrives
    * 4: time recorded - current time
    * 5: if less than 1, then the cars arrived at the same time
    * 
    * - Complete a complex script that determines actual Right of Way for vehicles that arrive at the same time, 
    * maybe within a 1 second window of one car arriving to help with player identification. The cars' names 
    * should be saved in a list, instead of their game object, since their names are their ID numbers.
    * 
    */

    private void Start()
    {
        // Assign variables
        vehicleCount = 1;
    }

    public void getVehicle(Car vehicle)
    {
        // Add vechile and assign it an order
        if (rightOfWay.Count == 0)
        {
            // First car to intersection
            rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
            previousTime = gameManager.allSeconds;
        }
        else
        {
            // If vehicles arrive within a quarter second of each other, then they're competing for right of way
            if (gameManager.allSeconds - previousTime <= 0.25f)
            {
                // South West
                if (vehicle.direction == 45)
                {
                    // Other vehicle is to the left, granting its right of way
                    if (rightOfWay[vehicleCount].z == 315)
                    {
                        // Save the previous vehicle's information
                        float previousVehicleNumber = rightOfWay[vehicleCount].x;
                        float previousDirection = rightOfWay[vehicleCount].z;

                        // Delete and Readd previous vehicle
                        rightOfWay.RemoveAt(vehicleCount);
                        rightOfWay.Add(new Vector3(previousVehicleNumber, vehicleCount + 1, previousDirection));

                        // Add new vehicle in its place
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    // Other vehicle is to the right, yielding its right of way
                    else if (rightOfWay[vehicleCount].z == 135)
                    {
                        // Add new vehicle
                        vehicleCount++;
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    else
                    // Other vehicle is straight ahead, both cars have the right of way
                    {
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                }
                // North West
                else if (vehicle.direction == 135)
                {
                    // Other vehicle is to the left, granting its right of way
                    if (rightOfWay[vehicleCount].z == 45)
                    {
                        // Save the previous vehicle's information
                        float previousVehicleNumber = rightOfWay[vehicleCount].x;
                        float previousDirection = rightOfWay[vehicleCount].z;

                        // Delete and Readd previous vehicle
                        rightOfWay.RemoveAt(vehicleCount);
                        rightOfWay.Add(new Vector3(previousVehicleNumber, vehicleCount + 1, previousDirection));

                        // Add new vehicle in its place
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    // Other vehicle is to the right, yielding its right of way
                    else if (rightOfWay[vehicleCount].z == 225)
                    {
                        // Add new vehicle
                        vehicleCount++;
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    else
                    // Other vehicle is straight ahead, both cars have the right of way
                    {
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                }
                // South West
                else if (vehicle.direction == 225)
                {
                    // Other vehicle is to the left, granting its right of way
                    if (rightOfWay[vehicleCount].z == 135)
                    {
                        // Save the previous vehicle's information
                        float previousVehicleNumber = rightOfWay[vehicleCount].x;
                        float previousDirection = rightOfWay[vehicleCount].z;

                        // Delete and Readd previous vehicle
                        rightOfWay.RemoveAt(vehicleCount);
                        rightOfWay.Add(new Vector3(previousVehicleNumber, vehicleCount + 1, previousDirection));

                        // Add new vehicle in its place
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    // Other vehicle is to the right, yielding its right of way
                    else if (rightOfWay[vehicleCount].z == 315)
                    {
                        // Add new vehicle
                        vehicleCount++;
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    else
                    // Other vehicle is straight ahead, both cars have the right of way
                    {
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                }
                // South East
                else if (vehicle.direction == 315)
                {
                    // Other vehicle is to the left, granting its right of way
                    if (rightOfWay[vehicleCount].z == 225)
                    {
                        // Save the previous vehicle's information
                        float previousVehicleNumber = rightOfWay[vehicleCount].x;
                        float previousDirection = rightOfWay[vehicleCount].z;

                        // Delete and Readd previous vehicle
                        rightOfWay.RemoveAt(vehicleCount);
                        rightOfWay.Add(new Vector3(previousVehicleNumber, vehicleCount + 1, previousDirection));

                        // Add new vehicle in its place
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    // Other vehicle is to the right, yielding its right of way
                    else if (rightOfWay[vehicleCount].z == 45)
                    {
                        // Add new vehicle
                        vehicleCount++;
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                    else
                    // Other vehicle is straight ahead, both cars have the right of way
                    {
                        rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                    }
                }
            }
            // Vehicle did not arrive at the same time as another vehicle, add in order
            else
            {
                vehicleCount++;
                rightOfWay.Add(new Vector3(int.Parse(vehicle.name), vehicleCount, vehicle.direction));
                previousTime = gameManager.allSeconds;
            }
        }
    }
}
