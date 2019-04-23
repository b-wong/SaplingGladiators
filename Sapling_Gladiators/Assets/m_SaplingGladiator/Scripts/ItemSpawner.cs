using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Spawnable
    {
        public GameObject item;
        [Range(0, 100)]
        public int probability;
    }
    public Spawnable[] items;
    public float spawnWidth;
    [Range(0, 20)]
    public int minVerticalDist;
    public int maxSpawnHeight;
    public static int itemSpawnProbability = 25;
    public Transform itemParent;

    private void Start()
    {
        for (int i = 0; i < maxSpawnHeight; i++)
        {
            if (Random.Range(0f, 100f) < itemSpawnProbability)//spawn an item
            {
                SpawnItem(i);
                i += minVerticalDist - 1;
            }
        }
    }

    private void SpawnItem(int _verticalPosition)
    {
        //?Debug.Log("---------------------------");
        Vector2 _spawnPosition = new Vector2(Random.Range(-spawnWidth / 2, spawnWidth / 2), _verticalPosition);
        int _totalProbability = 0;
        foreach (var spawnable in items)// find total probability
        {
            _totalProbability += spawnable.probability;
        }
        //?Debug.Log("Total probability is: "+_totalProbability);
        int _result = Random.Range(0, _totalProbability+1);// pick a number within the total probability range
        //?Debug.Log("Random pick is: " + _result);
        float _temp = 0;
        int _selection = 0;
        for (int i = 0; i < items.Length; i++)// determine which item will be spawned
        {
            if (_temp + items[i].probability < _result)
            {
                _temp += items[i].probability;
            }
            else
            {
                _selection = i;
                break;
            }
        }
        //?Debug.Log("Selected item is: " + _selection);
        Instantiate(items[_selection].item, _spawnPosition, Quaternion.identity, itemParent);// spawn item
        //?Debug.Log("---------------------------");
    }
}


