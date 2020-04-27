using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class OneIntersectionAcademyDuplicate : Academy
{
    /*public int maxSpawnRate;
    public int minSpawnRate;
    private int spawnDiff;
    public int maxExitRate;
    public int minExitRate;
    private int exitDiff;
    public int maxSteps;
    private int step = 0;
    private TwoLanePathGenerator twoLanePathGenerator;
    private List<SpawnRoad> spawnRoads;
    private List<ExitRoad> exitRoads;
    private OneIntersectionController agent;


    public override void InitializeAcademy()
    {
        twoLanePathGenerator = FindObjectOfType<TwoLanePathGenerator>();
        spawnRoads = new List<SpawnRoad>(FindObjectsOfType<SpawnRoad>());
        exitRoads = new List<ExitRoad>(FindObjectsOfType<ExitRoad>());
        spawnDiff = maxSpawnRate - minSpawnRate;
        spawnDiff = maxExitRate - minExitRate;
        agent = FindObjectOfType<OneIntersectionController>();
    }

    public override void AcademyReset()
    {
        Dictionary<int, float> dic = SimSettings.weightSpawnRateDict;
        for (int i = 0; i <= spawnRoads.Count; i++)
        {
            dic[i] = Random.value * spawnDiff + minSpawnRate;
        }
        for (int i = spawnRoads.Count + 1; i <= spawnRoads.Count + exitRoads.Count; i++)
        {
            dic[i] = Random.value * exitDiff + minExitRate;
        }
        SimSettings.weightSpawnRateDict = dic;

        foreach (SpawnRoad sr in spawnRoads)
        {
            sr.resetSpawnRoad();
        }
        foreach (ExitRoad er in exitRoads)
        {
            er.resetExitRoad();
        }

        List<Car> cars = new List<Car>(FindObjectsOfType<Car>());
        foreach (Car c in cars)
        {
            c.endTime = Time.frameCount;
            Statistics.SetCarStatictics(c.endTime - c.startTime, c.distanceTraveled);
            Destroy(c.gameObject);
        }
        float r = (float)Statistics.getAverageDistanceTimeRatio();
        Statistics.ResetCarStatistics();

        twoLanePathGenerator.loadWeightedExitRoads();
        foreach (SpawnRoad sr in spawnRoads)
        {
            sr.startSpawnRoad();
        }
        agent.setReward(r);
        step = 0;
    }

    public override void AcademyStep()
    {
        step++;
        if (step >= maxSteps)
        {
            AcademyReset();
        }
        base.AcademyStep();
    }*/
}
