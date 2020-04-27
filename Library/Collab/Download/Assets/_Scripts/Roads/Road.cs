using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightDirection { North, East, South, West, CrossNorthSouth, CrossEastWest }; //NorthSouth & EastWest used by intersection for letting multiple go at once

public class Road : MonoBehaviour
{
    public LightDirection direction;

    public Car[] occupants;//Vehicles/spaces on road
    private bool lastCarMoved = false;//Tracks whether last car will need to be advanced this tick

    public Car advance()
    {
        return advance(occupants.Length);
    }

    public Car advance(int numberOfElements)
    {
        Car current = occupants[0];

        for(int i = 0; i < numberOfElements; i++)
        {
            Car car = occupants[i];
            if (null == car)
            {
                continue;
            }//Continue if no car in slot

            if (Time.frameCount == car.frameMoved)
            {
                car.frameMoved = null;
                lastCarMoved = true;
                continue;
            }

            switch (direction)
            {
                case LightDirection.North:
                    car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 1, car.transform.position.z);
                    break;
                case LightDirection.East:
                    car.transform.position = new Vector3(car.transform.position.x + 1, car.transform.position.y, car.transform.position.z);
                    break;
                case LightDirection.South:
                    car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y - 1, car.transform.position.z);
                    break;
                case LightDirection.West:
                    car.transform.position = new Vector3(car.transform.position.x - 1, car.transform.position.y, car.transform.position.z);
                    break;
            }//Moves cars along road
        }


        if(false == lastCarMoved)
        {
            Array.Copy(occupants, 1, occupants, 0, numberOfElements - 1);
            occupants[numberOfElements - 1] = null;//Shifts data along array
        }
        else
        {
            Array.Copy(occupants, 1, occupants, 0, numberOfElements - 2);
            occupants[numberOfElements - 2] = null;//Shifts data along array
            lastCarMoved = false;//Reset for next tick
        }//If last car already moved, slide 1 less down
        return current;
    }//Pops head off of queue of cars


    /*
     * Two seperate functions for backward compatibility
     * Calling Fill with no parameters will call Fill with the min as 1 and max as occupants.Length
     * This will allow Fill to only shift part of the array if necessary
     * This is necessary for two lane roads in the specific case that:
     *      - the left lane is filled
     *      - a car in the straight lane needs to turn left
     *      - it will wait in the straight lane for the left lane to open
     *      - cars in front of the left turning car should advance/fill up to front of the lane
     *      - cars behing should fill up to directly behind the left turning car
     *      
     * Min: the first car to check (Default = 1 because the first car in the Road cannot move forward any further)
     * Max: Last Car to check + 1 (Default = occupants.Length so the entire road is checked)
     */

    public void Fill()
    {
        Fill(1, occupants.Length);
    }

    public void Fill(int min, int max)
    {
        if(null == occupants)
        {
            return;
        }

        int i = min;
        for (; i < max; i++)
        {
            if(null == occupants[i - 1] && null != occupants[i])
            {
                if (null != occupants[max - 1] && Time.frameCount == occupants[max - 1].frameMoved)
                {
                    Array.Copy(occupants, i, occupants, i - 1, max - i - 1);
                    occupants[max - 2] = null;//Shifts data down
                    break;
                }//If last car already moved, don't shift it
                else
                {
                    Array.Copy(occupants, i, occupants, i - 1, max - i);
                    occupants[max - 1] = null;//Shifts data down
                    break;
                }
            }  
        }//Find first gap in line and fill it

     
        for (;i < max; i++)
        {
            if (null != occupants[i - 1])
            {
                Car car = occupants[i - 1];
                if (Time.frameCount != car.frameMoved)
                {
                    switch (direction)
                    {
                        case LightDirection.North:
                            car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 1, car.transform.position.z);
                            break;
                        case LightDirection.East:
                            car.transform.position = new Vector3(car.transform.position.x + 1, car.transform.position.y, car.transform.position.z);
                            break;
                        case LightDirection.South:
                            car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y - 1, car.transform.position.z);
                            break;
                        case LightDirection.West:
                            car.transform.position = new Vector3(car.transform.position.x - 1, car.transform.position.y, car.transform.position.z);
                            break;
                    }//Moves cars along road
                }
                else
                {
                    car.frameMoved = null;
                }//If car has moved, don't move again, else reset for next iteration
            }

        }
    }//Will move cars closer to intersection, but not move into intersection

}
