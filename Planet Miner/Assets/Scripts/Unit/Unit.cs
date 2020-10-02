using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Node nodeEnd;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.TryGetComponent<Node>(out nodeEnd))
                {

                    Node nodeStart;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit))
                    {
                        if (hit.transform.TryGetComponent<Node>(out nodeStart))
                        {
                            StartCoroutine(walkPath(nodeStart, nodeEnd));
                        }
                    }
                }
            }
        }
    }

    IEnumerator walkPath(Node start, Node goal)
    {
        List<Vector3> path = Pathfinding.findPath(start, goal);

        Vector3 current = path[0];
        int index = 0;

        while (index < path.Count)
        {
            current = path[index];
            yield return new WaitForSeconds(.25f);

            this.transform.position = Vector3.MoveTowards(transform.position, current, .25f);

            if (transform.position.Equals(current))
                index++;

        }
    }


}
