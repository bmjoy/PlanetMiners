using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;

    private State state = null;

    GameObject walltarget;

    public float moveSpeed
    {
        get { return _moveSpeed * Time.deltaTime; }
    }
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.TryGetComponent<Node>(out Node nodeEnd))
                {                   
                    if (Physics.Raycast(transform.position, Vector3.down, out hit))
                    {
                        if (hit.transform.TryGetComponent<Node>(out Node nodeStart))
                        {
                            changeState(new Walking(nodeStart.position, nodeEnd.position, this,null));
                        }
                    }
                }

                if(hit.transform.TryGetComponent<Wall>(out Wall wall))
                {
                    foreach(GameObject go in wall.neighbours.Values)
                    {
                        if(go.TryGetComponent<Node>(out Node endnode))
                        {

                            if (Physics.Raycast(transform.position, Vector3.down, out hit))
                            {
                                if (hit.transform.TryGetComponent<Node>(out Node nodeStart))
                                {
                                    changeState(new Walking(nodeStart.position, endnode.position, this,wall));
                                    break;
                                }
                            }
                        }
                        
                    }
                }
            }
        }

        if (state != null)
            state.execute();

    }

    public void changeState(State state)
    {
        this.state = state;
    }




}
