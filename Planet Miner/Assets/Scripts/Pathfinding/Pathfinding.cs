using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private static List<Node> nodeMap = new List<Node>();

    public static void addNode(Node node)
    {
        node.initialize();

        foreach (Node n in nodeMap)
            if (Vector3.Distance(n.transform.position, node.transform.position) <= 1)
            {
                n.addConnection(node);
                node.addConnection(n);
            }

        nodeMap.Add(node);
    }

    public static Node getNodeByPosition(Vector3 nodePos)
    {
        nodePos.y = 0;

        foreach (Node node in nodeMap)
            if (node.transform.position == nodePos)
                return node;

        return null;
    }

    public static List<Vector3> findPath(Vector3 startPos, Vector3 endPos)
    {
        Node start = getNodeByPosition(startPos);
        Node goal = getNodeByPosition(endPos);

        if (start == null || goal == null)
            return null;
        //a* initializing


        //list of unvisited nodes
        List<Node> openSet = new List<Node>();

        List<Node> closedSet = new List<Node>();

        //adds the start node to the open set
        start.g = 0;
        start.h(goal);
        start.calculateF();
        start.cameFrom = null;

        openSet.Add(start);

        //current node
        Node current;

        //start of the pathfinder
        while (openSet.Count > 0)
        {
            current = getLowest(openSet);

            //check if current is goal
            if (current == goal)
            {
                return makePath(current);
            }
            //move current to closedSet
            openSet.Remove(current);

            closedSet.Add(current);
            //go through neighbours

            foreach (Node connectedNode in current.connections)
            {
                if (closedSet.Contains(connectedNode) || !connectedNode.canWalkHere)
                    continue;

                connectedNode.g = int.MaxValue;
                connectedNode.calculateF();
                connectedNode.cameFrom = null;


                int tentativeG = current.g + current.distanceTo(connectedNode);

                if (tentativeG < connectedNode.g)
                {
                    connectedNode.cameFrom = current;
                    connectedNode.g = tentativeG;
                    connectedNode.h(goal);
                    connectedNode.calculateF();

                    if (!openSet.Contains(connectedNode))
                        openSet.Add(connectedNode);
                }
            }
        }
        //no target found
        return null;
    }

    private static Node getLowest(List<Node> nodes)
    {
        Node lowest = nodes[0];
        foreach (Node node in nodes)
        {
            if (node.f < lowest.f)
                lowest = node;
        }

        return lowest;
    }

    private static List<Vector3> makePath(Node _current)
    {
        List<Vector3> path = new List<Vector3>();

        Node current = _current;
        Vector3 nodePos = current.position;
        
        path.Add(nodePos);

        while (current.cameFrom != null)
        {
            nodePos = current.cameFrom.position;
            
            path.Add(nodePos);
            current = current.cameFrom;
        }
        path.Reverse();

        return path;
    }

    public static void checkForNewConnections()
    {
        Node n1 = null, n2 = null;

        for (int i = 0; i < nodeMap.Count; i++)
        {

            n1 = nodeMap[i];
            if (i + 1 < nodeMap.Count)
                n2 = nodeMap[i + 1];
            else if (i == nodeMap.Count - 1)
                n2 = nodeMap[i - 1];

            if (Vector3.Distance(n1.transform.position, n2.transform.position) <= 1)
            {
                n1.addConnection(n2);
                n2.addConnection(n1);
            }

        }
    }

    public static bool checkForPath(Vector3 startpos,  Vector3 endpos)
    {
        return (findPath(startpos, endpos) != null);
    }
}
