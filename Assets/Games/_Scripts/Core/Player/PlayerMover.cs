using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    [SerializeField] private AnimationController animationController;

    List<Node> _path = new List<Node>();

    GridManager _gridManager;
    Pathfinder _pathfinder;
    private Coroutine _updateAnimation;

    Node _previousNode;
    Node _currentNode;

    void OnEnable()
    {
        _previousNode = null;
        _currentNode = null;
        ReturnToStart();
        RecalculatePath(true);

        if (_updateAnimation != null)
        {
            StopCoroutine(_updateAnimation);
        }
    }

    void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();
    }


    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = _pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        _path.Clear();
        _path = _pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathfinder.StartCoordinates);
    }

    void FinishPath()
    {
        ObjectPool.Instance.UnUsed(gameObject);
        gameObject.SetActive(false);
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < _path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            _currentNode = _gridManager.GetNode(_gridManager.GetCoordinatesFromPosition(transform.position));
            _currentNode.isWalkable = false;
            Vector3 endPosition = _gridManager.GetPositionFromCoordinates(_path[i].coordinates);
            float travelPercent = 0f;
            startPosition.z = 0;
            endPosition.z = 0;
            animationController.StartMoveAnimation((endPosition - startPosition).normalized);
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

            _currentNode.isWalkable = true;
        }

        if (_currentNode != null)
        {
            _currentNode.isWalkable = true;
        }

        FinishPath();
    }
}