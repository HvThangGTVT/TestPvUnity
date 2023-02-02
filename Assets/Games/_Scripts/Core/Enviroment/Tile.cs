using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;

    public bool IsPlaceable
    {
        get { return isPlaceable; }
    }

    GridManager gridManager;
    Vector2Int coordinates = new Vector2Int();

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }
}