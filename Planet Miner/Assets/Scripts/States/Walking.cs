using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Walking : State
{
    private List<Vector3> _path;
    private Vector3 _goal;
    private Vector3 _currentGoal;
    private int _pathIndex = 0;

    private Unit _unit;
    public Walking(Vector3 start, Vector3 goal, Unit unit)
    {
        _unit = unit;
        _goal = goal;
        _path = Pathfinding.findPath(start, goal);
        if (_path != null)
            _currentGoal = _path[_pathIndex];
        else
            _unit.changeState(new Idle());
    }
    public override void run()
    {
        if (_path == null)
            _unit.changeState(new Idle());

        if (_pathIndex < _path.Count)
        {
            _currentGoal = _path[_pathIndex];

            _unit.transform.position = Vector3.MoveTowards(_unit.transform.position, _currentGoal, _unit.moveSpeed);
            _unit.transform.LookAt(_currentGoal);

            if (_unit.transform.position.Equals(_currentGoal))
                _pathIndex++;
        }
        else
        {
            _unit.changeState(new Idle());
        }
    }
}
