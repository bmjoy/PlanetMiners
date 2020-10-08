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
        removeSingleWalls(ref worldMap, 0, 0, worldWidth, worldHeight);

        //rotate normal walls
        rotateWallsToGround(ref worldMap, 0, 0, worldWidth, worldHeight);

        //create normal corners
        createCorners(0, 0, worldWidth, worldHeight);

        //create inner corners
        createInnerCorners(ref worldMap, 0, 0, worldWidth, worldHeight);
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
    private void removeSingleWalls(ref GameObject[,] worldmap, int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    GameObject[] neighbours = findNeighbours(worldMap, x, z);

                    g.setNeighbours(neighbours);

                    //if ground has wall on left side
                    if (g.neighbours["left"] != null && !g.neighbours["left"].CompareTag("Ground"))
                    {
                        Wall w = g.neighbours["left"].GetComponent<Wall>();
                        //find left walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x - 1, z));
                        //if wall doesnt have left and right neighbour walls then destroy wall
                        if (w.neighbours["left"] != null && w.neighbours["left"].CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x - 1, z] = Instantiate(grounds[0], new Vector3(x - 1, 0, z), Quaternion.identity, this.transform);
                            Pathfinding.addNode(worldmap[x - 1, z].GetComponent<Node>());
                        }
                    }

                    //if ground has wall on right side
                    if (g.neighbours["right"] != null && !g.neighbours["right"].CompareTag("Ground"))
                    {
                        Wall w = g.neighbours["right"].GetComponent<Wall>();
                        //find right walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x + 1, z));
                        //if wall doesnt have left and right neighbour walls then destroy wall
                        if (w.neighbours["right"] != null && w.neighbours["right"].CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x + 1, z] = Instantiate(grounds[0], new Vector3(x + 1, 0, z), Quaternion.identity, this.transform);
                            Pathfinding.addNode(worldmap[x + 1, z].GetComponent<Node>());

                        }
                    }

                    //if ground has wall on up side
                    if (g.neighbours["up"] != null && !g.neighbours["up"].CompareTag("Ground"))
                    {
                        Wall w = g.neighbours["up"].GetComponent<Wall>();
                        //find up walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x, z + 1));
                        //if wall doesnt have up and down neighbour walls then destroy wall
                        if (w.neighbours["up"] != null && w.neighbours["up"].CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x, z + 1] = Instantiate(grounds[0], new Vector3(x, 0, z + 1), Quaternion.identity, this.transform);
                            Pathfinding.addNode(worldmap[x, z + 1].GetComponent<Node>());

                        }
                    }

                    //if ground has wall on down side
                    if (g.neighbours["down"] != null && !g.neighbours["down"].CompareTag("Ground"))
                    {
                        Wall w = g.neighbours["down"].GetComponent<Wall>();
                        //find up walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x, z - 1));
                        //if wall doesnt have up and down neighbour walls then destroy wall
                        if (w.neighbours["down"] != null && w.neighbours["down"].CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x, z - 1] = Instantiate(grounds[0], new Vector3(x, 0, z - 1), Quaternion.identity, this.transform);
                            Pathfinding.addNode(worldmap[x, z - 1].GetComponent<Node>());

                        }
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

    private void createInnerCorners(ref GameObject[,] worldmap, int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    GameObject[] neighbours = findNeighbours(worldMap, x, z);

                    g.setNeighbours(neighbours);

                    //left wall
                    if (g.neighbours["left"] != null && g.neighbours["left"].CompareTag("Wall") || g.neighbours["left"].CompareTag("WallCorner"))
                    {
                        //upper wall
                        if (g.neighbours["up"] != null && g.neighbours["up"].CompareTag("Wall") || g.neighbours["up"].CompareTag("WallCorner"))
                        {
                            //upper left wall to corner inner
                            if (g.neighbours["upperLeft"] != null && g.neighbours["upperLeft"].CompareTag("Wall"))
                            {
                                Destroy(g.neighbours["upperLeft"]);
                                worldMap[x - 1, z + 1] = Instantiate(walls[2], new Vector3(x - 1, 0, z + 1), Quaternion.identity, this.transform);
                                worldMap[x - 1, z + 1].transform.Rotate(Vector3.up, 180f);

                            }
                        }
                        //down wall
                        if (g.neighbours["down"] != null && g.neighbours["down"].CompareTag("Wall") || g.neighbours["down"].CompareTag("WallCorner"))
                        {
                            //lower left wall to corner inner
                            if (g.neighbours["lowerLeft"] != null && g.neighbours["lowerLeft"].CompareTag("Wall"))
                            {
                                Destroy(g.neighbours["lowerLeft"]);
                                worldMap[x - 1, z - 1] = Instantiate(walls[2], new Vector3(x - 1, 0, z - 1), Quaternion.identity, this.transform);
                                worldMap[x - 1, z - 1].transform.Rotate(Vector3.up, 90f);
                            }
                        }
                    }

                    //right wall
                    if (g.neighbours["right"] != null && g.neighbours["right"].CompareTag("Wall") || g.neighbours["right"].CompareTag("WallCorner"))
                    {
                        //up wall
                        if (g.neighbours["up"] != null && g.neighbours["up"].CompareTag("Wall") || g.neighbours["up"].CompareTag("WallCorner"))
                        {
                            //upper right wall to corner inner
                            if (g.neighbours["upperRight"] != null && g.neighbours["upperRight"].CompareTag("Wall"))
                            {
                                Destroy(g.neighbours["upperRight"]);
                                worldMap[x + 1, z + 1] = Instantiate(walls[2], new Vector3(x + 1, 0, z + 1), Quaternion.identity, this.transform);
                                worldMap[x + 1, z + 1].transform.Rotate(Vector3.up, -90f);
                            }
                        }
                        //down wall
                        if (g.neighbours["down"] != null && g.neighbours["down"].CompareTag("Wall") || g.neighbours["down"].CompareTag("WallCorner"))
                        {
                            //lower right wall to corner inner
                            if (g.neighbours["lowerRight"] != null && g.neighbours["lowerRight"].CompareTag("Wall"))
                            {
                                Destroy(g.neighbours["lowerRight"]);
                                worldMap[x + 1, z - 1] = Instantiate(walls[2], new Vector3(x + 1, 0, z - 1), Quaternion.identity, this.transform);
                                worldMap[x + 1, z - 1].transform.Rotate(Vector3.up, 0f);
                            }
                        }


                    }

                }

            }
        }
    }

    private void rotateWallsToGround(ref GameObject[,] worldmap, int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    g.setNeighbours(findNeighbours(worldmap, x, z));

                    //rotate walls left from ground
                    if (g.neighbours["left"] != null && g.neighbours["left"].CompareTag("Wall"))
                    {
                        g.neighbours["left"].transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    //rotate walls right from ground
                    if (g.neighbours["right"] != null && g.neighbours["right"].CompareTag("Wall"))
                    {
                        g.neighbours["right"].transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    //rotate walls up from ground
                    if (g.neighbours["up"] != null && g.neighbours["up"].CompareTag("Wall"))
                    {
                        g.neighbours["up"].transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                }
            }
        }
    }

    private void createNormalWalls(ref GameObject[,] worldmap, int startx, int startz, int endx, int endz)
    {
        for (int x = startx; x < endx; x++)
        {
            for (int z = startz; z < endz; z++)
            {
                if (worldMap[x, z].TryGetComponent<Ground>(out Ground g))
                {
                    GameObject[] neighbours = findNeighbours(worldMap, x, z);

                    g.setNeighbours(neighbours);

                    //left
                    if (g.neighbours["upperLeft"] != null && g.neighbours["left"] != null && g.neighbours["lowerLeft"] != null)
                        if (!g.neighbours["upperLeft"].CompareTag("Ground") && !g.neighbours["left"].CompareTag("Wall") && !g.neighbours["left"].CompareTag("Ground") && !g.neighbours["lowerLeft"].CompareTag("Ground"))
                        {
                            Destroy(g.neighbours["left"].gameObject);
                            worldmap[x - 1, z] = Instantiate(walls[0], new Vector3(x - 1, 0, z), Quaternion.identity, this.transform);
                        }
                    //right
                    if (g.neighbours["upperRight"] != null && g.neighbours["right"] != null && g.neighbours["lowerRight"] != null)
                        if (!g.neighbours["upperRight"].CompareTag("Ground") && !g.neighbours["right"].CompareTag("Wall") && !g.neighbours["right"].CompareTag("Ground") && !g.neighbours["lowerRight"].CompareTag("Ground"))
                        {
                            Destroy(g.neighbours["right"].gameObject);
                            worldmap[x + 1, z] = Instantiate(walls[0], new Vector3(x + 1, 0, z), Quaternion.identity, this.transform);
                        }
                    //up
                    if (g.neighbours["upperLeft"] != null && g.neighbours["up"] != null && g.neighbours["upperRight"] != null)
                        if (!g.neighbours["upperLeft"].CompareTag("Ground") && !g.neighbours["up"].CompareTag("Wall") && !g.neighbours["up"].CompareTag("Ground") && !g.neighbours["upperRight"].CompareTag("Ground"))
                        {
                            Destroy(g.neighbours["up"].gameObject);
                            worldmap[x, z + 1] = Instantiate(walls[0], new Vector3(x, 0, z + 1), Quaternion.identity, this.transform);
                        }
                    //down
                    if (g.neighbours["lowerLeft"] != null && g.neighbours["down"] != null && g.neighbours["lowerRight"] != null)
                        if (!g.neighbours["lowerLeft"].CompareTag("Ground") && !g.neighbours["down"].CompareTag("Wall") && !g.neighbours["down"].CompareTag("Ground") && !g.neighbours["lowerRight"].CompareTag("Ground"))
                        {
                            Destroy(g.neighbours["down"].gameObject);
                            worldmap[x, z - 1] = Instantiate(walls[0], new Vector3(x, 0, z - 1), Quaternion.identity, this.transform);
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
        removeSingleWalls(ref worldMap, replaceX - 1, replaceZ - 1, replaceX + 1, replaceZ + 1);

        //create normal walls
        createNormalWalls(ref worldMap, replaceX - 1, replaceZ - 1, replaceX + 1, replaceZ + 1);

        //rotate normal walls
        rotateWallsToGround(ref worldMap, replaceX - 1, replaceZ - 1, replaceX + 1, replaceZ + 1);

        //create normal corners
        createCorners(0, 0, worldWidth, worldHeight);

        //create inner corners
        createInnerCorners(ref worldMap, 0, 0, worldWidth, worldHeight);


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
