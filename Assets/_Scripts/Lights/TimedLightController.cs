using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLightController : AbstractLightController
{
    public int greenTime = 10;
    public int yellowTime = 4;
    private int lightRemaining;

    void Start()
    {
        lightRemaining = greenTime;
    }
    void Update()
    {
        lightRemaining--;
        if (0 >= lightRemaining)
        {
            if (LightColor.Green == lightColor)
            {
                lightRemaining = yellowTime;
                SetLightColor(LightColor.Yellow);
            }
            else
            {
                lightRemaining = greenTime;
                SetLightColor(LightColor.Green);
                //LightDirection = (Direction)((((int)LightDirection) + 1) % ((int)Enum.GetNames(typeof(Direction)).Length));
                lightDirection = LightDirection.CrossNorthSouth == lightDirection ? LightDirection.CrossEastWest : LightDirection.CrossNorthSouth;//Lights oscillate between alternating crosses
            }
        }//Cycle from green to yellow and around directions
    }
}
