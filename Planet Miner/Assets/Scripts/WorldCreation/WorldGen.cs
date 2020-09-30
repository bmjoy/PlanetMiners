using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    [Header("World size")]
    public int worldWidth = 10;
    public int worldHeight = 10;
    public float scale = 1;
    public float offsetX = 1;
    public float offsetZ = 1;

    private GameObject[,] worldMap;
    private float[,] noiseMap;

    [Header("Terain Objects")]
    public GameObject[] walls;
    public GameObject[] ground;

    [Header("Spawn Range Terrain")]
    [Range(0, 1f)]
    public float groundRange;

    [Space()]
    [Header("Spawn chance for crystals")]
    [Range(0, 1)]
    public float chanceCrystal = 0f;

    private void Start()
    {
        
        generateWorld();
    }

    public void generateWorld()
    {
        offsetX = Random.Range(0, 1000);
        offsetZ = Random.Range(0, 1000);

        worldWidth = PlayerPrefs.GetInt("WorldWidth");
        worldHeight = PlayerPrefs.GetInt("WorldHeight");

        worldMap = new GameObject[worldWidth, worldHeight];

        noiseMap = GeneratePerlinMap.generateMap(worldWidth, worldHeight, scale, offsetX, offsetZ);

        noiseMap = convertNoiseMapToWorld(noiseMap);
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
                }else if(noiseMap[x,z] == 2)
                {
                    worldMap[x, z] = Instantiate(walls[3], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                }
                //ground
                else
                {
                    worldMap[x, z] = Instantiate(ground[0], new Vector3(x, 0, z), Quaternion.identity, this.transform);
                }
            }
        }
        //remove single ground caves
        removeSingleGroundCaves(ref noiseMap);

        //remove lonely walls
        removeSingleWalls(ref worldMap);

        //rotate normal walls
        rotateWallsToGround(ref worldMap);

        //create normal corners
        createCorners(ref worldMap);

        //create inner corners
        createInnerCorners(ref worldMap);

        //create all nodes and connections for pathfinding
        setupPathfinding(ref worldMap);
    }
    private float[,] convertNoiseMapToWorld(float[,] _noiseMap)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                if (_noiseMap[x, z] <= groundRange)
                    _noiseMap[x, z] = 0;
                else
                
                    if (Random.Range(0, 2f) <= chanceCrystal)
                        _noiseMap[x, z] = 2;
                    else
                        _noiseMap[x, z] = 1;
                

            }
        }

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
    private void removeSingleWalls(ref GameObject[,] worldmap)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    GameObject[] neighbours = findNeighbours(worldMap, x, z);

                    g.setNeighbours(neighbours);

                    //if ground has wall on left side
                    if (g.leftNeighbour != null && g.leftNeighbour.CompareTag("Wall"))
                    {
                        Wall w = g.leftNeighbour.GetComponent<Wall>();
                        //find left walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x - 1, z));
                        //if wall doesnt have left and right neighbour walls then destroy wall
                        if (w.leftNeighbour != null && w.leftNeighbour.CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x - 1, z] = Instantiate(ground[0], new Vector3(x - 1, 0, z), Quaternion.identity, this.transform);
                        }
                    }

                    //if ground has wall on right side
                    if (g.rightNeighbour != null && g.rightNeighbour.CompareTag("Wall"))
                    {
                        Wall w = g.rightNeighbour.GetComponent<Wall>();
                        //find right walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x + 1, z));
                        //if wall doesnt have left and right neighbour walls then destroy wall
                        if (w.rightNeighbour != null && w.rightNeighbour.CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x + 1, z] = Instantiate(ground[0], new Vector3(x + 1, 0, z), Quaternion.identity, this.transform);
                        }
                    }

                    //if ground has wall on up side
                    if (g.upNeighbour != null && g.upNeighbour.CompareTag("Wall"))
                    {
                        Wall w = g.upNeighbour.GetComponent<Wall>();
                        //find up walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x, z + 1));
                        //if wall doesnt have up and down neighbour walls then destroy wall
                        if (w.upNeighbour != null && w.upNeighbour.CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x, z + 1] = Instantiate(ground[0], new Vector3(x, 0, z + 1), Quaternion.identity, this.transform);
                        }
                    }

                    //if ground has wall on down side
                    if (g.downNeighbour != null && g.downNeighbour.CompareTag("Wall"))
                    {
                        Wall w = g.downNeighbour.GetComponent<Wall>();
                        //find up walls neighbours
                        w.setNeighbours(findNeighbours(worldmap, x, z - 1));
                        //if wall doesnt have up and down neighbour walls then destroy wall
                        if (w.downNeighbour != null && w.downNeighbour.CompareTag("Ground"))
                        {
                            Destroy(w.gameObject);
                            worldmap[x, z - 1] = Instantiate(ground[0], new Vector3(x, 0, z - 1), Quaternion.identity, this.transform);
                        }
                    }
                }
            }
        }
    }

    private void createCorners(ref GameObject[,] worldmap)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    g.setNeighbours(findNeighbours(worldmap, x, z));

                    //corner up
                    if (g.upperleftNeighbour != null && g.upNeighbour != null && g.upperrightNeighbour != null)
                    {
                        if (!g.upperleftNeighbour.CompareTag("Ground") && !g.upNeighbour.CompareTag("Ground") && g.upperrightNeighbour.CompareTag("Ground"))
                        {
                            Destroy(g.upNeighbour);
                            worldMap[x, z + 1] = Instantiate(walls[1], new Vector3(x, 0, z + 1), Quaternion.identity, this.transform);
                            worldMap[x, z + 1].transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }

                    //corner up inv
                    if (g.upperleftNeighbour != null && g.upNeighbour != null && g.upperrightNeighbour != null)
                    {
                        if (g.upperleftNeighbour.CompareTag("Ground") && !g.upNeighbour.CompareTag("Ground") && !g.upperrightNeighbour.CompareTag("Ground"))
                        {
                            Destroy(g.upNeighbour);
                            worldMap[x, z + 1] = Instantiate(walls[1], new Vector3(x, 0, z + 1), Quaternion.identity, this.transform);
                            worldMap[x, z + 1].transform.rotation = Quaternion.Euler(0, -90, 0);
                        }
                    }


                    //corner down
                    if (g.lowerleftNeighbour != null && g.downNeighbour != null && g.lowerrightNeighbour != null)
                    {
                        if (g.lowerleftNeighbour.CompareTag("Ground") && !g.downNeighbour.CompareTag("Ground") && !g.lowerrightNeighbour.CompareTag("Ground"))
                        {
                            Destroy(g.downNeighbour);
                            worldMap[x, z - 1] = Instantiate(walls[1], new Vector3(x, 0, z - 1), Quaternion.identity, this.transform);
                            worldMap[x, z - 1].transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }

                    //corner down inv
                    if (g.lowerleftNeighbour != null && g.downNeighbour != null && g.lowerrightNeighbour != null)
                    {
                        if (!g.lowerleftNeighbour.CompareTag("Ground") && !g.downNeighbour.CompareTag("Ground") && g.lowerrightNeighbour.CompareTag("Ground"))
                        {
                            Destroy(g.downNeighbour);
                            worldMap[x, z - 1] = Instantiate(walls[1], new Vector3(x, 0, z - 1), Quaternion.identity, this.transform);
                            worldMap[x, z - 1].transform.rotation = Quaternion.Euler(0, 90, 0);
                        }
                    }
                }
            }
        }
    }

    private void createInnerCorners(ref GameObject[,] worldmap)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    GameObject[] neighbours = findNeighbours(worldMap, x, z);

                    g.setNeighbours(neighbours);

                    //left wall
                    if (g.leftNeighbour != null && g.leftNeighbour.CompareTag("Wall") || g.leftNeighbour.CompareTag("WallCorner"))
                    {
                        //upper wall
                        if (g.upNeighbour != null && g.upNeighbour.CompareTag("Wall") || g.upNeighbour.CompareTag("WallCorner"))
                        {
                            //upper left wall to corner inner
                            if (g.upperleftNeighbour != null && g.upperleftNeighbour.CompareTag("Wall"))
                            {
                                Destroy(g.upperleftNeighbour);
                                worldMap[x - 1, z + 1] = Instantiate(walls[2], new Vector3(x - 1, 0, z + 1), Quaternion.identity, this.transform);
                                worldMap[x - 1, z + 1].transform.Rotate(Vector3.up, 180f);

                            }
                        }
                        //down wall
                        if (g.downNeighbour != null && g.downNeighbour.CompareTag("Wall") || g.downNeighbour.CompareTag("WallCorner"))
                        {
                            //lower left wall to corner inner
                            if (g.lowerleftNeighbour != null && g.lowerleftNeighbour.CompareTag("Wall"))
                            {
                                Destroy(g.lowerleftNeighbour);
                                worldMap[x - 1, z - 1] = Instantiate(walls[2], new Vector3(x - 1, 0, z - 1), Quaternion.identity, this.transform);
                                worldMap[x - 1, z - 1].transform.Rotate(Vector3.up, 90f);
                            }
                        }
                    }

                    //right wall
                    if (g.rightNeighbour != null && g.rightNeighbour.CompareTag("Wall") || g.rightNeighbour.CompareTag("WallCorner"))
                    {
                        //up wall
                        if (g.upNeighbour != null && g.upNeighbour.CompareTag("Wall") || g.upNeighbour.CompareTag("WallCorner"))
                        {
                            //upper right wall to corner inner
                            if (g.upperrightNeighbour != null && g.upperrightNeighbour.CompareTag("Wall"))
                            {
                                Destroy(g.upperrightNeighbour);
                                worldMap[x + 1, z + 1] = Instantiate(walls[2], new Vector3(x + 1, 0, z + 1), Quaternion.identity, this.transform);
                                worldMap[x + 1, z + 1].transform.Rotate(Vector3.up, -90f);
                            }
                        }
                        //down wall
                        if (g.downNeighbour != null && g.downNeighbour.CompareTag("Wall") || g.downNeighbour.CompareTag("WallCorner"))
                        {
                            //lower right wall to corner inner
                            if (g.lowerrightNeighbour != null && g.lowerrightNeighbour.CompareTag("Wall"))
                            {
                                Destroy(g.lowerrightNeighbour);
                                worldMap[x + 1, z - 1] = Instantiate(walls[2], new Vector3(x + 1, 0, z - 1), Quaternion.identity, this.transform);
                                worldMap[x + 1, z - 1].transform.Rotate(Vector3.up, 0f);
                            }
                        }


                    }

                }

            }
        }
    }

    private void rotateWallsToGround(ref GameObject[,] worldmap)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                Ground g;
                if (worldMap[x, z].TryGetComponent<Ground>(out g))
                {
                    g.setNeighbours(findNeighbours(worldmap, x, z));

                    //rotate walls left from ground
                    if (g.leftNeighbour != null && g.leftNeighbour.CompareTag("Wall"))
                    {
                        g.leftNeighbour.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    //rotate walls right from ground
                    if (g.rightNeighbour != null && g.rightNeighbour.CompareTag("Wall"))
                    {
                        g.rightNeighbour.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    //rotate walls up from ground
                    if (g.upNeighbour != null && g.upNeighbour.CompareTag("Wall"))
                    {
                        g.upNeighbour.transform.rotation = Quaternion.Euler(0, 180, 0);
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

    private void setupPathfinding(ref GameObject[,] worldMap)
    {
        
    }

    void clearWorld()
    {
        foreach (GameObject g in worldMap)
            Destroy(g);

        worldMap = new GameObject[worldWidth, worldHeight];
    }
}
