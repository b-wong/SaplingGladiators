using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GameMusic : MonoBehaviour
{
    public static GameMusic gameMusic;
    public StudioEventEmitter musicEmitter;
    public float musicDelay;
    public float musicIncreaseTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (gameMusic == null)
        {
            gameMusic = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicEmitter.Stop();
        StartCoroutine(MusicStartDelay(musicDelay));
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator MusicStartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        musicEmitter.Play();
        StartCoroutine(MusicSpeed(GameTimer.gameLength));
        
    }

    public IEnumerator MusicSpeed(float gameTime)
    {
        float t;
        float numOfAdjustments = 1;
        for(;;)
        {
            t = (1/gameTime) * numOfAdjustments;
            musicEmitter.EventInstance.setParameterValue("Speed", Mathf.Lerp(0,1, t));
            numOfAdjustments+=musicIncreaseTime;
            yield return new WaitForSeconds(musicIncreaseTime);
        }
    }
}
