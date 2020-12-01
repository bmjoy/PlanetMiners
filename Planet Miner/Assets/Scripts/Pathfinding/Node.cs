using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private List<Node> _connections;

    //most efficient previous node
    [SerializeField]
    private Node _cameFrom = null;

    [SerializeField]
    private int _g = 0;
    [SerializeField]
    private int _h = 0;
    [SerializeField]
    private int _f = 0;
    [SerializeField]
    private bool _canWalkHere = true;


    public void initialize()
    {
        _connections = new List<Node>();
    }

    public void addConnection(Node connectTo)
    {
        if (!_connections.Contains(connectTo))
            _connections.Add(connectTo);

    }
    public void removeConnection(Node n, bool bothSides)
    {
        connections.Remove(n);
        if (bothSides)
            n.removeConnection(this, false);
    }

    public void removeAllConnections()
    {
        foreach (Node n in connections)
        {
            n.removeConnection(this, false);
            connections.Remove(n);
        }
    }

    public List<Node> connections
    {
        get => _connections;
    }

    public bool canWalkHere
    {
        get => _canWalkHere;
        set => _canWalkHere = value;
    }

    //calculate the distance to another node
    public int distanceTo(Node node)
    {
        int distance;
        int nY;
        int nX;

        nX = (int)Mathf.Abs(position.x - node.position.x);
        nY = (int)Mathf.Abs(position.y - node.position.y);

        distance = (int)Mathf.Sqrt(nX * nX + nY * nY);

        return distance;
    }


    // calculate or set the g, h, f and previous of this node
    public int g
    {
        get => _g;
        set => _g = value;
    }
    public void h(Node goal)
    {
        _h = distanceTo(goal);
    }

    public int getH
    {
        get => _h;
    }
    public int f
    {
        get => _f;
    }

    public void calculateF()
    {
        _f = _g + _h;
    }
    public Node cameFrom
    {
        get => _cameFrom;
        set => _cameFrom = value;
    }

    public Vector3 position
    {
        get => transform.position;
    }
}
