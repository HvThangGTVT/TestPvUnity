using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games._Scripts.Core.Utis;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    [SerializeField] List<GameObject> playerPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 100;
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 0.1f;

    List<GameObject> _poolObjects = new List<GameObject>();

    void Awake()
    {
        _poolObjects.Clear();
        PopulatePool(poolSize);
    }

    List<GameObject> PopulatePool(int amount)
    {
        List<GameObject> listNew = new List<GameObject>();
        List<int> teams = new List<int>();
        var countLeft = amount;
        for (int i = 0; i < playerPrefab.Count; i++)
        {
            if (countLeft > 0)
            {
                var tmpCount = Random.Range(0, countLeft);
                if (tmpCount > 0 && i < playerPrefab.Count - 1)
                {
                    teams.Add(tmpCount);
                }

                if (i == playerPrefab.Count - 1)
                {
                    teams.Add(countLeft);
                }

                countLeft = countLeft - tmpCount;
            }
        }

        for (int i = 0; i < teams.Count; i++)
        {
            for (int j = 0; j < teams[i]; j++)
            {
                var tmp = Instantiate(playerPrefab[i], transform.position, Quaternion.identity);
                _poolObjects.Add(tmp);
                listNew.Add(tmp);
                tmp.SetActive(false);
            }
        }


        RandomListUtil(_poolObjects);
        return listNew;
    }


    public void Spawn(int amount)
    {
        StartCoroutine(SpawnPri(amount));
    }

    private Dictionary<GameObject, bool> _listUsed = new Dictionary<GameObject, bool>();

    private IEnumerator SpawnPri(int amount)
    {
        var tmp = amount;
        var listAvailable = new List<GameObject>();

        for (int i = 0; i < _poolObjects.Count; i++)
        {
            if (!_poolObjects[i].activeSelf)
            {
                if (_listUsed.ContainsKey(_poolObjects[i]))
                {
                    if (!_listUsed[_poolObjects[i]])
                    {
                        listAvailable.Add(_poolObjects[i]);
                    }
                }
                else
                {
                    listAvailable.Add(_poolObjects[i]);
                }
            }
        }

        if (tmp > listAvailable.Count)
        {
            var exted = PopulatePool(tmp - listAvailable.Count);
            listAvailable.AddRange(exted);
        }

        for (int i = 0; i < tmp; i++)
        {
            if (_listUsed.ContainsKey(listAvailable[i]))
            {
                _listUsed[listAvailable[i]] = true;
            }
            else
            {
                _listUsed.Add(listAvailable[i], true);
            }
        }


        for (int i = 0; i < tmp && i < listAvailable.Count; i++)
        {
            yield return new WaitForSeconds(0.7f);
            listAvailable[i].SetActive(true);
        }
    }


    private void RandomListUtil(List<GameObject> list)
    {
        var ranL = new System.Random();
        list.OrderBy(a => ranL.Next()).ToList();
    }

    public void UnUsed(GameObject player)
    {
        if (_listUsed.ContainsKey(player))
        {
            _listUsed[player] = false;
        }
    }
}