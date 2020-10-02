using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldBlock : MonoBehaviour
{
    private float _health = 0;

    protected WorldGen worldGen;

    private Dictionary<string, GameObject> _neighbours = new Dictionary<string, GameObject>();

    public float health
    {
        get => _health;
        set => _health = value;
    }

    public virtual void doDamage(float damage)
    {
        _health -= damage;
    }

    public Dictionary<string,GameObject> neighbours { get => _neighbours; }
    

    public void setNeighbours(GameObject[] neighbours)
    {
        _neighbours["left"] = neighbours[0];
        _neighbours["right"] = neighbours[1];

        _neighbours["up"] = neighbours[2];
        _neighbours["down"] = neighbours[3];

        _neighbours["upperLeft"] =  neighbours[4];
        _neighbours["upperRight"] = neighbours[5];

        _neighbours["lowerLeft"] = neighbours[6];
        _neighbours["lowerRight"] = neighbours[7];
    }


    private void Start()
    {
        worldGen = FindObjectOfType<WorldGen>();
    }
}
