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


    public void initialize()
    {
        _connections = new List<Node>();
    }

    public void addConnection(Node connectTo)
    {
        if (!_connections.Contains(connectTo))
            _connections.Add(connectTo);

    }

    public List<Node> connections
    {
        get => _connections;
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

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(position, new Vector3(.5f, 1f, .5f));

        Gizmos.color = Color.green;

        Vector3 drawpos = position;
        Vector3 drawto;
        Vector3 arrowOffset = new Vector3(0, 0, 0) ;
        
        foreach (Node node in connections)
        {
            drawto = node.position;
            drawpos = position;
            //under
            if (drawto.z > position.z) {

                drawpos.x -= .25f;
                drawto.x -= .25f;

                arrowOffset = new Vector3(.25f,0,-.5f);
            }
            else
            //over
            if (drawto.z < position.z)
            {
                drawpos.x += .25f;
                drawto.x += .25f;
         
                arrowOffset = new Vector3(.25f, 0, .5f);
            }
            else
            //right
            if (drawto.x > position.x)
            {
                drawpos.z += .25f;
                drawto.z += .25f;
                arrowOffset = new Vector3(-.5f, 0,.25f);
            }
            else
            //left
            if (drawto.x > position.x)
            {
                drawpos.z -= .25f;
                drawto.z -= .25f;
                
                arrowOffset = new Vector3(.5f, 0,.25f);

            }

            Gizmos.DrawLine(drawpos, drawto);
            Gizmos.DrawLine(drawto, drawpos - arrowOffset);


        }
    }*/
}
