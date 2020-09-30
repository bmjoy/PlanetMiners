using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IStateMachine
{
    private State _state;

    public State getState => _state;

    public State changeState { set => _state = value; }

    private void Update()
    {
        _state.excecute(this);
    }


    private void move(Vector3 screenSpace)
    {
        
    }
}
