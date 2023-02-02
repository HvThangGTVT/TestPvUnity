using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node connectedTo;
    public Vector3 position;


    public Node(Vector2Int coordinates, bool isWalkable, Vector3 pos)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
        this.position = pos;
    }
}