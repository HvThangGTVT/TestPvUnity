using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private GridManager _gridManager;
    private Pathfinder _pathfinder;

    void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (_pathfinder != null && _gridManager != null)
        {
            _gridManager.GetPositionFromCoordinates(_pathfinder.StartCoordinates);
        }
    }


    [Button]
    private void MakePlayer()
    {
        ObjectPool.Instance.Spawn(10);
    }


    public void AddCharacters(int charactersToAdd)
    {
        ObjectPool.Instance.Spawn(charactersToAdd);
    }
}