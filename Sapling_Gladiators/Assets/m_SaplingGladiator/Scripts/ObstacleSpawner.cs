using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Obstacle
    {
        public GameObject obstacle;
        [Range(0, 100)]
        public int probability;
    }
    public Obstacle[] obstacles;
    public float spawnWidth;
    public int obstacleStartHeight = 5;// so that the obstacles does not spawn at the very beginning of the game area
    [Range(0, 20)]
    public int minVerticalDist;
    public int maxSpawnHeight;
    [Range(0, 20)]
    public static int obstacleSpawnProbability = 25;
    public Transform obstacleParent;

    private void Start()
    {
        for (int i = obstacleStartHeight; i < maxSpawnHeight; i++)
        {
            if (Random.Range(0f, 100f) < obstacleSpawnProbability)//spawn an obstacle
            {
                SpawnObstacle(i);
                i += minVerticalDist - 1;
            }
        }
    }

    private void SpawnObstacle(int _verticalPosition)
    {
        //?Debug.Log("---------------------------");
        Vector2 _spawnPosition = new Vector2(Random.Range(-spawnWidth / 2, spawnWidth / 2), _verticalPosition);
        int _totalProbability = 0;
        foreach (var obstacle in obstacles)// find total probability
        {
            _totalProbability += obstacle.probability;
        }
        //?Debug.Log("Total probability is: "+_totalProbability);
        int _result = Random.Range(0, _totalProbability + 1);// pick a number within the total probability range
        //?Debug.Log("Random pick is: " + _result);
        float _temp = 0;
        int _selection = 0;
        for (int i = 0; i < obstacles.Length; i++)// determine which obstacle will be spawned
        {
            if (_temp + obstacles[i].probability < _result)
            {
                _temp += obstacles[i].probability;
            }
            else
            {
                _selection = i;
                break;
            }
        }
        //?Debug.Log("Selected item is: " + _selection);
        Instantiate(obstacles[_selection].obstacle, _spawnPosition, Quaternion.identity, obstacleParent);// spawn obstacle
        //?Debug.Log("---------------------------");
    }
}
