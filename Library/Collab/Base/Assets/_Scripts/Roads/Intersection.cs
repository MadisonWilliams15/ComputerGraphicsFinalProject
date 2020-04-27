using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Light {Green, Yellow};

public class Intersection : MonoBehaviour
{
    public Road incomingNorthRoad, incomingEastRoad, incomingSouthRoad, incomingWestRoad, outNorthRoad, outEastRoad, outSouthRoad, outWestRoad;

    public Car[,] occupants = new Car[2, 2];//0,0 is nw corner, 0,1 is ne, 1,0 is sw, 1,1 is se corner

    private iLightController lightController;

    private void Start()
    {
        //adds user choosen lightcontroller
        GameObject intersection = this.gameObject;
        if (PlayerPrefs.GetString("light") == "TimedLightController")
        {
            intersection.AddComponent<TimedLightController>();
        }
        
        lightController = GetComponent<iLightController>();
        if(null == lightController)
        {
            Debug.LogError(transform.name + " has no lightcontroller");
            Destroy(this);
        }//Gets light controller or errors
    }

    void Update()
    {
        
        if (null != occupants[0,0] && Direction.West == occupants[0,0].path.Peek() && TestRoad(outWestRoad) && Time.frameCount != occupants[0,0].frameMoved)
        {
            occupants[0, 0].frameMoved = Time.frameCount;
            outWestRoad.occupants[outWestRoad.occupants.Length - 1] = occupants[0,0];
            occupants[0, 0].distanceTraveled += outWestRoad.occupants.Length;//STATS
            occupants[0, 0].transform.position = new Vector3(occupants[0, 0].transform.position.x - 1, occupants[0, 0].transform.position.y, occupants[0, 0].transform.position.z);
            occupants[0, 0].path.Dequeue();
            occupants[0, 0] = null;
        }//If in nw slot, heading west, and road has an open slot
        if (null != occupants[0, 0] && (Direction.East == occupants[0, 0].path.Peek() || Direction.South == occupants[0, 0].path.Peek()) && null == occupants[1, 0] && Time.frameCount != occupants[0, 0].frameMoved)
        {
            occupants[1, 0] = occupants[0, 0];
            occupants[0, 0].distanceTraveled++;//STATS
            occupants[0, 0].transform.position = new Vector3(occupants[0, 0].transform.position.x, occupants[0, 0].transform.position.y - 1, occupants[0, 0].transform.position.z);
            occupants[0, 0] = null;
            occupants[1, 0].frameMoved = Time.frameCount;
        }//If in nw slot, heading east or south, and sw corner has an open slot
        if (null != occupants[0, 0] && Direction.North == occupants[0, 0].path.Peek() && null == occupants[0, 1] && Time.frameCount != occupants[0, 0].frameMoved)
        {
            occupants[0, 1] = occupants[0, 0];
            occupants[0, 0].distanceTraveled++;//STATS
            occupants[0, 0].transform.position = new Vector3(occupants[0, 0].transform.position.x + 1, occupants[0, 0].transform.position.y, occupants[0, 0].transform.position.z);
            occupants[0, 0] = null;
            occupants[0, 1].frameMoved = Time.frameCount;
        }//If in nw slot, heading north, and ne corner has an open slot  --- U-turn


        if (null != occupants[0, 1] && Direction.North == occupants[0, 1].path.Peek() && TestRoad(outNorthRoad) && Time.frameCount != occupants[0, 1].frameMoved)
        {
            occupants[0, 1].frameMoved = Time.frameCount;
            outNorthRoad.occupants[outNorthRoad.occupants.Length - 1] = occupants[0, 1];
            occupants[0, 1].distanceTraveled += outNorthRoad.occupants.Length;//STATS
            occupants[0, 1].transform.position = new Vector3(occupants[0, 1].transform.position.x, occupants[0, 1].transform.position.y + 1, occupants[0, 1].transform.position.z);
            occupants[0, 1].path.Dequeue();
            occupants[0, 1] = null;
        }//If in ne slot, heading north, and road has an open slot
        if (null != occupants[0, 1] && (Direction.West == occupants[0, 1].path.Peek() || Direction.South == occupants[0, 1].path.Peek()) && null == occupants[0, 0] && Time.frameCount != occupants[0, 1].frameMoved)
        {
            occupants[0, 0] = occupants[0, 1];
            occupants[0, 1].distanceTraveled++; //STATS
            occupants[0, 1].transform.position = new Vector3(occupants[0, 1].transform.position.x - 1, occupants[0, 1].transform.position.y, occupants[0, 1].transform.position.z);
            occupants[0, 1] = null;
            occupants[0, 0].frameMoved = Time.frameCount;
        }//If in ne slot, heading west or south, and nw corner has an open slot
        if (null != occupants[0, 1] && Direction.East == occupants[0, 1].path.Peek() && null == occupants[1, 1] && Time.frameCount != occupants[0, 1].frameMoved)
        {
            occupants[1, 1] = occupants[0, 1];
            occupants[0, 1].distanceTraveled++;//STATS
            occupants[0, 1].transform.position = new Vector3(occupants[0, 1].transform.position.x, occupants[0, 1].transform.position.y - 1, occupants[0, 1].transform.position.z);
            occupants[0, 1] = null;
            occupants[1, 1].frameMoved = Time.frameCount;
        }//If in ne slot, heading east, and se corner has an open slot  --- U-turn


        if (null != occupants[1, 0] && Direction.South == occupants[1, 0].path.Peek() && TestRoad(outSouthRoad) && Time.frameCount != occupants[1, 0].frameMoved)
        {
            occupants[1, 0].frameMoved = Time.frameCount;
            outSouthRoad.occupants[outSouthRoad.occupants.Length - 1] = occupants[1, 0];
            occupants[1, 0].distanceTraveled += outSouthRoad.occupants.Length;//STATS
            occupants[1, 0].transform.position = new Vector3(occupants[1, 0].transform.position.x, occupants[1, 0].transform.position.y - 1, occupants[1, 0].transform.position.z);
            occupants[1, 0].path.Dequeue();
            occupants[1, 0] = null;
        }//If in sw slot, heading south, and road has an open slot
        if (null != occupants[1, 0] && (Direction.East == occupants[1, 0].path.Peek() || Direction.North == occupants[1, 0].path.Peek()) && null == occupants[1, 1] && Time.frameCount != occupants[1, 0].frameMoved)
        {
            occupants[1, 1] = occupants[1, 0];
            occupants[1, 0].distanceTraveled++;//STATS
            occupants[1, 0].transform.position = new Vector3(occupants[1, 0].transform.position.x + 1, occupants[1, 0].transform.position.y, occupants[1, 0].transform.position.z);
            occupants[1, 0] = null;
            occupants[1, 1].frameMoved = Time.frameCount;
        }//If in sw slot, heading east or north, and se corner has an open slot
        if (null != occupants[1, 0] && Direction.West == occupants[1, 0].path.Peek() && null == occupants[0, 0] && Time.frameCount != occupants[1, 0].frameMoved)
        {
            occupants[0, 0] = occupants[1, 0];
            occupants[1, 0].distanceTraveled++;//STATS
            occupants[1, 0].transform.position = new Vector3(occupants[1, 0].transform.position.x, occupants[1, 0].transform.position.y + 1, occupants[1, 0].transform.position.z);
            occupants[1, 0] = null;
            occupants[0, 0].frameMoved = Time.frameCount;
        }//If in sw slot, heading west, and nw corner has an open slot  --- U-turn


        if (null != occupants[1, 1] && Direction.East == occupants[1, 1].path.Peek() && TestRoad(outEastRoad) && Time.frameCount != occupants[1, 1].frameMoved)
        {
            occupants[1, 1].frameMoved = Time.frameCount;
            outEastRoad.occupants[outEastRoad.occupants.Length - 1] = occupants[1, 1];
            occupants[1, 1].distanceTraveled += outEastRoad.occupants.Length;//STATS
            occupants[1, 1].transform.position = new Vector3(occupants[1, 1].transform.position.x + 1, occupants[1, 1].transform.position.y, occupants[1, 1].transform.position.z);
            occupants[1, 1].path.Dequeue();
            occupants[1, 1] = null;
        }//If in se slot, heading east, and road has an open slot
        if (null != occupants[1, 1] && (Direction.West == occupants[1, 1].path.Peek() || Direction.North == occupants[1, 1].path.Peek()) && null == occupants[0, 1] && Time.frameCount != occupants[1, 1].frameMoved)
        {
            occupants[0, 1] = occupants[1, 1];
            occupants[1, 1].distanceTraveled++;//STATS
            occupants[1, 1].transform.position = new Vector3(occupants[1, 1].transform.position.x, occupants[1, 1].transform.position.y + 1, occupants[1, 1].transform.position.z);
            occupants[1, 1] = null;
            occupants[0, 1].frameMoved = Time.frameCount;
        }//If in se slot, heading west or north, and ne corner has an open slot
        if (null != occupants[1, 1] && Direction.South == occupants[1, 1].path.Peek() && null == occupants[1, 0] && Time.frameCount != occupants[1, 1].frameMoved)
        {
            occupants[1, 0] = occupants[1, 1];
            occupants[1, 1].distanceTraveled++;//STATS
            occupants[1, 1].transform.position = new Vector3(occupants[1, 1].transform.position.x - 1, occupants[1, 1].transform.position.y, occupants[1, 1].transform.position.z);
            occupants[1, 1] = null;
            occupants[1, 0].frameMoved = Time.frameCount;
        }//If in se slot, heading south, and sw corner has an open slot  --- U-turn


        for (int x = 0; 2 > x; x++)
        {
            for (int y = 0; 2 > y; y++)
            {
                if (null != occupants[x, y])
                {
                    occupants[x, y].frameMoved = null;
                }
            }
        }//Reset all hasMoved to false before next iteration



        if(null == occupants[1,1] && ((Direction.North == lightController.LightDirection && Light.Green == lightController.LightColor) || lightController.GetType() == typeof(Turn)))
        {
            occupants[1,1] = incomingNorthRoad?.advance();
            
        }
        else
        {
            incomingNorthRoad?.Fill();
        }//Move into intersection if empty, else backfill road
        if (null == occupants[1, 0] && ((Direction.East == lightController.LightDirection && Light.Green == lightController.LightColor) || lightController.GetType() == typeof(Turn)))
        {
            occupants[1, 0] = incomingEastRoad?.advance();
            
        }
        else
        {
            incomingEastRoad?.Fill();
        }//Move into intersection if empty, else backfill road
        if (null == occupants[0,1] && ((Direction.West == lightController.LightDirection && Light.Green == lightController.LightColor) || lightController.GetType() == typeof(Turn)))
        {
            
            occupants[0, 1] = incomingWestRoad?.advance();
            
        }
        else
        {
            incomingWestRoad?.Fill();
        }//Move into intersection if empty, else backfill road
        if (null == occupants[0, 0] && ((Direction.South == lightController.LightDirection && Light.Green == lightController.LightColor) || lightController.GetType() == typeof(Turn)))
        {
            occupants[0, 0] = incomingSouthRoad?.advance();
            
        }
        else
        {
            incomingSouthRoad?.Fill();
        }//Move into intersection if empty, else backfill road

       
    }

    bool TestRoad(Road road)
    {
        if(null != road?.occupants && 0 != road.occupants.Length && null == road.occupants[road.occupants.Length - 1])
        {
            return true;
        }//If road exists and has space for cars
        return false;
    }
}
