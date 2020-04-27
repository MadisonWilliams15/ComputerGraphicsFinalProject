using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GreedyLightController : AbstractLightController
{
    public int greenTime = 5;
    public int yellowTime = 4;
    private int lightRemaining;
    private LightDirection nextDirection;

    void Start()
    {
        lightRemaining = greenTime;
        nextDirection = lightDirection;
    }

    //Greedy algorithm choose either crossNorthSouth or crossEastWest every x ticks. If that direction has the most cars in queue, then the light does not change, else it changes to the opposite direction
    void Update()
    {
        lightRemaining--;
        if (0 >= lightRemaining)
        {
            if (LightColor.Green == lightColor)
            {
                Nullable<int> northStraightOccupants = intersection?.inboundNorthRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> northLeftOccupants = intersection?.inboundNorthRoad?.leftRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> eastStraightOccupants = intersection?.inboundEastRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> eastLeftOccupants = intersection?.inboundEastRoad?.leftRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> southStraightOccupants = intersection?.inboundSouthRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> southLeftOccupants = intersection?.inboundSouthRoad?.leftRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> westStraightOccupants = intersection?.inboundWestRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
                Nullable<int> westLeftOccupants = intersection?.inboundWestRoad?.leftRoadScript.occupants.Where(car => null != car).Count();//Get cars in each lane
                int northSouthLanes = (null == intersection?.inboundNorthRoad ? 0 : 1) + (null == intersection?.inboundSouthRoad ? 0 : 1);
                int eastWestLanes = (null == intersection?.inboundEastRoad ? 0 : 1) + (null == intersection?.inboundWestRoad ? 0 : 1);//Get number of lanes
                int northSouthOccupants = (northStraightOccupants ?? 0) + (northLeftOccupants ?? 0) + (southStraightOccupants ?? 0) + (southLeftOccupants ?? 0);
                int eastWestOccupants = (eastStraightOccupants ?? 0) + (eastLeftOccupants ?? 0) + (westStraightOccupants ?? 0) + (westLeftOccupants ?? 0);//Add number of cars in each cross amount
                northSouthOccupants *= (1 == northSouthLanes ? 2 : 1);
                eastWestOccupants *= (1 == eastWestLanes ? 2 : 1);//Multiply by 2 if only 1 lane present so values can be comparable
                if(northSouthOccupants > eastWestOccupants)
                {
                    nextDirection = LightDirection.CrossNorthSouth;
                }
                else if(eastWestOccupants > northSouthOccupants)
                {
                    nextDirection = LightDirection.CrossEastWest;
                }//Next direction is direction with most cars
            }

            if(LightColor.Yellow == lightColor)
            {
                SetLightColor(LightColor.Green);
                lightRemaining = greenTime;
                lightDirection = nextDirection;
            }//If yellow, switch the next direction
            else if(lightDirection == nextDirection)
            {
                lightRemaining = greenTime;
            }//If green, but not changing continue
            else
            {
                SetLightColor(LightColor.Yellow);
                lightRemaining = yellowTime;
            }//If green and changing, switch to yellow
        }
    }
}
