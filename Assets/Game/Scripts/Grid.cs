using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridsArray;
    private Vector3 spawnedPosition;

    //Call grid object.
    public Grid(int width, int height, float cellSize, Vector3 spawnPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridsArray = new int[width, height];

        Debug.Log(width + " : " + height);

        for (int i = 0; i < gridsArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridsArray.GetLength(1); j++)
            {
                Debug.DrawLine(spawnPos + GetWorldPosition(i, j), spawnPos + GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(spawnPos + GetWorldPosition(i, j), spawnPos + GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }
        Debug.DrawLine(spawnPos + GetWorldPosition(0, height), spawnPos + GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(spawnPos + GetWorldPosition(width, 0), spawnPos + GetWorldPosition(width, height), Color.white, 100f);
        spawnedPosition = spawnPos;
    }
    //Get grid mid point for square spawns.
    public Vector3 GetGridMidPoint(Vector3 worldPos)
    {
        int x,y;
        GetXY(worldPos, out x, out y);
        return GetWorldPosition(x, y) + (new Vector3(cellSize , cellSize) * .5f) + spawnedPosition;
    }
    //Get world position.
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize;
    }
    //Get XY coord
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }
    //Set value.
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x,y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
    private void SetValue(int x, int y, int value)
    {
        //Set value if not -1.
        if(GetValue(x,y) > -1)
        gridsArray[x, y] = value;
    }
    //Get value.
    public float GetValue(Vector3 worldPosition)
    {
        int x,y;
        GetXY(worldPosition, out x, out y);
        //Debug.Log("x : " + x + "y : " + y);
        return GetValue(x, y);
    }
    private float GetValue(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridsArray[x , y];
        }
        else
        {
            return -1;
        }
    }
}
