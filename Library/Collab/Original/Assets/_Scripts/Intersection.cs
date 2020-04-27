using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Light {Green, Yellow};

public class Intersection : MonoBehaviour
{
    public Road incomingNorthRoad, incomingEastRoad, incomingSouthRoad, incomingWestRoad, outNorthRoad, outEastRoad, outSouthRoad, outWestRoad;

    public Car[,] occupants = new Car[2, 2];//0,0 is nw corner, 0,1 is ne, 1,0 is sw, 1,1 is se corner
    private bool[,] hasMoved = new bool[2, 2];//Tracks whether a car had been moved into that slot this iteration

    public int greenTime = 10;//Time light stays green
    public int yellowTime = 4;//Time light stays yellow
    public Direction lightDirection = Direction.East;//Direction the light is letting traffic flow to
    public Light lightColor = Light.Green;
    private int lightRemaining;//Time till light changes

    private void Start()
    {
        lightRemaining = greenTime;//Sets initial time on light   
    }

    void Update()
    {
        if (null != occupants[0,0] && Direction.West == occupants[0,0].path.Peek() && testRoad(outWestRoad) && false == hasMoved[0,0])
        {
            outWestRoad.occupants[outWestRoad.occupants.Length - 1] = occupants[0,0];
            occupants[0, 0].distanceTraveled += outWestRoad.occupants.Length;//STATS
            occupants[0, 0].transform.position = new Vector3(occupants[0, 0].transform.position.x - 1, occupants[0, 0].transform.position.y, occupants[0, 0].transform.position.z);
            occupants[0, 0].path.Dequeue();
            occupants[0, 0] = null;
        }//If in nw slot, heading west, and road has an open slot
        if (null != occupants[0, 0] && (Direction.East == occupants[0, 0].path.Peek() || Direction.South == occupants[0, 0].path.Peek()) && null == occupants[1, 0] && false == hasMoved[0, 0])
        {
            occupants[1, 0] = occupants[0, 0];
            occupants[0, 0].distanceTraveled++;//STATS
            occupants[0, 0].transform.position = new Vector3(occupants[0, 0].transform.position.x, occupants[0, 0].transform.position.y - 1, occupants[0, 0].transform.position.z);
            occupants[0, 0] = null;
            hasMoved[1, 0] = true;
        }//If in nw slot, heading east or south, and sw corner has an open slot
        if (null != occupants[0, 0] && Direction.North == occupants[0, 0].path.Peek() && null == occupants[0, 1] && false == hasMoved[0, 0])
        {
            occupants[0, 1] = occupants[0, 0];
            occupants[0, 0].distanceTraveled++;//STATS
            occupants[0, 0].transform.position = new Vector3(occupants[0, 0].transform.position.x + 1, occupants[0, 0].transform.position.y, occupants[0, 0].transform.position.z);
            occupants[0, 0] = null;
            hasMoved[0, 1] = true;
        }//If in nw slot, heading north, and ne corner has an open slot  --- U-turn


        if (null != occupants[0, 1] && Direction.North == occupants[0, 1].path.Peek() && testRoad(outNorthRoad) && false == hasMoved[0, 1])
        {
            outNorthRoad.occupants[outNorthRoad.occupants.Length - 1] = occupants[0, 1];
            occupants[0, 1].distanceTraveled += outNorthRoad.occupants.Length;//STATS
            occupants[0, 1].transform.position = new Vector3(occupants[0, 1].transform.position.x, occupants[0, 1].transform.position.y + 1, occupants[0, 1].transform.position.z);
            occupants[0, 1].path.Dequeue();
            occupants[0, 1] = null;
        }//If in ne slot, heading north, and road has an open slot
        if (null != occupants[0, 1] && (Direction.West == occupants[0, 1].path.Peek() || Direction.South == occupants[0, 1].path.Peek()) && null == occupants[0, 0] && false == hasMoved[0, 1])
        {
            occupants[0, 0] = occupants[0, 1];
            occupants[0, 1].distanceTraveled++; //STATS
            occupants[0, 1].transform.position = new Vector3(occupants[0, 1].transform.position.x - 1, occupants[0, 1].transform.position.y, occupants[0, 1].transform.position.z);
            occupants[0, 1] = null;
            hasMoved[0, 0] = true;
        }//If in ne slot, heading west or south, and nw corner has an open slot
        if (null != occupants[0, 1] && Direction.East == occupants[0, 1].path.Peek() && null == occupants[1, 1] && false == hasMoved[0, 1])
        {
            occupants[1, 1] = occupants[0, 1];
            occupants[0, 1].distanceTraveled++;//STATS
            occupants[0, 1].transform.position = new Vector3(occupants[0, 1].transform.position.x, occupants[0, 1].transform.position.y - 1, occupants[0, 1].transform.position.z);
            occupants[0, 1] = null;
            hasMoved[1, 1] = true;
        }//If in ne slot, heading east, and se corner has an open slot  --- U-turn


        if (null != occupants[1, 0] && Direction.South == occupants[1, 0].path.Peek() && testRoad(outSouthRoad) && false == hasMoved[1, 0])
        {
            outSouthRoad.occupants[outSouthRoad.occupants.Length - 1] = occupants[1, 0];
            occupants[1, 0].distanceTraveled += outSouthRoad.occupants.Length;//STATS
            occupants[1, 0].transform.position = new Vector3(occupants[1, 0].transform.position.x, occupants[1, 0].transform.position.y - 1, occupants[1, 0].transform.position.z);
            occupants[1, 0].path.Dequeue();
            occupants[1, 0] = null;
        }//If in sw slot, heading south, and road has an open slot
        if (null != occupants[1, 0] && (Direction.East == occupants[1, 0].path.Peek() || Direction.North == occupants[1, 0].path.Peek()) && null == occupants[1, 1] && false == hasMoved[1, 0])
        {
            occupants[1, 1] = occupants[1, 0];
            occupants[1, 0].distanceTraveled++;//STATS
            occupants[1, 0].transform.position = new Vector3(occupants[1, 0].transform.position.x + 1, occupants[1, 0].transform.position.y, occupants[1, 0].transform.position.z);
            occupants[1, 0] = null;
            hasMoved[1, 1] = true;
        }//If in sw slot, heading east or north, and se corner has an open slot
        if (null != occupants[1, 0] && Direction.West == occupants[1, 0].path.Peek() && null == occupants[0, 0] && false == hasMoved[1, 0])
        {
            occupants[0, 0] = occupants[1, 0];
            occupants[1, 0].distanceTraveled++;//STATS
            occupants[1, 0].transform.position = new Vector3(occupants[1, 0].transform.position.x, occupants[1, 0].transform.position.y + 1, occupants[1, 0].transform.position.z);
            occupants[1, 0] = null;
            hasMoved[1, 1] = true;
        }//If in sw slot, heading west, and nw corner has an open slot  --- U-turn


        if (null != occupants[1, 1] && Direction.East == occupants[1, 1].path.Peek() && testRoad(outEastRoad) && false == hasMoved[1, 1])
        {
            outEastRoad.occupants[outEastRoad.occupants.Length - 1] = occupants[1, 1];
            occupants[1, 1].distanceTraveled += outEastRoad.occupants.Length;//STATS
            occupants[1, 1].transform.position = new Vector3(occupants[1, 1].transform.position.x + 1, occupants[1, 1].transform.position.y, occupants[1, 1].transform.position.z);
            occupants[1, 1].path.Dequeue();
            occupants[1, 1] = null;
        }//If in se slot, heading east, and road has an open slot
        if (null != occupants[1, 1] && (Direction.West == occupants[1, 1].path.Peek() || Direction.North == occupants[1, 1].path.Peek()) && null == occupants[0, 1] && false == hasMoved[1, 1])
        {
            occupants[0, 1] = occupants[1, 1];
            occupants[1, 1].distanceTraveled++;//STATS
            occupants[1, 1].transform.position = new Vector3(occupants[1, 1].transform.position.x, occupants[1, 1].transform.position.y + 1, occupants[1, 1].transform.position.z);
            occupants[1, 1] = null;
            hasMoved[0, 1] = true;
        }//If in se slot, heading west or north, and ne corner has an open slot
        if (null != occupants[1, 1] && Direction.South == occupants[1, 1].path.Peek() && null == occupants[1, 0] && false == hasMoved[1, 1])
        {
            occupants[1, 0] = occupants[1, 1];
            occupants[1, 1].distanceTraveled++;//STATS
            occupants[1, 1].transform.position = new Vector3(occupants[1, 1].transform.position.x - 1, occupants[1, 1].transform.position.y, occupants[1, 1].transform.position.z);
            occupants[1, 1] = null;
            hasMoved[1, 0] = true;
        }//If in se slot, heading south, and sw corner has an open slot  --- U-turn

        hasMoved = new bool[2, 2];//Reset all to false before next iteration



        if(null == occupants[1,1] && Direction.North == lightDirection && Light.Green == lightColor)
        {
            
            occupants[1,1] = incomingNorthRoad?.advance();
            
        }
        else
        {
            incomingNorthRoad?.Fill();
        }//Move into intersection if empty, else backfill road
        if (null == occupants[1, 0] && Direction.East == lightDirection && Light.Green == lightColor)
        { 
            occupants[1, 0] = incomingEastRoad?.advance();
            
        }
        else
        {
            incomingEastRoad?.Fill();
        }//Move into intersection if empty, else backfill road
        if (null == occupants[0,1] && Direction.West == lightDirection && Light.Green == lightColor)
        {
            
            occupants[0, 1] = incomingWestRoad?.advance();
            
        }
        else
        {
            incomingWestRoad?.Fill();
        }//Move into intersection if empty, else backfill road
        if (null == occupants[0, 0] && Direction.South == lightDirection && Light.Green == lightColor)
        {
            occupants[0, 0] = incomingSouthRoad?.advance();
            
        }
        else
        {
            incomingSouthRoad?.Fill();
        }//Move into intersection if empty, else backfill road

        lightRemaining--;
        if(0 >= lightRemaining)
        {
            if (Light.Green == lightColor)
            {
                lightRemaining = yellowTime;
                lightColor = Light.Yellow;
            }
            else
            {
                lightRemaining = greenTime;
                lightColor = Light.Green;
                lightDirection = (Direction) ((((int) lightDirection) + 1) % ((int)Enum.GetNames(typeof(Direction)).Length));
            }
        }//Cycle from green to yellow and around directions
    }

    bool testRoad(Road road)
    {
        if(null != road?.occupants && 0 != road.occupants.Length && null == road.occupants[road.occupants.Length - 1])
        {
            return true;
        }//If road exists and has space for cars
        return false;
    }
}
