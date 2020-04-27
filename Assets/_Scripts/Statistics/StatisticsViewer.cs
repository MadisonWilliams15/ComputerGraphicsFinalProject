using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StatisticsViewer : MonoBehaviour
{
    //ui fields
    public Text att;
    public Text ast;
    public Text adt;
    public Text runs;
    
    void Start()
    {
      
        runs.text = "Runs: " + Statistics.runList.Count.ToString();

        if (Statistics.runList.Count == 1)
        {
            att.text += Statistics.runList[0].stats.averageTravelTime.ToString() + " frames";
            ast.text += Statistics.runList[0].stats.averageStopTime.ToString() + "%";
            adt.text += Statistics.runList[0].stats.averageDistanceTimeRatio.ToString() + "%";
        }
        else
        {
            //0 = att ; 1 = ast ; 2 = adt
            List<double> averageOfRuns = Statistics.getAverageOfRuns();

            att.text += averageOfRuns[0].ToString() + " frames";
            ast.text += averageOfRuns[1].ToString() + "%";
            adt.text += averageOfRuns[2].ToString() + "%";
        }
    }

    public void SeralizeData()
    {

        JsonData jsonData = Statistics.getJsonData();
        jsonData.simName = PlayerPrefs.GetString("sim_name");
        string json = JsonUtility.ToJson(jsonData);
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        Directory.CreateDirectory(Application.persistentDataPath + "/SavedData/");
        string path = Application.persistentDataPath + "/SavedData/" + timestamp + ".json";
        print("This is the path JSON file was saved at -> " + path);

        if (path.Length != 0)
        {
            if (json != null)
                File.WriteAllText(path, json);
        }
        
    }
}
