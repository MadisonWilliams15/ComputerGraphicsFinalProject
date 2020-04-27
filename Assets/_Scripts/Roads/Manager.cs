using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private float RunTime = 5;
    private static int NumRuns = 3;
    private static int currentRun = 1;

    void Awake()
    {
        if (true == PlayerPrefs.HasKey("fps"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("fps");
        }//Set framerate based on setting
        else
        {
            Application.targetFrameRate = 1;//Set app to 1 fps
        }
        QualitySettings.vSyncCount = 0;
    }

    void Start()
    {
        if (PlayerPrefs.GetString("loop") == "on")
        {
            RunTime = int.Parse(PlayerPrefs.GetString("runLength"));
            NumRuns = int.Parse(PlayerPrefs.GetString("numRuns"));
            Invoke("RunSimAgain", RunTime);
        }
    }

    private void RunSimAgain()
    {
        Statistics.PrintTrafficStatistics();
        Statistics.AddCarStatistics(currentRun);
        Statistics.ResetCarStatistics();
        if (currentRun++ < NumRuns)
        {
            PlayerPrefs.SetInt("id", 0);
            PlayerPrefs.SetInt("exitid", 0);
            SceneManager.LoadScene(PlayerPrefs.GetString("sim_name"));
        }
        else
        {
            SceneManager.LoadScene(7);
        }
    }
}
