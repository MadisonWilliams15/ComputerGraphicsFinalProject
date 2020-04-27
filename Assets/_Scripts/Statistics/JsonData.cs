using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//used for seralizing into json
[Serializable]
public class JsonData
{
    //add more data here that could be saved to a file
    public string simName;
    public List<Run> runList = new List<Run>();
    public Customization customization = new Customization();

    [Serializable]
    public class Run
    {
        public int runNumber;
        public Stats stats = new Stats();
    }
    [Serializable]
    public class Stats
    {
        public double averageTravelTime;
        public double averageStopTime;
        public double averageDistanceTimeRatio;
    }

    [Serializable]
    public class Customization
    {
        

    }

}