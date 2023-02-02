using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] private float gridCellRadius;
    [SerializeField] private float gridCellWidth, gridCellHeight;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public Dictionary<Vector2Int, Node> Grid
    {
        get { return grid; }
    }

    void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();

        foreach (var node in Grid)
        {
            if (Vector3.Distance(node.Value.position, position) < gridCellRadius)
            {
                coordinates = node.Key;
                break;
            }
        }

        return coordinates;
    }
    
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = transform.position.x + coordinates.x * gridCellWidth + (gridCellWidth / 2);
        position.y = transform.position.y + coordinates.y * gridCellHeight + (gridCellHeight / 2);
        return position;
    }
    
    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true, GetPositionFromCoordinates(coordinates)));
            }
        }
    }


    #region EDITOR

    private void OnDrawGizmos()
    {
        DebugDrawGrid(transform.position, gridSize.x, gridSize.y, gridCellWidth, gridCellHeight, Color.blue);
    }

    public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellWidth, float cellHeight, Color color)
    {
        float width = (numCols * cellWidth);
        float height = (numRows * cellHeight);

        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellWidth * new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(1.0f, 0.0f, 0.0f);

            Debug.DrawLine(startPos, endPos, color);
        }

        for (int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * cellHeight * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + width * new Vector3(0.0f, 1.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }

    #endregion

    [Button]
    private void MakeObstacleView()
    {
        foreach (var tmp in grid)
        {
            if (!tmp.Value.isWalkable)
            {
                //Instantiate(cc, tmp.Value.position, Quaternion.identity);
            }
        }
    }
}