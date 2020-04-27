using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    //Add all UI changes as unique functions to this script.
    //To access these functions as create an EMPTY GameObject and assign the script
    //From there add an action event to the button and select functionality
    public Dropdown sceneDropdown;
    public InputField fpsSettings;
    public int defaultFPS = 1;
    public static GameObject[] roads;
    public void Start()
    {
        if(true == PlayerPrefs.HasKey("fps") && null != fpsSettings)
        {
            fpsSettings.text = PlayerPrefs.GetInt("fps").ToString();//Sets current value in field
        }//Check if setting is currently set

        if(true == PlayerPrefs.HasKey("sim_name") && null != sceneDropdown)
        {
            List<Dropdown.OptionData> dropDownChoices = sceneDropdown.GetComponent<Dropdown>().options;
            for(int i = 0; dropDownChoices.Count > i; i = i + 1)
            {
                string sceneName = PlayerPrefs.GetString("sim_name");
                if (dropDownChoices[i].text == sceneName)
                {
                    sceneDropdown.GetComponent<Dropdown>().value = i;
                    break;
                }
            }
        }//Loads existing sim_name option to avoid having to select dropdown every time
    }

    public void PreviousScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            Statistics.runList.Clear();
            Statistics.ResetCarStatistics();
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void GenerateSimulation()
    {
        SetSettings();
        int selectedIndex = sceneDropdown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> dropDownChoices = sceneDropdown.GetComponent<Dropdown>().options;
        string selectedSceneText = dropDownChoices[selectedIndex].text;

        PlayerPrefs.SetString("sim_name", selectedSceneText);
        SceneManager.LoadScene("SimSettings");
    }

    public static GameObject[] getRoads()
    {
        return roads;
    }
    
    public void StartSimulation()
    {
        SimSettings.AddSpawnRates();
        SceneManager.LoadScene(PlayerPrefs.GetString("sim_name"));
    }

    public void SetSettings()
    {
        int fps;
        if (false == int.TryParse(fpsSettings.text,out fps))
        {
            if(true == PlayerPrefs.HasKey("fps"))
            {
                return;
            }//If setting exists, use it
            else
            {
                PlayerPrefs.SetInt("fps", defaultFPS);
                return;
            }//Else set default value
        }//If can't parse fps
        PlayerPrefs.SetInt("fps", fps);//Sets fps to user selected value
    }

    public void EndSimulation()
    {
        Statistics.AddCarStatistics(1);
        SceneManager.LoadScene(7);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
