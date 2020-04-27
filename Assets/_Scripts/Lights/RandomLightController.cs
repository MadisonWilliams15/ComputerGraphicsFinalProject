using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLightController : AbstractLightController
{
    public int maxGreenTime = 20;
    public int yellowTime = 4;
    private int lightRemaining;

    void Start()
    {
        lightRemaining = Random.Range(0, maxGreenTime + 1);
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
                lightRemaining = Random.Range(0, maxGreenTime + 1);
                SetLightColor(LightColor.Green);
                lightDirection = LightDirection.CrossNorthSouth == lightDirection ? LightDirection.CrossEastWest : LightDirection.CrossNorthSouth;//Lights oscillate between alternating crosses
            }
        }//Cycle from green to yellow and around directions
    }
}
