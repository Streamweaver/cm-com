using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;

    private GridSquare[,] gridSquareArray;

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        this.gridSquareArray = new GridSquare[width, height];


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridSquareArray[x, z] = new GridSquare(this, new GridPosition(x, z));
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.X, 0, gridPosition.Z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize), 
            Mathf.RoundToInt(worldPosition.z / cellSize)
            );
    }

    public GridSquare GetGridSquare(GridPosition gridPosition)
    {
        return gridSquareArray[gridPosition.X, gridPosition.Z];
    }

    public void CreateDebugObjects(Transform debugPrefab, Transform parent)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition) + new Vector3(0f, 0.01f, 0), Quaternion.identity, parent);
                GridDebugSquare gridDebugSquare = debugTransform.GetComponent<GridDebugSquare>();
                gridDebugSquare.SetGridSquare(GetGridSquare(gridPosition));
            }
        }
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.X >= 0 &&
               gridPosition.Z >= 0 &&
               gridPosition.X < width &&
               gridPosition.Z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
