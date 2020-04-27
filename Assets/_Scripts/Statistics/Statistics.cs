using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    public static ArrayList travelTimes = new ArrayList();//list of time it took each car to get from spawn point to destiantion
    public static ArrayList stopRatios = new ArrayList();//list of how long each car spent stopped
    public static ArrayList distanceTimeRatios = new ArrayList();//list of each car's distance traveled / time ratio
    public static List<JsonData.Run> runList = new List<JsonData.Run>();

    private static JsonData jsonData = new JsonData();


    //called every time a car exits the scene/reaches destination before it is destroyed
    public static void SetCarStatictics(int travelTime, int distanceTraveled)
    {
        if (travelTime != 0)
        {
            travelTimes.Add(travelTime);
            stopRatios.Add((double)(travelTime - distanceTraveled) / travelTime);
            distanceTimeRatios.Add((double)distanceTraveled / travelTime);
        }
        //PrintTrafficStatistics(); //can uncomment this to print statistics gathered to the console every time a car exits the scene
    }

    public static void ResetCarStatistics()
    {
        travelTimes.Clear();
        stopRatios.Clear();
        distanceTimeRatios.Clear();
    }

    public static void AddCarStatistics(int runNumber)
    {
        JsonData.Run run = new JsonData.Run();
        run.runNumber = runNumber;
        run.stats.averageTravelTime = getAverageTravelTime();
        run.stats.averageStopTime = getAverageStopTime();
        run.stats.averageDistanceTimeRatio = getAverageDistanceTimeRatio();
        runList.Add(run);
    }

    public static double getAverageTravelTime()
    {
        if (travelTimes.Count==0)
        {
            return 0;
        }
        double avgTravelTime = 0;
        foreach (int time in travelTimes)
        {
            avgTravelTime += time;
        }
        avgTravelTime /= travelTimes.Count;
        return Math.Round(avgTravelTime, 4);
    }//get the average amount of time that is taking cars to reach their destinations

    public static double getAverageStopTime()
    {
        if (stopRatios.Count == 0)
        {
            return 0;
        }
        double avgStopRatio = 0;
        foreach (double time in stopRatios)
        {
            avgStopRatio += time;
        }
        avgStopRatio /= stopRatios.Count;
        return Math.Round(avgStopRatio * 100, 4);
    }//get the average percentage of time that cars are stopped at a red light

    public static double getAverageDistanceTimeRatio()
    {
        if (distanceTimeRatios.Count == 0)
        {
            return 0;
        }
        double avgRatio = 0;
        foreach (double time in distanceTimeRatios)
        {
            avgRatio += time;
        }
        avgRatio /= distanceTimeRatios.Count;
        return Math.Round(avgRatio * 100, 4);
    }//get the average ratio of how far cars traveled / how long it took the car to reach destination
    
    public static List<double> getAverageOfRuns()
    {
        //TODO change; rough mock up to see if it works probably can be done more dynamically
        //instiante with all values at 0
        List<double> averageOfRuns = new List<double>()
        {
            0.0,0.0,0.0
        };
        
        foreach (JsonData.Run run in runList)
        {
            averageOfRuns[0] += run.stats.averageTravelTime;
            averageOfRuns[1] += run.stats.averageStopTime;
            averageOfRuns[2] += run.stats.averageDistanceTimeRatio;
        }

        //average all 3 values
        averageOfRuns[0] = Math.Round(averageOfRuns[0] / runList.Count, 4);
        averageOfRuns[1] = Math.Round(averageOfRuns[1] / runList.Count, 4);
        averageOfRuns[2] = Math.Round(averageOfRuns[2] / runList.Count, 4);

        return averageOfRuns;
    }

    public static void PrintTrafficStatistics()
    {
        print("Frame # " + Time.frameCount + " Average Travel Time: " + getAverageTravelTime() + " Average Stop Ratio: " + getAverageStopTime() + " Average Distance Time Ratio: " + getAverageDistanceTimeRatio());

    }//A temporary way to display statistics, We will eventually need to display this to the user 

    public static JsonData getJsonData()
    {
        jsonData.runList = runList;
        return jsonData;
    }

}
