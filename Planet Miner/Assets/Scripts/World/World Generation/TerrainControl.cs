using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainControl : MonoBehaviour
{
    [Header("World size")]
    public int worldWidth = 10;
    public int worldHeight = 10;
    public float scale = 1;
    public float offsetX = 1;
    public float offsetZ = 1;

    public int startAreaSize = 3;

    private GameObject[,] worldMap;
    private float[,] noiseMap;

    [Header("Prefabs")]
    public GameObject[] walls;
    public GameObject[] grounds;
    [Space]
    public GameObject unitPrefab;

    private List<Unit> units = new List<Unit>();

    [Header("Spawn Range Terrain")]
    [Range(0, 1f)]
    public float groundRange;

    [Space()]
    [Header("Spawn chance for crystals")]
    [Range(0, 1)]
    public float chanceCrystal = 0f;

    //World object class lists
    private List<Ground> groundObjects = new List<Ground>();
    private List<Wall> wallObjects = new List<Wall>();
    private List<Node> nodeObjects = new List<Node>();


    public List<Unit> getUnits()
    {
        return units;
    }
    public void generateWorld()
    {
        offsetX = UnityEngine.Random.Range(0, 1000);
        offsetZ = UnityEngine.Random.Range(0, 1000);

        worldWidth = PlayerPrefs.GetInt("WorldWidth");
        worldHeight = PlayerPrefs.GetInt("WorldHeight");

        worldMap = new GameObject[worldWidth, worldHeight];

        noiseMap = GeneratePerlinMap.generateMap(worldWidth, worldHeight, scale, offsetX, offsetZ);

        noiseMap = convertNoiseMapToWorld(noiseMap);

        int unitcount = 0;
        //generate base walls and floors
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {

                if (z == 0 || x == 0 || z == worldHeight - 1 || x == worldWidth - 1)
                {
                    //top left world corner
                    if (x == 0 && z == 0)
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.Euler(0, 90, 0), this.transform);
                    //bot right world corner
                    else if (x == 0 && z == worldHeight - 1)
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.Euler(0, 180, 0), this.transform);
                    //top right world corner
                    else if (x == worldWidth - 1 && z == 0)
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0), this.transform);
                    //bot left world corner
                    else if (x == worldWidth - 1 && z == worldHeight - 1)
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.Euler(0, -90, 0), this.transform);
                    //walls side of world
                    else
                    {
                        if (x == 0)
                            worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.Euler(0, 90, 0), this.transform);
                        else if (x == worldWidth - 1)
                            worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.Euler(0, -90, 0), this.transform);
                        else if (z == 0)
                            worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0), this.transform);
                        else
                            worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.Euler(0, 180, 0), this.transform);
                    }

                }
                //normal wall
                else if (noiseMap[x, z] == 1)
                {
                    worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                    //crystal wall
                }
                else if (noiseMap[x, z] == 2)
                {
                    worldMap[x, z] = Instantiate(walls[3], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                }
                //ground
                else if (noiseMap[x, z] == 0)
                {
                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                    worldMap[x, z].name = "Ground" + x + "," + z;
                }
                else if (noiseMap[x, z] == 3)
                {
                    //TEMP
                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                    worldMap[x, z].name = "Ground" + x + "," + z;

                    GameObject u = Instantiate(unitPrefab, new Vector3(x, 1, z), Quaternion.identity, this.transform);
                    unitcount++;

                }
            }
        }
        //remove single ground caves
        removeSingleGroundCaves(ref noiseMap);

        //remove lonely walls
        removeSingleWalls(0, 0, worldWidth, worldHeight);

        //rotate normal walls
        rotateWallsToGround(0, 0, worldWidth, worldHeight);

        //create normal corners
        createCorners(0, 0, worldWidth, worldHeight);

        //create inner corners
        createInnerCorners(0, 0, worldWidth, worldHeight);
    }
    private float[,] convertNoiseMapToWorld(float[,] _noiseMap)
    {

        int centerx, centerz;

        centerx = Mathf.FloorToInt(worldWidth / 2);
        centerz = Mathf.FloorToInt(worldHeight / 2);



        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                if (_noiseMap[x, z] <= groundRange)
                    _noiseMap[x, z] = 0;
                else

                    if (UnityEngine.Random.Range(0, 2f) <= chanceCrystal)
                    _noiseMap[x, z] = 2;
                else
                    _noiseMap[x, z] = 1;


            }
        }

        //insert start area beginning in center
        for (int x = centerx; x < centerx + startAreaSize + 1; x++)
        {
            for (int z = centerz; z < centerz + startAreaSize + 1; z++)
            {
                if (x == centerx)
                    _noiseMap[x, z] = 1;
                else
                if (z == centerz)
                    _noiseMap[x, z] = 1;
                else
                if (x == x + startAreaSize)
                    _noiseMap[x, z] = 1;
                else
                if (z == z + startAreaSize)
                    _noiseMap[x, z] = 1;
                else
                    _noiseMap[x, z] = 0;
            }
        }


        _noiseMap[centerx + (int)startAreaSize / 2, centerz + (int)startAreaSize / 2] = 3;

        //force double walls
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                if (_noiseMap[x, z] == 0)
                {
                    //left
                    if (x > 1 && _noiseMap[x - 1, z] == 1)
                    {
                        if (_noiseMap[x - 2, z] == 0)
                            _noiseMap[x - 2, z] = 1;
                    }

                    //right
                    if (x < worldWidth - 2 && _noiseMap[x + 1, z] == 1)
                    {
                        if (_noiseMap[x + 2, z] == 0)
                            _noiseMap[x + 2, z] = 1;
                    }
                    //up
                    if (z < worldHeight - 2 && _noiseMap[x, z + 1] == 1)
                    {
                        if (_noiseMap[x, z + 2] == 0)
                            _noiseMap[x, z + 2] = 1;
                    }
                    //down
                    if (z > 1 && _noiseMap[x, z - 1] == 1)
                    {
                        if (_noiseMap[x, z - 2] == 0)
                            _noiseMap[x, z - 2] = 1;
                    }

                    //up left
                    if (x > 1 && z < worldHeight - 2 && _noiseMap[x - 1, z + 1] == 1)
                    {
                        if (_noiseMap[x - 2, z + 1] == 0)
                            _noiseMap[x - 2, z + 1] = 1;

                        if (_noiseMap[x - 1, z + 2] == 0)
                            _noiseMap[x - 1, z + 2] = 1;

                        if (_noiseMap[x - 2, z + 2] == 0)
                            _noiseMap[x - 2, z + 2] = 1;
                    }

                    //up right
                    if (x < worldWidth - 2 && z < worldHeight - 2 && _noiseMap[x + 1, z + 1] == 1)
                    {
                        if (_noiseMap[x + 2, z + 1] == 0)
                            _noiseMap[x + 2, z + 1] = 1;

                        if (_noiseMap[x + 1, z + 2] == 0)
                            _noiseMap[x + 1, z + 2] = 1;

                        if (_noiseMap[x + 2, z + 2] == 0)
                            _noiseMap[x + 2, z + 2] = 1;
                    }

                    //down left
                    if (x > 1 && z > 1 && _noiseMap[x - 1, z - 1] == 1)
                    {
                        if (_noiseMap[x - 2, z - 1] == 0)
                            _noiseMap[x - 2, z - 1] = 1;

                        if (_noiseMap[x - 1, z - 2] == 0)
                            _noiseMap[x - 1, z - 2] = 1;

                        if (_noiseMap[x - 2, z - 2] == 0)
                            _noiseMap[x - 2, z - 2] = 1;
                    }
                    //down right
                    if (x < worldWidth - 2 && z > 1 && _noiseMap[x + 1, z - 1] == 1)
                    {
                        if (_noiseMap[x + 2, z - 1] == 0)
                            _noiseMap[x + 2, z - 1] = 1;

                        if (_noiseMap[x + 1, z - 2] == 0)
                            _noiseMap[x + 1, z - 2] = 1;

                        if (_noiseMap[x + 2, z - 2] == 0)
                            _noiseMap[x + 2, z - 2] = 1;
                    }
                }
            }
        }



        return _noiseMap;
    }

    private void removeSingleGroundCaves(ref float[,] _noisemap)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                if (_noisemap[x, z] == 0)
                {
                    int walls = 0;
                    if (x > 0)
                        if (_noisemap[x - 1, z] == 1)
                            walls++;

                    if (z > 0)
                        if (_noisemap[x, z - 1] == 1)
                            walls++;

                    if (x < worldWidth - 1)
                        if (_noisemap[x + 1, z] == 1)
                            walls++;

                    if (z > worldHeight - 1)
                        if (_noisemap[x, z + 1] == 1)
                            walls++;

                    //if all sides are walls replace ground with wall
                    if (walls == 4)
                        _noisemap[x, z] = 1;
                }
            }
        }
    }
    private void removeSingleWalls(int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                
                if (worldMap[x, z].TryGetComponent<Wall>(out  Wall wall))
                {
                    GameObject[] neighbours = findNeighbours(worldMap, x, z);

                    wall.setNeighbours(neighbours);

                    if(wall.neighbours["left"] != null && wall.neighbours["right"] != null)
                        if(wall.neighbours["left"].CompareTag("Ground") && wall.neighbours["right"].CompareTag("Ground"))
                        {
                            Destroy(wall.gameObject);
                            worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                            Pathfinding.addNode(worldMap[x, z].GetComponent<Node>());
                        }

                    if (wall.neighbours["up"] != null && wall.neighbours["down"] != null)
                        if (wall.neighbours["up"].CompareTag("Ground") && wall.neighbours["down"].CompareTag("Ground"))
                        {
                            Destroy(wall.gameObject);
                            worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                            Pathfinding.addNode(worldMap[x, z].GetComponent<Node>());
                        }
                }
            }
        }
    }

    private void createCorners(int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                if (worldMap[x, z].TryGetComponent<Wall>(out Wall wall))
                {
                    wall.setNeighbours(findNeighbours(worldMap, x, z));

                    if (wall.neighbours["up"] != null && wall.neighbours["right"] != null &&
                        wall.neighbours["up"].CompareTag("Ground") && wall.neighbours["right"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[1], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (wall.neighbours["right"] != null && wall.neighbours["down"] != null &&
                        wall.neighbours["right"].CompareTag("Ground") && wall.neighbours["down"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[1], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (wall.neighbours["down"] != null && wall.neighbours["left"] != null &&
                        wall.neighbours["down"].CompareTag("Ground") && wall.neighbours["left"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[1], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    else if (wall.neighbours["left"] != null && wall.neighbours["up"] != null &&
                        wall.neighbours["left"].CompareTag("Ground") && wall.neighbours["up"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[1], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                }
            }
        }
    }

    private void createInnerCorners(int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                if (worldMap[x, z].TryGetComponent<Wall>(out Wall wall))
                {
                    wall.setNeighbours(findNeighbours(worldMap, x, z));

                    if (wall.neighbours["up"] != null && wall.neighbours["upperRight"] != null && wall.neighbours["right"] != null && wall.neighbours["left"] != null &&
                        !wall.neighbours["up"].CompareTag("Ground") && wall.neighbours["upperRight"].CompareTag("Ground") && !wall.neighbours["right"].CompareTag("Ground") && !wall.neighbours["left"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (wall.neighbours["right"] != null && wall.neighbours["lowerRight"] != null && wall.neighbours["down"] != null && wall.neighbours["up"] != null &&
                        !wall.neighbours["right"].CompareTag("Ground") && wall.neighbours["lowerRight"].CompareTag("Ground") && !wall.neighbours["down"].CompareTag("Ground") && !wall.neighbours["up"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (wall.neighbours["down"] != null && wall.neighbours["lowerLeft"] != null && wall.neighbours["left"] != null && wall.neighbours["right"] != null &&
                        !wall.neighbours["down"].CompareTag("Ground") && wall.neighbours["lowerLeft"].CompareTag("Ground") && !wall.neighbours["left"].CompareTag("Ground") && !wall.neighbours["right"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    else if (wall.neighbours["left"] != null && wall.neighbours["upperLeft"] != null && wall.neighbours["up"] != null && wall.neighbours["down"] != null &&
                        !wall.neighbours["left"].CompareTag("Ground") && wall.neighbours["upperLeft"].CompareTag("Ground") && !wall.neighbours["up"].CompareTag("Ground") && !wall.neighbours["down"].CompareTag("Ground"))
                    {
                        Destroy(wall.gameObject);
                        worldMap[x, z] = Instantiate(walls[2], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                        worldMap[x, z].transform.rotation = Quaternion.Euler(0, -0, 0);
                    }
                }


            }
        }
    }

    private void rotateWallsToGround(int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                if (worldMap[x, z].TryGetComponent<Wall>(out Wall wall))
                    if (wall.CompareTag("Wall"))
                    {
                        wall.setNeighbours(findNeighbours(worldMap, x, z));

                        if (wall.neighbours["left"] != null && wall.neighbours["left"].CompareTag("Ground"))
                        {
                            wall.transform.rotation = Quaternion.Euler(0, -90, 0);
                        }
                        else
                        if (wall.neighbours["right"] != null && wall.neighbours["right"].CompareTag("Ground"))
                        {
                            wall.transform.rotation = Quaternion.Euler(0, 90, 0);
                        }
                        else
                        if (wall.neighbours["up"] != null && wall.neighbours["up"].CompareTag("Ground"))
                        {
                            wall.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        if (wall.neighbours["down"] != null && wall.neighbours["down"].CompareTag("Ground"))
                        {
                            wall.transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
            }
        }
    }

    private void createNormalWalls(int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx + 1; x++)
        {
            for (int z = startz; z < endz + 1; z++)
            {
                if (worldMap[x, z].TryGetComponent<Wall>(out Wall wall))
                {
                    wall.setNeighbours(findNeighbours(worldMap, x, z));
                    if (!wall.CompareTag("Wall"))
                    {
                        if (wall.neighbours["up"] != null && wall.neighbours["down"] != null &&
                        !wall.neighbours["up"].CompareTag("Ground") && !wall.neighbours["down"].CompareTag("Ground"))
                        {
                            if (wall.neighbours["left"] != null && wall.neighbours["upperRight"] != null && wall.neighbours["lowerRight"] != null)
                                if (wall.neighbours["left"].CompareTag("Ground") && !wall.neighbours["upperRight"].CompareTag("Ground") && !wall.neighbours["lowerRight"].CompareTag("Ground"))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }
                                else if (wall.neighbours["left"].CompareTag("Ground") && (wall.neighbours["upperRight"].CompareTag("Ground") || wall.neighbours["lowerRight"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }

                            if (wall.neighbours["right"] != null && wall.neighbours["upperLeft"] != null && wall.neighbours["lowerLeft"] != null)
                                if (wall.neighbours["right"].CompareTag("Ground") && !wall.neighbours["upperLeft"].CompareTag("Ground") && !wall.neighbours["lowerLeft"].CompareTag("Ground"))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }
                                else if (wall.neighbours["right"].CompareTag("Ground") && (wall.neighbours["upperleft"].CompareTag("Ground") || wall.neighbours["lowerLeft"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }
                        }

                        if (wall.neighbours["left"] != null && wall.neighbours["right"] != null &&
                        !wall.neighbours["left"].CompareTag("Ground") && !wall.neighbours["right"].CompareTag("Ground"))
                        {
                            if (wall.neighbours["up"] != null && wall.neighbours["lowerLeft"] != null && wall.neighbours["lowerRight"] != null)
                                if (wall.neighbours["up"].CompareTag("Ground") && !wall.neighbours["lowerLeft"].CompareTag("Ground") && !wall.neighbours["lowerRight"].CompareTag("Ground"))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }
                                else if (wall.neighbours["up"].CompareTag("Ground") && (wall.neighbours["lowerLeft"].CompareTag("Ground") || wall.neighbours["lowerRight"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }

                            if (wall.neighbours["down"] != null && wall.neighbours["upperLeft"] != null && wall.neighbours["upperRight"] != null)
                                if (wall.neighbours["down"].CompareTag("Ground") && !wall.neighbours["upperLeft"].CompareTag("Ground") && !wall.neighbours["upperRight"].CompareTag("Ground"))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(walls[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }
                                else if (wall.neighbours["down"].CompareTag("Ground") && (wall.neighbours["upperLeft"].CompareTag("Ground") || wall.neighbours["upperRight"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                }
                        }
                    }
                }
            }
        }
    }

    private void checkCaveCorners(int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx + 1; x++)
        {
            for (int z = startz; z < endz + 1; z++)
            {
                if (worldMap[x, z].TryGetComponent<Wall>(out Wall wall))
                {
                    wall.setNeighbours(findNeighbours(worldMap, x, z));
                    if (!wall.CompareTag("Wall"))
                    {
                        if (wall.neighbours["up"] != null && wall.neighbours["down"] != null &&
                        !wall.neighbours["up"].CompareTag("Ground") && !wall.neighbours["down"].CompareTag("Ground"))
                        {
                            if (wall.neighbours["left"] != null && wall.neighbours["upperRight"] != null && wall.neighbours["lowerRight"] != null)
                                if (wall.neighbours["left"].CompareTag("Ground") && (wall.neighbours["upperRight"].CompareTag("Ground") || wall.neighbours["lowerRight"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                    Pathfinding.addNode(worldMap[x, z].GetComponent<Node>());
                                }

                            if (wall.neighbours["right"] != null && wall.neighbours["upperLeft"] != null && wall.neighbours["lowerLeft"] != null)

                                if (wall.neighbours["right"].CompareTag("Ground") && (wall.neighbours["upperLeft"].CompareTag("Ground") || wall.neighbours["lowerLeft"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                    Pathfinding.addNode(worldMap[x, z].GetComponent<Node>());
                                }
                        }

                        if (wall.neighbours["left"] != null && wall.neighbours["right"] != null &&
                        !wall.neighbours["left"].CompareTag("Ground") && !wall.neighbours["right"].CompareTag("Ground"))
                        {
                            if (wall.neighbours["up"] != null && wall.neighbours["lowerLeft"] != null && wall.neighbours["lowerRight"] != null)
                                if (wall.neighbours["up"].CompareTag("Ground") && (wall.neighbours["lowerLeft"].CompareTag("Ground") || wall.neighbours["lowerRight"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                    Pathfinding.addNode(worldMap[x, z].GetComponent<Node>());
                                }

                            if (wall.neighbours["down"] != null && wall.neighbours["upperLeft"] != null && wall.neighbours["upperRight"] != null)
                                if (wall.neighbours["down"].CompareTag("Ground") && (wall.neighbours["upperLeft"].CompareTag("Ground") || wall.neighbours["upperRight"].CompareTag("Ground")))
                                {
                                    Destroy(wall.gameObject);
                                    worldMap[x, z] = Instantiate(grounds[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                                    Pathfinding.addNode(worldMap[x, z].GetComponent<Node>());
                                }

                        }
                    }
                }
            }
        }
    }

    private GameObject[] findNeighbours(GameObject[,] worldMap, int x, int z)
    {
        GameObject[] neighbours = new GameObject[8];
        //left
        if (x > 0)
            neighbours[0] = worldMap[x - 1, z];
        else
            neighbours[0] = null;
        //right
        if (x < worldWidth - 1)
            neighbours[1] = worldMap[x + 1, z];
        else
            neighbours[1] = null;
        //up
        if (z < worldHeight - 1)
            neighbours[2] = worldMap[x, z + 1];
        else
            neighbours[2] = null;
        //down
        if (z > 0)
            neighbours[3] = worldMap[x, z - 1];
        else
            neighbours[3] = null;

        //top left
        if (x > 0 && z < worldHeight - 1)
            neighbours[4] = worldMap[x - 1, z + 1];
        else
            neighbours[4] = null;

        //Top right
        if (x < worldWidth - 1 && z < worldHeight - 1)
            neighbours[5] = worldMap[x + 1, z + 1];
        else
            neighbours[5] = null;

        //down left
        if (x > 0 && z > 0)
            neighbours[6] = worldMap[x - 1, z - 1];
        else
            neighbours[6] = null;
        //down right
        if (x < worldWidth - 1 && z > 0)
            neighbours[7] = worldMap[x + 1, z - 1];
        else
            neighbours[7] = null;


        return neighbours;
    }

    public void replaceWorldObject(GameObject oldObj, string newObj)
    {

        GameObject replaceObject = null;
        int replaceX = 0;
        int replaceZ = 0;

        foreach (GameObject g in walls)
            if (g.name == newObj)
                replaceObject = g;

        if (replaceObject == null)
            foreach (GameObject g in grounds)
                if (g.name == newObj)
                    replaceObject = g;

        if (replaceObject == null)
            return;

        replaceX = (int)oldObj.transform.position.x;
        replaceZ = (int)oldObj.transform.position.z;

        Destroy(oldObj);

        worldMap[replaceX, replaceZ] = Instantiate(replaceObject, new Vector3(replaceX, 0, replaceZ), Quaternion.identity, this.transform);

        if (worldMap[replaceX, replaceZ].TryGetComponent<Ground>(out Ground ground))
        {
            ground.setNeighbours(findNeighbours(worldMap, replaceX, replaceZ));
            Pathfinding.addNode(ground.GetComponent<Node>());
        }

        else if (worldMap[replaceX, replaceZ].TryGetComponent<Wall>(out Wall wall))
            wall.setNeighbours(findNeighbours(worldMap, replaceX, replaceZ));




        //remove the single standing walls
        removeSingleWalls(replaceX - 2, replaceZ - 2, replaceX + 2, replaceZ + 2);

        //check for cave corners
        checkCaveCorners(replaceX - 2, replaceZ - 2, replaceX + 2, replaceZ + 2);

        //create normal walls
        createNormalWalls(replaceX - 2, replaceZ - 2, replaceX + 2, replaceZ + 2);

        //rotate normal walls
        rotateWallsToGround(replaceX - 1, replaceZ - 1, replaceX + 2, replaceZ + 2);

        //create normal corners
        createCorners(0, 0, worldWidth, worldHeight);

        //create inner corners
        createInnerCorners(0, 0, worldWidth, worldHeight);

        //remove the single standing walls
        removeSingleWalls(replaceX - 2, replaceZ - 2, replaceX + 2, replaceZ + 2);

        //create normal walls
        createNormalWalls(replaceX - 2, replaceZ - 2, replaceX + 2, replaceZ + 2);


        Pathfinding.checkForNewConnections();
    }



    public List<Ground> getAllGroundObjects()
    {
        foreach (GameObject go in worldMap)
            if (go.TryGetComponent<Ground>(out Ground g))
                if (!groundObjects.Contains(g))
                    groundObjects.Add(g);

        return groundObjects;
    }

    public List<Wall> getAllWallObjects()
    {
        foreach (GameObject go in worldMap)
            if (go.TryGetComponent<Wall>(out Wall w))
                if (!wallObjects.Contains(w))
                    wallObjects.Add(w);
        return wallObjects;
    }

    public List<Node> getAllNodeObjects()
    {
        foreach (GameObject go in worldMap)
            if (go.TryGetComponent<Node>(out Node n))
                if (!nodeObjects.Contains(n))
                    nodeObjects.Add(n);
        return nodeObjects;
    }
}
