using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {  North, East, South, West, NorthSouth, EastWest }; //NorthSouth & EastWest used by intersection for letting multiple go at once

public class Road : MonoBehaviour
{
    public Direction direction;

    public Car[] occupants;//Vehicles/spaces on road


    public Car advance()
    {
        Car current = occupants[0];

        foreach (Car car in occupants)
        {
            if (null == car)
            {
                continue;
            }//Continue if no car in slot

            switch (direction)
            {
                case Direction.North:
                    car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 1, car.transform.position.z);
                    break;
                case Direction.East:
                    car.transform.position = new Vector3(car.transform.position.x + 1, car.transform.position.y, car.transform.position.z);
                    break;
                case Direction.South:
                    car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y - 1, car.transform.position.z);
                    break;
                case Direction.West:
                    car.transform.position = new Vector3(car.transform.position.x - 1, car.transform.position.y, car.transform.position.z);
                    break;
            }//Moves cars along road
        }



        Array.Copy(occupants, 1, occupants, 0, occupants.Length - 1);
        occupants[occupants.Length - 1] = null;//Shifts data along array
        return current;
    }//Pops head off of queue of cars

    public void Fill()
    {
        if(null == occupants)
        {
            return;
        }

        int i = 1;
        for (; i < occupants.Length; i++)
        {
            if(null == occupants[i - 1] && null != occupants[i])
            {
                Array.Copy(occupants, i, occupants, i - 1, occupants.Length - i);
                occupants[occupants.Length - 1] = null;//Shifts data down
                break;
            }
           
        }//Find first gap in line and fill it

     
        for (;i < occupants.Length; i++)
        {
            if (null != occupants[i - 1])
            {
                Car car = occupants[i - 1];
                switch (direction)
                {
                    case Direction.North:
                        car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 1, car.transform.position.z);
                        break;
                    case Direction.East:
                        car.transform.position = new Vector3(car.transform.position.x + 1, car.transform.position.y, car.transform.position.z);
                        break;
                    case Direction.South:
                        car.transform.position = new Vector3(car.transform.position.x, car.transform.position.y - 1, car.transform.position.z);
                        break;
                    case Direction.West:
                        car.transform.position = new Vector3(car.transform.position.x - 1, car.transform.position.y, car.transform.position.z);
                        break;
                }//Moves cars along road
            }

        }
    }//Will move cars closer to intersection, but not move into intersection

}
