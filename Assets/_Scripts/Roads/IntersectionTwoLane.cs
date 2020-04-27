using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionTwoLane : MonoBehaviour
{
    public RoadTwoLane inboundNorthRoad, inboundEastRoad, inboundSouthRoad, inboundWestRoad, outboundNorthRoad, outboundEastRoad, outboundSouthRoad, outboundWestRoad;

    public Car[,] occupants = new Car[3, 3];//0,0 is sw corner, 2,0 is se, 0,2 is nw, 2,2 is ne corner

    public IntersectionStatistics intersectionStatistics;

    public iLightController lightController;

    private bool cleared = true;//Tracks whether an intersection was cleared after end of last light

    private void Awake()
    {
        //this.gameObject.AddComponent<IntersectionStatistics>()
        intersectionStatistics = this.gameObject.AddComponent<IntersectionStatistics>();

        //adds user choosen lightcontroller or use existing
        if (true == PlayerPrefs.HasKey("light") && 0 == this.gameObject.GetComponents<iLightController>().Length)
        {
            switch (PlayerPrefs.GetString("light"))
            {
                case "TimedLightController":
                    lightController = (iLightController)this.gameObject.AddComponent(typeof(TimedLightController));
                    break;
                case "RandomLightController":
                    lightController = (iLightController)this.gameObject.AddComponent(typeof(RandomLightController));
                    break;
                case "GreedyLightController":
                    lightController = (iLightController)this.gameObject.AddComponent(typeof(GreedyLightController));
                    break;
                default:
                    lightController = GetComponent<iLightController>();
                    break;
            }
        }
        else
        {
            lightController = GetComponent<iLightController>();
        }

        if (null == lightController)
        {
            Debug.LogError(transform.name + " has no lightcontroller");
            Destroy(this);
        }//Gets light controller or errors        
    }

    void Update()
    {
        string name = this.transform.name;//Use this to do conditional breakpoints

        MoveCar((0, 0), LightDirection.North, (1, 0));
        MoveCar((0, 0), LightDirection.East, (1, 0));
        MoveCar((0, 0), LightDirection.South, outboundSouthRoad);

        MoveCar((1, 0), LightDirection.North, (2, 0));
        MoveCar((1, 0), LightDirection.East, (2, 0));
        MoveCar((1, 0), LightDirection.South, (0, 0));
        MoveCar((1, 0), LightDirection.West, (1, 1));

        MoveCar((2, 0), LightDirection.North, (2, 1));
        MoveCar((2, 0), LightDirection.East, outboundEastRoad);
        MoveCar((2, 0), LightDirection.West, (2, 1));

        MoveCar((0, 1), LightDirection.North, (1, 1));
        MoveCar((0, 1), LightDirection.East, (0, 0));
        MoveCar((0, 1), LightDirection.South, (0, 0));
        MoveCar((0, 1), LightDirection.West, (0, 2));

        MoveCar((1, 1), LightDirection.North, (2, 1));
        MoveCar((1, 1), LightDirection.East, (1, 0));
        MoveCar((1, 1), LightDirection.South, (0, 1));
        MoveCar((1, 1), LightDirection.West, (1, 2));

        MoveCar((2, 1), LightDirection.North, (2, 2));
        MoveCar((2, 1), LightDirection.East, (2, 0));
        MoveCar((2, 1), LightDirection.South, (1, 1));
        MoveCar((2, 1), LightDirection.West, (2, 2));

        MoveCar((0, 2), LightDirection.South, (0, 1));
        MoveCar((0, 2), LightDirection.East, (0, 1));
        MoveCar((0, 2), LightDirection.West, outboundWestRoad);

        MoveCar((1, 2), LightDirection.North, (2, 2));
        MoveCar((1, 2), LightDirection.East, (1, 1));
        MoveCar((1, 2), LightDirection.South, (0, 2));
        MoveCar((1, 2), LightDirection.West, (0, 2));

        MoveCar((2, 2), LightDirection.North, outboundNorthRoad);
        MoveCar((2, 2), LightDirection.West, (1, 2));
        MoveCar((2, 2), LightDirection.South, (1, 2));

        /*HandleRoad((1, 0), (2, 0), LightDirection.North, inboundNorthRoad);
        HandleRoad((0, 1), (0, 0), LightDirection.East, inboundEastRoad);
        HandleRoad((1, 2), (0, 2), LightDirection.South, inboundSouthRoad);
        HandleRoad((2, 1), (2, 2), LightDirection.West, inboundWestRoad);*/
        //Advance normal directions

        cleared = CheckClear();//Check if clear only once, so seperated from each move

        HandleCrossRoad((1, 0), (2, 0), LightDirection.CrossNorthSouth, inboundNorthRoad, inboundSouthRoad, new (int x, int y)[] { (1, 0), (1, 1), (1, 2) }, new (int x, int y)[] { (0, 1), (0, 2) });
        HandleCrossRoad((0, 1), (0, 0), LightDirection.CrossEastWest, inboundEastRoad, inboundWestRoad, new (int x, int y)[] { (0, 1), (1, 1), (2, 1) }, new (int x, int y)[] { (0, 0), (1, 0) });
        HandleCrossRoad((1, 2), (0, 2), LightDirection.CrossNorthSouth, inboundSouthRoad, inboundNorthRoad, new (int x, int y)[] { (1, 0), (1, 1), (1, 2) }, new (int x, int y)[] { (2, 0), (2, 1) });
        HandleCrossRoad((2, 1), (2, 2), LightDirection.CrossEastWest, inboundWestRoad, inboundEastRoad, new (int x, int y)[] { (0, 1), (1, 1), (2, 1) }, new (int x, int y)[] { (1, 2), (2, 2) });
        //Advance cross directions, left lane yields

        updateIntersectionWaitTimes();
        if (lightController.GetFrameChanged() == Time.frameCount - 1)
        {
            intersectionStatistics.carCounts.Add(countCarsInIntersection());
        }
    }

    private void MoveCar((int x, int y) startPosition, LightDirection carDirection, (int x, int y) endPosition)
    {
        Car startCar = occupants[startPosition.x, startPosition.y];
        if (null != startCar && carDirection == startCar.path.Peek() && Time.frameCount != startCar.frameMoved && null == occupants[endPosition.x, endPosition.y])
        {//move one spot in intersection 
            occupants[endPosition.x, endPosition.y] = startCar;
            occupants[startPosition.x, startPosition.y] = null;
            startCar.distanceTraveled++;
            startCar.frameMoved = Time.frameCount;
            startCar.transform.position = new Vector3(startCar.transform.position.x + endPosition.x - startPosition.x, startCar.transform.position.y + endPosition.y - startPosition.y, startCar.transform.position.z);
            RotateCar(startPosition, endPosition, startCar);
        }//If car in starting position, moving specified direction, hasn't moved yet this frame, and no car in end position
    }

    private void MoveCar((int x, int y) startPosition, LightDirection carDirection, RoadTwoLane endRoad)
    {//moving into new road out of intersection 
        Car startCar = occupants[startPosition.x, startPosition.y];
        if (null != startCar && carDirection == startCar.path.Peek() && Time.frameCount != startCar.frameMoved && null != endRoad?.straightRoadScript?.occupants && 0 != endRoad.straightRoadScript.occupants.Length && null == endRoad.straightRoadScript.occupants[endRoad.straightRoadScript.occupants.Length - 1])
        {
            intersectionStatistics.throughput++;//moving car out of intersection so increment throughput
            intersectionStatistics.waitTimes.Add(startCar.waitTime);//collect wait times from cars as they pass through intersection 
            startCar.waitTime = 0;//reset wait time as car enters a new road 
            endRoad.straightRoadScript.occupants[endRoad.straightRoadScript.occupants.Length - 1] = startCar;
            occupants[startPosition.x, startPosition.y] = null;
            startCar.distanceTraveled += endRoad.straightRoadScript.occupants.Length;
            startCar.frameMoved = Time.frameCount;
            startCar.path.Dequeue();
            switch (carDirection)
            {
                case LightDirection.North:
                    startCar.transform.position = new Vector3(startCar.transform.position.x, startCar.transform.position.y + 1, startCar.transform.position.z);
                    RotateCar(startPosition, (startPosition.x, startPosition.y + 1), startCar);
                    break;
                case LightDirection.East:
                    startCar.transform.position = new Vector3(startCar.transform.position.x + 1, startCar.transform.position.y, startCar.transform.position.z);
                    RotateCar(startPosition, (startPosition.x + 1, startPosition.y), startCar);
                    break;
                case LightDirection.South:
                    startCar.transform.position = new Vector3(startCar.transform.position.x, startCar.transform.position.y - 1, startCar.transform.position.z);
                    RotateCar(startPosition, (startPosition.x, startPosition.y - 1), startCar);
                    break;
                case LightDirection.West:
                    startCar.transform.position = new Vector3(startCar.transform.position.x - 1, startCar.transform.position.y, startCar.transform.position.z);
                    RotateCar(startPosition, (startPosition.x - 1, startPosition.y), startCar);
                    break;
            }
        }//If car in starting position, moving specified direction, hasn't moved this frame, and road exists to exit to and road has open spot
    }

    /*
    Left position is slot left lane enters into intersection
    straight position is slot straight lane enters into intersection
    direction is light direction when the incoming road advances
    incoming road is the incoming road
    oncoming road is the road opposite of the incoming road
    mid positions are the slots in the middle of the intersections that the straight lanes don't enter i.e. the spots the left turn lane goes into
    uTurnpositions are the 2 slots that need to be clear for a car to safely execute a u turn e.g. a car going north doing a u turn to go south needs the top left and left middle slot clear to give enough time to execute the turn
    */
    private void HandleCrossRoad((int x, int y) leftPosition, (int x, int y) straightPosition, LightDirection direction, RoadTwoLane incomingRoad, RoadTwoLane oncomingRoad, (int x, int y)[] midPositions, (int x, int y)[] uTurnPositions)
    {//left turns yield
            if (typeof(Turn) != lightController.GetType() && true == incomingRoad?.leftAvailable)
            {
                if (false == (null == occupants[leftPosition.x, leftPosition.y] && direction == lightController.GetLightDirection() && LightColor.Green == lightController.GetLightColor()) || false == cleared)
                {
                    incomingRoad?.FillLeft();
                }// If not open space, correct light then fill left turn lane
                else if ((null != oncomingRoad?.straightRoadScript?.occupants && 3 < oncomingRoad.straightRoadScript.occupants.Length && null == oncomingRoad.straightRoadScript.occupants[3] && null == oncomingRoad.straightRoadScript.occupants[4] && 3 == midPositions?.Length && null == occupants[midPositions[0].x, midPositions[0].y] && null == occupants[midPositions[1].x, midPositions[1].y] && null == occupants[midPositions[2].x, midPositions[2].y] || null == oncomingRoad) && null != incomingRoad?.leftRoadScript?.occupants?[0] && incomingRoad.leftDirection == incomingRoad?.leftRoadScript?.occupants?[0]?.path?.Peek())
                {
                    occupants[leftPosition.x, leftPosition.y] = incomingRoad?.AdvanceLeft();
                    intersectionStatistics.waitTimes.Add(occupants[leftPosition.x, leftPosition.y].waitTime);//collecting left turn car's wait time
                    occupants[leftPosition.x, leftPosition.y].waitTime = 0;
                }//If space in oncoming traffic, and intersection clear and going left or if no oncoming traffic
                else if (2 == uTurnPositions?.Length && null == occupants[uTurnPositions[0].x, uTurnPositions[0].y] && null == occupants[uTurnPositions[1].x, uTurnPositions[1].y] && null != incomingRoad?.leftRoadScript?.occupants?[0] && incomingRoad.uTurnDirection == incomingRoad?.leftRoadScript?.occupants?[0]?.path?.Peek())
                {
                    occupants[leftPosition.x, leftPosition.y] = incomingRoad?.AdvanceLeft();
                    intersectionStatistics.waitTimes.Add(occupants[leftPosition.x, leftPosition.y].waitTime);//collecting u turn car's wait time
                    occupants[leftPosition.x, leftPosition.y].waitTime = 0;
                }//If space in oncoming traffic, and intersection clear doing u-turn
                else
                {
                    incomingRoad?.FillLeft();
                }
            }//No left lane if a turn or left lane doesn't exist

            HandleStraight(straightPosition, direction, incomingRoad);//Handles straight portion same for crosstraffic and normal
    }

    /*private void HandleRoad((int x, int y) leftPosition, (int x, int y) straightPosition, LightDirection direction, RoadTwoLane road)
    {//shouldnt be calling this function anymore but would handle green left turns 
        if(typeof(Turn) != lightController.GetType())
        {
            //Always handle left lane first
            if (null == occupants[leftPosition.x, leftPosition.y] && ((direction == lightController.LightDirection && LightColor.Green == lightController.LightColor)))
            {
                occupants[leftPosition.x, leftPosition.y] = road?.AdvanceLeft();
            }//If spot in intersection open and light is green or a turn. If is a left turn yield, check if oncoming traffic
            else
            {
                road?.FillLeft();
            }//Else fill spot in left lane
        }//No left turn lane on turns

        HandleStraight(straightPosition, direction, road);//Handles straight portion same for crosstraffic and normal
    }*/

    private void HandleStraight((int x, int y) straightPosition, LightDirection direction, RoadTwoLane road)
    {
        if (null == occupants[straightPosition.x, straightPosition.y] && true == cleared && ((direction == lightController.GetLightDirection() && LightColor.Green == lightController.GetLightColor()) || typeof(Turn) == lightController.GetType()))
        {
            occupants[straightPosition.x, straightPosition.y] = road?.AdvanceStraight();
        }//If spot in intersection open and light is green or a turn
        else
        {
            road?.FillStraight();
        }//Else fill spot in straight lane
    }

    private void RotateCar((int x, int y) startPosition, (int x, int y) endPosition, Car car)
    {
        int x2 = startPosition.x - endPosition.x;
        if (x2 == 1)
        {
            car.transform.eulerAngles = new Vector3(0, 0, 90);
            return;
        }
        else if (x2 == -1)
        {
            car.transform.eulerAngles = new Vector3(0, 0, 270);
            return;
        }

        int y2 = startPosition.y - endPosition.y;
        if (y2 == 1)
        {
            car.transform.eulerAngles = new Vector3(0, 0, 180);
            return;
        }
        else
        {
            car.transform.eulerAngles = new Vector3(0, 0, 0);
            return;
        }
    }

    public int countCarsInIntersection()
    {
        int count = 0;
        if (inboundWestRoad != null)
        {
            count += inboundWestRoad.countCars();
        }
        if (inboundSouthRoad != null)
        {
            count += inboundSouthRoad.countCars();
        }
        if (inboundNorthRoad != null)
        {
            count += inboundNorthRoad.countCars();
        }
        if (inboundEastRoad != null)
        {
            count += inboundEastRoad.countCars();
        }
        return count;
    }


    public void updateIntersectionWaitTimes()
    {
        inboundEastRoad?.updateRoadWaitTimes();
        inboundNorthRoad?.updateRoadWaitTimes();
        inboundSouthRoad?.updateRoadWaitTimes();
        inboundWestRoad?.updateRoadWaitTimes();

    }

    private bool CheckClear()
    {//Checks if intersection clear
        if (Time.frameCount == lightController.GetFrameChanged() + 1 && lightController.GetLightColor() == LightColor.Green || false == cleared)
        {
            for (int rows = 0; 3 > rows; rows += 1)
            {
                for (int cols = 0; 3 > cols; cols += 1)
                {
                    if (null != occupants[rows, cols])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}