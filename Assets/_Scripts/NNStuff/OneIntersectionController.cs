using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MLAgents;

public class OneIntersectionController : Agent, iLightController
{
    public LightDirection lightDirection = LightDirection.CrossNorthSouth;
    public LightColor lightColor = LightColor.Green;
    public IntersectionTwoLane intersection;
    public int frameChanged = 0;//Updated when lightColor set
    public int yellowTime = 4;
    private int YellowLightRemaining;
    private LightDirection nextDirection;
    private float reward = 0;
    public int maxSpawnRate;
    public int minSpawnRate;
    private int spawnDiff;
    public int maxExitRate;
    public int minExitRate;
    private int exitDiff;
    private TwoLanePathGenerator twoLanePathGenerator;
    private List<SpawnRoad> spawnRoads;
    private List<ExitRoad> exitRoads;
    private OneIntersectionController agent;

    public LightDirection GetLightDirection()
    {
        return lightDirection;
    }

    public void SetLightDirection(LightDirection d)
    {
        lightDirection = d;
    }

    public LightColor GetLightColor()
    {
        return lightColor;
    }

    public void SetLightColor(LightColor c)
    {
        lightColor = c;
        if (c != LightColor.Yellow)
        {
            frameChanged = Time.frameCount;
        }
    }

    public int GetFrameChanged()
    {
        return frameChanged;
    }

    public void Start()
    {
        intersection = transform.GetComponent<IntersectionTwoLane>();
        twoLanePathGenerator = FindObjectOfType<TwoLanePathGenerator>();
        spawnRoads = new List<SpawnRoad>(FindObjectsOfType<SpawnRoad>());
        exitRoads = new List<ExitRoad>(FindObjectsOfType<ExitRoad>());
        spawnDiff = maxSpawnRate - minSpawnRate;
        exitDiff = maxExitRate - minExitRate;
        agent = FindObjectOfType<OneIntersectionController>();
    }

    public override void AgentReset()
    {
        /*foreach (SpawnRoad sr in spawnRoads)
        {
            sr.resetSpawnRoad(UnityEngine.Random.value * spawnDiff + minSpawnRate);
        }
        foreach (ExitRoad er in exitRoads)
        {
            er.resetExitRoad(UnityEngine.Random.value * exitDiff + minExitRate);
        }

        List<Car> cars = new List<Car>(FindObjectsOfType<Car>());
        foreach (Car c in cars)
        {
            c.endTime = Time.frameCount;
            Statistics.SetCarStatictics(-1, 1);
            Destroy(c.gameObject);
        }
        float r = (float)Statistics.getAverageDistanceTimeRatio();
        r /= 100;
        Statistics.ResetCarStatistics();
        SetReward(r);

        twoLanePathGenerator.loadWeightedExitRoads();
        foreach (SpawnRoad sr in spawnRoads)
        {
            sr.startSpawnRoad();
        };
        lightColor = LightColor.Green;
        lightDirection = LightDirection.CrossNorthSouth;
        frameChanged = 0;*/
    }

    public override void CollectObservations()
    {
        int? northStraightOccupants = intersection?.inboundNorthRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
        int? northLeftOccupants = intersection?.inboundNorthRoad?.leftRoadScript.occupants.Where(car => null != car).Count();
        int? eastStraightOccupants = intersection?.inboundEastRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
        int? eastLeftOccupants = intersection?.inboundEastRoad?.leftRoadScript.occupants.Where(car => null != car).Count();
        int? southStraightOccupants = intersection?.inboundSouthRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
        int? southLeftOccupants = intersection?.inboundSouthRoad?.leftRoadScript.occupants.Where(car => null != car).Count();
        int? westStraightOccupants = intersection?.inboundWestRoad?.straightRoadScript.occupants.Where(car => null != car).Count();
        int? westLeftOccupants = intersection?.inboundWestRoad?.leftRoadScript.occupants.Where(car => null != car).Count();//Get cars in each lane
        int northSouthLanes = (null == intersection?.inboundNorthRoad ? 0 : 1) + (null == intersection?.inboundSouthRoad ? 0 : 1);
        int eastWestLanes = (null == intersection?.inboundEastRoad ? 0 : 1) + (null == intersection?.inboundWestRoad ? 0 : 1);//Get number of lanes
        int northSouthOccupants = (northStraightOccupants ?? 0) + (northLeftOccupants ?? 0) + (southStraightOccupants ?? 0) + (southLeftOccupants ?? 0);
        int eastWestOccupants = (eastStraightOccupants ?? 0) + (eastLeftOccupants ?? 0) + (westStraightOccupants ?? 0) + (westLeftOccupants ?? 0);//Add number of cars in each cross amount
        northSouthOccupants *= (1 == northSouthLanes ? 2 : 1);
        eastWestOccupants *= (1 == eastWestLanes ? 2 : 1);

        AddVectorObs(northSouthOccupants);
        AddVectorObs(eastWestOccupants);
        //AddVectorObs(frameChanged);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        YellowLightRemaining--;
        if (YellowLightRemaining <= 0)
        {
            if (lightColor == LightColor.Green)
            {
                if (vectorAction[0] >= 0.5)
                {
                    nextDirection = LightDirection.CrossNorthSouth;
                }
                else
                {
                    nextDirection = LightDirection.CrossEastWest;
                }
            }
            if (lightColor == LightColor.Yellow)
            {
                SetLightColor(LightColor.Green);
                lightDirection = nextDirection;
            }//If yellow, switch the next direction
            else if (lightDirection != nextDirection)
            {
                SetLightColor(LightColor.Yellow);
                YellowLightRemaining = yellowTime;
            }
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[1] = Math.Abs(Input.GetAxis("Horizontal"));
        action[0] = Math.Abs(Input.GetAxis("Vertical"));
        return action;
    }
}
