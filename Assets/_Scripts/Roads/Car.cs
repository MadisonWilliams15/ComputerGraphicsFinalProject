using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Road entryPoint;
    public Queue<LightDirection> path;
    public int startTime;
    public int endTime;
    public int distanceTraveled;
    public Nullable<int> frameMoved;
    public int waitTime; 

    private void Update()
    {
        
    }

    
}

//put a field in the car class
//have an intersection assign direction
// update car field, have intersection change it