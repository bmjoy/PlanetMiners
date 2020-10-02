using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static List<Node> nodeMap = new List<Node>();

    public void addNode(Node node)
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

    public static List<Vector3> findPath(Node start, Node goal)
    {
        if (!nodeMap.Contains(start) || !nodeMap.Contains(goal) || start == null || goal == null)
            return null;
        //a* initializing
        Debug.Log("Start pathfind");

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
                Debug.Log("Path found, now creating");
                return makePath(current);
            }
            //move current to closedSet
            openSet.Remove(current);

            closedSet.Add(current);
            //go through neighbours

            foreach (Node connectedNode in current.connections)
            {
                if (closedSet.Contains(connectedNode))
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
        Debug.Log("No path found");
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
        nodePos.y = 1;
        path.Add(nodePos);

        while (current.cameFrom != null)
        {
            nodePos = current.cameFrom.position;
            nodePos.y = 1;
            path.Add(nodePos);
            current = current.cameFrom;
        }
        path.Reverse();

        return path;
    }
}
