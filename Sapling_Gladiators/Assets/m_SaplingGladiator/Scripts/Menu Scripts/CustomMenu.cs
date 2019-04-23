using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CustomMenu : MonoBehaviour
{
    public Slider matchTime;
    //public GameTimer timer;
    public Slider itemSpawn;
    public Slider obstacleSpawn;
   // public ItemSpawner spawnItem;
   // public ObstacleSpawner spawnObstacle;

    private void Start()
    {
        matchTime.value = 60;
        itemSpawn.value = 25;
        obstacleSpawn.value = 25;
    }

    public void slider()
    {
        matchTime.minValue = 0;
        matchTime.maxValue = 100;
        GameTimer.gameLength = matchTime.value;
    }

    public void itemSlider()
    {
        itemSpawn.minValue = 0;
        itemSpawn.maxValue = 100;
        ItemSpawner.itemSpawnProbability = (int)itemSpawn.value;
    }

    public void obstacleSlider()
    {
        obstacleSpawn.minValue = 0;
        obstacleSpawn.maxValue = 100;
        ObstacleSpawner.obstacleSpawnProbability = (int)obstacleSpawn.value;
    }

}
