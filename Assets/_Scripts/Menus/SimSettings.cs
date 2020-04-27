using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimSettings : MonoBehaviour
{

    public Text simText;
    public string simString;
    public Dropdown lightDropdown;
    public Toggle loopToggle;
    public InputField runsInput;
    public InputField runLengthInput;
    public List<GameObject> entryList = new List<GameObject>();
    public GameObject entry;
    public GameObject exitEntry;
    public ScrollRect table;
   

    //using dictionary of dictionary so I can pass a static one amongst classes
    public static Dictionary<int,float> weightSpawnRateDict { get; set; }

    static SimSettings()
    {
        weightSpawnRateDict = new Dictionary<int, float>();
    }

    // Start is called before the first frame update
    void Awake()
    {
        var test = FindObjectsOfType<SimSettings>();

        simString = PlayerPrefs.GetString("sim_name");
        if (simText != null)
        {
            simText.text = simString;
        }

        if (true == PlayerPrefs.HasKey("light") && null != lightDropdown)
        {
            List<Dropdown.OptionData> dropDownChoices = lightDropdown.GetComponent<Dropdown>().options;
            for (int i = 0; dropDownChoices.Count > i; i = i + 1)
            {
                string lightController = PlayerPrefs.GetString("light");
                if (dropDownChoices[i].text == lightController)
                {
                    lightDropdown.GetComponent<Dropdown>().value = i;
                    break;
                }
            }
        }//Loads existing light option to avoid having to select dropdown every time



        //initalize to prevent error
        PlayerPrefs.SetString("loop", "off");

        GetActionRoads();
        PlayerPrefs.SetInt("id", 0);
        PlayerPrefs.SetInt("exitid", 0);
    }

    public static void AddSpawnRates()
    {
        var entries = GameObject.FindGameObjectsWithTag("Finish");
        foreach (var entry in entries)
        {
            var text = entry.GetComponentInChildren<Text>();
            int id = 0;
            Int32.TryParse(text.text, out id);
            var slider = entry.GetComponentInChildren<Slider>();
            float rate = slider.value;
            if (weightSpawnRateDict.ContainsKey(id) == true)
            {
                weightSpawnRateDict[id] = rate;
            }
            else
            {
                weightSpawnRateDict.Add(id, rate);
            }
            //print(id + "  " + weightSpawnRateDict[id]);
        }
    }

    public void AddLightController()
    {
        int selectedIndex = lightDropdown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> dropDownChoices = lightDropdown.GetComponent<Dropdown>().options;
        string selectedLight = dropDownChoices[selectedIndex].text;

        PlayerPrefs.SetString("light", selectedLight);
    }

public void loopToggleEvent()
    {
        if (loopToggle.isOn == true)
        {
            runsInput.interactable = true;
            runLengthInput.interactable = true;
            PlayerPrefs.SetString("loop", "on");
        }
        else
        {
            runsInput.interactable = false;
            runsInput.text = "3";
            runLengthInput.interactable = false;
            runLengthInput.text = "45";
            PlayerPrefs.SetString("loop", "off");
        }
    }

    public void addMultipleRuns()
    {
        int numRuns;
        int runLength;
        if (runsInput.text.Length == 0 || !int.TryParse(runsInput.text, out numRuns) || numRuns < 1)
        {
            numRuns = 3;
        }
        if (runLengthInput.text.Length == 0 || !int.TryParse(runLengthInput.text, out runLength) || runLength < 1)
        {
            runLength = 45;
        }
        PlayerPrefs.SetString("numRuns", numRuns.ToString());
        PlayerPrefs.SetString("runLength", runLength.ToString());
    }

    public static Dictionary<int, float> getDictionary()
    {
        return weightSpawnRateDict; 
    }

    void GetActionRoads()
    {
        //Item1 = SpawnRoads Item2=ExitRoads
        string simname = PlayerPrefs.GetString("sim_name");
        Tuple<int, int> simRoads = null; //var simRoads; = new Tuple<int, int>();
        switch (simname)
        {
            case "1Intersection-2Lanes":
                simRoads = new Tuple<int, int>(4,4);
                break;
            case "4Intersection-2Lane":
                simRoads = new Tuple<int, int>(8, 8);
                break;
            case "SmallTwoLaneGrid":
                simRoads = new Tuple<int, int>(7, 7);
                break;
            case "LargeTwoLaneGrid":
                simRoads = new Tuple<int, int>(14, 14);
                break;
            default:
                Debug.Log("Table not hardcoded for given scene: " + simname);
                return;
        }



        for (int i = 1 ; i <= simRoads.Item1; i++)
        {
            var clone = Instantiate(entry, table.content, false);
            var textComponents = clone.GetComponentsInChildren<Text>();
            textComponents[0].text = i.ToString();
            textComponents[1].text = "SpawnRoad";
        }

        for (int i = simRoads.Item1 + 1; i <= simRoads.Item1 + simRoads.Item2; i++) //lazy way to have it auto sorted
        {
            var clone = Instantiate(exitEntry, table.content, false);
            var textComponents = clone.GetComponentsInChildren<Text>();
            textComponents[0].text = i.ToString();
            textComponents[1].text = "ExitRoad";
        }
    }

}
