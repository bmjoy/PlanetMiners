using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = .5f;

    private State state = null;

    public float moveSpeed
    {
        get { return _moveSpeed * Time.deltaTime;}
    }
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
                            changeState(new Walking(nodeStart.position, nodeEnd.position,this));
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
