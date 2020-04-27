using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionStatistics : MonoBehaviour
{
    public List<double> carCounts; //count of cars in an intersection at a specific time
    public List<double> waitTimes; //amount of time each car took to get through stoplight 
    public int throughput; //throughput for each stoplight/intersection

    public IntersectionStatistics()//constructor
    {
        carCounts = new List<double>();
        waitTimes = new List<double>();
        throughput = 0;
    }

    public double getAverageCarCount()
    {
        if (this.carCounts.Count == 0)
        {
            return 0;
        }
        double avgCarCount = 0;
        foreach (int count in this.carCounts)
        {
            avgCarCount += count;
        }
        avgCarCount /= this.carCounts.Count;
        return System.Math.Round(avgCarCount, 4);
    }//get the average number of cars that are in a given intersection

    public double getAverageWaitTimes()
    {
        if (this.waitTimes.Count == 0)
        {
            return 0;
        }
        double avgWaitTimes = 0;
        foreach (int time in this.waitTimes)
        {
            avgWaitTimes += time;
        }
        avgWaitTimes /= this.waitTimes.Count;
        return System.Math.Round(avgWaitTimes, 4);
    }//get the average time that it takes cars to get through an intersection

}
