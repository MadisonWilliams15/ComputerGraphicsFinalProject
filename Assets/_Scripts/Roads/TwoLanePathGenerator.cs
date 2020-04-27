using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLanePathGenerator : MonoBehaviour
{
    private IntersectionTwoLane[] intersections;
    private List<ExitRoad> exitRoads;
    private Dictionary<(SpawnRoad spawn, ExitRoad exit), Queue<LightDirection>> optimalPaths;
    private System.Random rand;

    void Start()
    {
        intersections = FindObjectsOfType<IntersectionTwoLane>();
        exitRoads = new List<ExitRoad>();
        rand = new System.Random();
        Dictionary<Road, IntersectionTwoLane> grid = new Dictionary<Road, IntersectionTwoLane>();
        //generate exit points from given intersections
        foreach (IntersectionTwoLane i in intersections)
        {
            if (i.outboundNorthRoad?.straightRoadScript is ExitRoad)
            {
                exitRoads.Add((ExitRoad)i.outboundNorthRoad.straightRoadScript);
            }
            if (i.outboundEastRoad?.straightRoadScript is ExitRoad)
            {
                exitRoads.Add((ExitRoad)i.outboundEastRoad.straightRoadScript);
            }
            if (i.outboundSouthRoad?.straightRoadScript is ExitRoad)
            {
                exitRoads.Add((ExitRoad)i.outboundSouthRoad.straightRoadScript);
            }
            if (i.outboundWestRoad?.straightRoadScript is ExitRoad)
            {
                exitRoads.Add((ExitRoad)i.outboundWestRoad.straightRoadScript);
            }
            if (i.inboundNorthRoad?.straightRoadScript != null)
            {
                grid.Add(i.inboundNorthRoad.straightRoadScript, i);
            }
            if (i.inboundEastRoad?.straightRoadScript != null)
            {
                grid.Add(i.inboundEastRoad.straightRoadScript, i);
            }
            if (i.inboundSouthRoad?.straightRoadScript != null)
            {
                grid.Add(i.inboundSouthRoad.straightRoadScript, i);
            }
            if (i.inboundWestRoad?.straightRoadScript != null)
            {
                grid.Add(i.inboundWestRoad.straightRoadScript, i);
            }
        }
        optimalPaths = FloydBigBrain(exitRoads, grid);
        loadWeightedExitRoads();
    }

    public int RandomNumber(int min, int max)
    {
        return rand.Next(min, max);
    }

    public Queue<LightDirection> GetPath(Road entry)
    {
        ExitRoad exit;
        Queue<LightDirection> q;
        //loop until an exit point is selected that can be reached from the given entry point
        do
        {
            //pick exit point from list of exit points randomly
            exit = exitRoads[rand.Next(exitRoads.Count)];
        } while (!optimalPaths.TryGetValue(((SpawnRoad)entry, exit), out q));
        //Return clone of corresponding Queue
        //Important that is a clone, if not they all share the same reference and when you call Dequeue() it will empty the master Queue
        return new Queue<LightDirection>(q);
    }

    //Floyd-Warshall algorithm to generate car path (Queue<Direction>) between all spawn and exit points (SpawnRoad, ExitRoad)
    //Called at startup time so path does not need to be generated everytime a new car is spawned
    //On car spawn it will pick a random ExitRoad then call the Dictionary generated here with it's SpawnRoad and ExitRoad
    //It will return a Queue<Direction> with the path needed to get from Spawn to Exit
    private Dictionary<(SpawnRoad spawn, ExitRoad exit), Queue<LightDirection>> FloydBigBrain(List<ExitRoad> exitRoads, Dictionary<Road, IntersectionTwoLane> dic)
    {
        int n = exitRoads.Count + dic.Count;
        int[,] dist = new int[n, n];
        List<Road> allRoads = new List<Road>(exitRoads);

        foreach (Road r in dic.Keys)
        {
            allRoads.Add(r);
        }

        //Set to 99999 unless they are neighbors
        //Set to 1 if neighbors (will need to change later based on distances/speed limits)
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    dist[i, j] = 0;
                }
                else if (dic.ContainsKey(allRoads[i]) && IsNeighbor(allRoads[j], dic[allRoads[i]], allRoads[i].direction))
                {
                    dist[i, j] = allRoads[i].occupants.Length;
                }
                else
                {
                    dist[i, j] = 99999;
                }
            }
        }

        //Set path matrix
        int[,] path = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                path[i, j] = j;
            }
        }

        //Calc shortest distance & record the path between all points
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    if (dist[j, i] + dist[i, k] < dist[j, k])
                    {
                        dist[j, k] = dist[j, i] + dist[i, k];
                        path[j, k] = path[j, i];
                    }
                }
            }
        }

        //Finally, record paths from all SpawnRoads to all ExitRoads into the dictionary
        Dictionary<(SpawnRoad spawn, ExitRoad exit), Queue<LightDirection>> allPaths = new Dictionary<(SpawnRoad spawn, ExitRoad exit), Queue<LightDirection>>();
        for (int i = 0; i < n; i++)
        {
            if (!(allRoads[i] is SpawnRoad))
            {
                continue;
            }
            for (int j = 0; j < n; j++)
            {
                if (!(allRoads[j] is ExitRoad))
                {
                    continue;
                }
                int u = i;
                Queue<LightDirection> q = new Queue<LightDirection>();
                do
                {
                    u = path[u, j]; //this may not be right, check results for multi intersection system
                    q.Enqueue(allRoads[u].direction);
                } while (u != j);
                allPaths.Add(((SpawnRoad)allRoads[i], (ExitRoad)allRoads[j]), q);
            }
        }
        return allPaths;
    }

    //Helper function for FloydBigBrain
    private bool IsNeighbor(Road r, IntersectionTwoLane i, LightDirection incomingDirection)
    {
        bool TurnIntersection = false;
        if (i.lightController is Turn)
        {
            TurnIntersection = true;
        }
        if (r == i.outboundNorthRoad?.straightRoadScript)
        {
            if (TurnIntersection && incomingDirection == LightDirection.South)
            {
                return false;
            }
            return true;
        }
        if (r == i.outboundEastRoad?.straightRoadScript)
        {
            if (TurnIntersection && incomingDirection == LightDirection.West)
            {
                return false;
            }
            return true;
        }
        if (r == i.outboundSouthRoad?.straightRoadScript)
        {
            if (TurnIntersection && incomingDirection == LightDirection.North)
            {
                return false;
            }
            return true;
        }
        if (r == i.outboundWestRoad?.straightRoadScript)
        {
            if (TurnIntersection && incomingDirection == LightDirection.East)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void loadWeightedExitRoads()
    {
        List<ExitRoad> newList = new List<ExitRoad>();
        HashSet<ExitRoad> exitSet = new HashSet<ExitRoad>(exitRoads);
        foreach(ExitRoad e in exitRoads)
        {
            for(int i = 0; i < e.exitRate; i++)
            {
                newList.Add(e);
                //print(e.id + "   " + e.exitRate);
            }
        }
        exitRoads = newList;
    }
}
