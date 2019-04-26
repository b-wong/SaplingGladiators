using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    public Toggle toggle;

    public bool tutorialButton;

    public void classic()
    {
        GameTimer.gameLength = 60;
        ItemSpawner.itemSpawnProbability = 25;
        ObstacleSpawner.obstacleSpawnProbability = 25;
        if (tutorialButton)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("ClassicGame");
        }
    }

    public void custom()
    {
        SceneManager.LoadScene("CustomMenu");
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void tutorial()
    {
        if (toggle.isOn == true){
            tutorialButton = true;
        }
        else
        {
            tutorialButton = false;
        }
    }

}
