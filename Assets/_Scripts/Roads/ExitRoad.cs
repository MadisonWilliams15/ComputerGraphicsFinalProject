using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoad : Road
{
    public int exitRate = 2;
    public int id = 1;
    public int numSpawnRoads = 0;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("exitid"))
        {
            id = PlayerPrefs.GetInt("exitid") + 1;
            PlayerPrefs.SetInt("exitid", id);
        }
        else
        {
            PlayerPrefs.SetInt("exitid", id);
        }
        numSpawnRoads = GameObject.FindGameObjectsWithTag("Respawn").Length;

        Dictionary<int, float> weightedSpawnRatesDict = SimSettings.getDictionary();
        float weight;

        if (weightedSpawnRatesDict.TryGetValue(id + numSpawnRoads, out weight))
        {
            exitRate = (int)weight;
        }
        else
        {
            exitRate = 1;
        }
    }

    void Update()
    {
        Car car = advance();
        if (null != car)
        {
            car.endTime = Time.frameCount;
            Statistics.SetCarStatictics(car.endTime - car.startTime, car.distanceTraveled);
            Destroy(car.gameObject);
        }
    }

    public void resetExitRoad(float weight)
    {
        exitRate = (int)weight;
    }
}
