using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    [Header("Timer")]
    public float gameTime; //The current time of the game session
    public static float gameLength = 60; //The total amount of time the game session should last
    

    public Text timerText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTime+=Time.deltaTime;
        if (timerText != null)
        {
            string cleanTime = gameTime.ToString("F1");
            timerText.text = cleanTime;
        }

        if (gameTime >= gameLength)
        {
            timerText.color = Color.red;
            timerText.text = gameLength.ToString();
        }
        else
        {
            timerText.color = Color.black;
        }
    }
}
