using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratePerlinMap
{

    public static float[,] generateMap(int width, int height,float scale,float xoffset,float zoffset)
    {
        float[,] noiseMap = new float[width, height];

        float xcoord, zcoord;

        for(int x = 0; x < width;x++)
        {
            for (int z = 0; z < height; z++){

                xcoord = (float)x / width * scale + xoffset;
                zcoord = (float)z / width * scale + zoffset;

                noiseMap[x,z] = Mathf.PerlinNoise(xcoord, zcoord);
            }
        }

        return noiseMap;
    }
}
