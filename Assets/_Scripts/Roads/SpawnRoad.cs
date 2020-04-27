using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoad : Road
{
    //make this into an array and then randomly pick one.
    //could just make all of the sprites into a prefab
    //sprite renderer, car script
    public static Sprite[] carSprites = new Sprite[7];
    private TwoLanePathGenerator twoLanePathGenerator;
    private Vector3 carDirection;
    public int id = 1;
    public float spawnRate = 1;

    void Awake()
    {
        for (int i = 0; 6 > i; i = i + 1)
        {
            carSprites[i] = Resources.Load<Sprite>("Sprites/Cars/car_people" + (i + 1));
        }//Loads car sprites
        //reset player pref so ids are true for playerprefs
        if (PlayerPrefs.HasKey("id"))
        {
            id = PlayerPrefs.GetInt("id") + 1;
            PlayerPrefs.SetInt("id", id);
        }
        else
        {
            PlayerPrefs.SetInt("id", id);
        }
    }
    void Start()
    {
        twoLanePathGenerator = FindObjectOfType<TwoLanePathGenerator>();
        if (direction == LightDirection.North) carDirection = new Vector3(0, 0, 0);
        if (direction == LightDirection.East) carDirection = new Vector3(0, 0, 270);
        if (direction == LightDirection.South) carDirection = new Vector3(0, 0, 180);
        if (direction == LightDirection.West) carDirection = new Vector3(0, 0, 90);


        //create unique ID for spawn road so weighted rate can get assigned
        

        
        //setup generation for cars
        //On timer call Spawn()
        Dictionary<int, float> weightedSpawnRatesDict = SimSettings.getDictionary();
        float weight;

        if (weightedSpawnRatesDict.TryGetValue(id, out weight))
        {
            spawnRate = weight / Application.targetFrameRate;
        }
        else
        {
            spawnRate = 10.0f / Application.targetFrameRate;
        }

        if (PlayerPrefs.GetString("sim_name") == "LargeTwoLaneGrid")
        {
            print("large two lane");
            spawnRate *= 2;
        }

        InvokeRepeating("Spawn", 0.0f, spawnRate);
    }

    public int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);
    }

    public void Spawn()
    {
        //make prefab for next 5 lines.
        //create new car
        GameObject newCar;
        newCar = new GameObject("Car");
        //newCar.transform.sca
        //Add sprite
        SpriteRenderer sr = newCar.AddComponent<SpriteRenderer>();
        sr.sprite = carSprites[twoLanePathGenerator.RandomNumber(0, 6)];
        
        //Add script
        newCar.AddComponent<Car>();

        //Set initial rotation
        //Set position based on direction of current road
        switch (direction)
        {
            case LightDirection.West:
                newCar.transform.position = new Vector3(transform.lossyScale.x / 2.0f + transform.position.x - .5f, transform.position.y, -1.0f);
                break;
            case LightDirection.East:
                newCar.transform.position = new Vector3(transform.lossyScale.x / -2.0f + transform.position.x + .5f, transform.position.y, -1.0f);
                break;
            case LightDirection.North:
                newCar.transform.position = new Vector3(transform.position.x, transform.lossyScale.x / -2.0f + transform.position.y + .5f, -1.0f);
                break;
            case LightDirection.South:
                newCar.transform.position = new Vector3(transform.position.x, transform.lossyScale.x / 2.0f + transform.position.y - .5f, -1.0f);
                break;
        }
        newCar.transform.eulerAngles = carDirection;//Set initial rotation

        //Set entrypoint road reference
        Car carObject = newCar.GetComponent<Car>();
        carObject.entryPoint = this;
        carObject.startTime = Time.frameCount;
        carObject.distanceTraveled += occupants.Length;
        //Call pathgenertor
        carObject.path = twoLanePathGenerator.GetPath(this);
        //Put onto road array
        occupants[occupants.Length - 1] = carObject;
    }

    public void resetSpawnRoad(float weight)
    {
        CancelInvoke("Spawn");
        spawnRate = weight;
    }

    public void startSpawnRoad()
    {
        InvokeRepeating("Spawn", 0.0f, spawnRate);
    }
}
