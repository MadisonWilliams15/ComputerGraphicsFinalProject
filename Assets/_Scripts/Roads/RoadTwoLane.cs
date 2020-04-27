using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTwoLane : MonoBehaviour
{
    private GameObject leftRoad;
    private GameObject straightRoad;
    [HideInInspector]
    public Road leftRoadScript;
    [HideInInspector]
    public Road straightRoadScript;
    private int leftLaneLength;
    private int straightLaneLength;
    [HideInInspector]
    public LightDirection leftDirection;
    [HideInInspector]
    public LightDirection uTurnDirection;
    private Vector3 leftShift;
    public bool leftAvailable = true;

    void Awake()
    {
        Transform firstItem = transform.GetChild(0);
        Transform secondItem = transform.GetChild(1);
        if (firstItem.name.Equals("StraightLane"))
        {
            straightRoad = firstItem.gameObject;
            straightRoadScript = straightRoad.GetComponent<Road>();
        }
        else if (firstItem.name.Equals("LeftLane"))
        {
            leftRoad = firstItem.gameObject;
            leftRoadScript = leftRoad.GetComponent<Road>();
        }

        if (secondItem.name.Equals("StraightLane"))
        {
            straightRoad = secondItem.gameObject;
            straightRoadScript = straightRoad.GetComponent<Road>();
        }
        else if (secondItem.name.Equals("LeftLane"))
        {
            leftRoad = secondItem.gameObject;
            leftRoadScript = leftRoad.GetComponent<Road>();
        }

        if (!leftRoad || !straightRoad || !leftRoadScript || !straightRoadScript)
        {
            Debug.LogError("cannot load inner Road objects. Make sure they are named 'LeftLane' and 'StraightLane'");
            Destroy(this);
        }

        leftLaneLength = leftRoadScript.occupants.Length - 1;
        if (leftLaneLength == -1) leftAvailable = false;
        straightLaneLength = straightRoadScript.occupants.Length - 1;
        LightDirection direction = straightRoadScript.direction;
        if (direction == LightDirection.North)
        {
            leftShift = new Vector3(-1,0,0);
            leftDirection = LightDirection.West;
            uTurnDirection = LightDirection.South;
        }
        else if (direction == LightDirection.East)
        {
            leftShift = new Vector3(0,1,0);
            leftDirection = LightDirection.North;
            uTurnDirection = LightDirection.West;
        }
        else if (direction == LightDirection.South)
        {
            leftShift = new Vector3(1, 0, 0);
            leftDirection = LightDirection.East;
            uTurnDirection = LightDirection.North;
        }
        else if (direction == LightDirection.West)
        {
            leftShift = new Vector3(0, -1, 0);
            leftDirection = LightDirection.South;
            uTurnDirection = LightDirection.East;
        }
    }

    /*
     * Four methods to interact with the two lanes:
     *  - AdvanceStraight: advances the straight lane and returns a car if it exits into Intersection
     *  - AdvanceLeft: advances the left lane and returns a car if it exits into Intersection
     *  - FillStraight: Fills straight lane if cars are stopped
     *  - FillLeft: Fills left lane if cars are stopped
     * 
     * If a car wishes to turn left then when it arrives at the end of the left lane it will first check if the space
     *  is occupied. If it is not then it will move into the left lane. If it is filled then it cannot move so the cars in
     *  front of it can advance/fill to the front and the cars behind it can fill up to his car. When the left lane is clear
     *  then the car will move into that lane allowing the cars by. This is all handled within the functions.
     *  
     **** Make sure to always call the left lane advance/fill before the straight lane. If you do not then there could be
     *      unnecessary dropped frames. ****
     * 
     */
    
    public Car AdvanceStraight()
    {
        if (!leftAvailable || shiftLeft())
        {
            return straightRoadScript.advance();
        }
        else
        {
            straightRoadScript.Fill(leftLaneLength + 2, straightLaneLength + 1);
            return straightRoadScript.advance(leftLaneLength);
        }
    }

    public Car AdvanceLeft()
    {
        if (!leftAvailable)
        {
            throw new System.Exception("No Left Lane Available");
        }
        return leftRoadScript.advance();
    }

    public void FillStraight()
    {
        if (!leftAvailable || shiftLeft())
        {
            straightRoadScript.Fill();
        }
        else
        {
            straightRoadScript.Fill(1, leftLaneLength);
            straightRoadScript.Fill(leftLaneLength + 2, straightLaneLength + 1);
        }
    }

    public void FillLeft()
    {
        if (!leftAvailable)
        {
            throw new System.Exception("No Left Lane Available");
        }
        leftRoadScript.Fill();
    }

    private bool shiftLeft()
    {
        if (straightRoadScript.occupants[leftLaneLength] == null)
        {
            return true;
        }
        else if (straightRoadScript.occupants[leftLaneLength].path.Peek() != leftDirection && straightRoadScript.occupants[leftLaneLength].path.Peek() != uTurnDirection)
        {
            return true;
        }
        else if (leftRoadScript.occupants[leftLaneLength] != null)
        {
            return false;
        }
        Car moveLeft = straightRoadScript.occupants[leftLaneLength];
        leftRoadScript.occupants[leftLaneLength] = moveLeft;
        moveLeft.transform.position += leftShift;

        straightRoadScript.occupants[leftLaneLength] = null;
        return true;
    }

    public int countCars()
	{
		int counter = 0;
        for(int i = 0; i < straightRoadScript.occupants.Length; i++)
		{
            if(straightRoadScript.occupants[i] != null)
			{
				counter++;
			}
		}

		if (leftAvailable)
		{
			for (int i = 0; i < leftRoadScript.occupants.Length; i++)
			{
                if(leftRoadScript.occupants[i] != null)
				{
					counter++;
				}

			}

		}

		return counter;
	}

	public void updateRoadWaitTimes()
	{
		for (int i = 0; i < straightRoadScript.occupants.Length; i++)
		{
			if (straightRoadScript.occupants[i] != null && straightRoadScript.occupants[i].frameMoved != Time.frameCount)
			{
				straightRoadScript.occupants[i].waitTime++;
			}
		}

		if (leftAvailable)
		{
			for (int i = 0; i < leftRoadScript.occupants.Length; i++)
			{
				if (leftRoadScript.occupants[i] != null && leftRoadScript.occupants[i].frameMoved != Time.frameCount)
				{
					leftRoadScript.occupants[i].waitTime++;
				}

			}
		}
	}
}