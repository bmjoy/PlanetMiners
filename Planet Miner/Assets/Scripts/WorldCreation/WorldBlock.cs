using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBlock : MonoBehaviour
{
    protected GameObject wall;

    private GameObject _leftNeighbour;
    private GameObject _rightNeighbour;
    private GameObject _upNeighbour;
    private GameObject _downNeighbour;
    private GameObject _upperleftNeighbour;
    private GameObject _upperrightNeighbour;
    private GameObject _lowerleftNeighbour;
    private GameObject _lowerrightNeighbour;

    public GameObject leftNeighbour { get => _leftNeighbour; set => _leftNeighbour = value; }
    public GameObject rightNeighbour { get => _rightNeighbour; set => _rightNeighbour = value; }
    public GameObject upNeighbour { get => _upNeighbour; set => _upNeighbour = value; }
    public GameObject downNeighbour { get => _downNeighbour; set => _downNeighbour = value; }
    public GameObject upperleftNeighbour { get => _upperleftNeighbour; set => _upperleftNeighbour = value; }
    public GameObject upperrightNeighbour { get => _upperrightNeighbour; set => _upperrightNeighbour = value; }
    public GameObject lowerleftNeighbour { get => _lowerleftNeighbour; set => _lowerleftNeighbour = value; }
    public GameObject lowerrightNeighbour { get => _lowerrightNeighbour; set => _lowerrightNeighbour = value; }

    private void Start()
    {

    }

    public void setNeighbours(GameObject[] _neighbours)
    {
        wall = this.gameObject;
        leftNeighbour = _neighbours[0];
        rightNeighbour = _neighbours[1];
        upNeighbour = _neighbours[2];
        downNeighbour = _neighbours[3];
        upperleftNeighbour = _neighbours[4];
        upperrightNeighbour = _neighbours[5];
        lowerleftNeighbour = _neighbours[6];
        lowerrightNeighbour = _neighbours[7];
    }
}
