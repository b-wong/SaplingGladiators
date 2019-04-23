using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum GameState { IN_PROGRESS, END }
    [Header("Game State")]
    public GameState gameState;
    public float endGameTimeScale;
    public float reloadSceneDelayTime;
    public string sceneNameToLoadOnEnd;

    [Space]

    [Header("Game Scripts")]
    public GameTimer gameTimer;
    public GameUI gameUI;

    [Space]

    [Header("Player References")]
    public List<Player> players;
    public Player winningPlayer;
    public Player losingPlayer;
    public bool noWinner;
    private bool playingSound;

    [Space(50)]
    public GameObject surface;
    public Camera gameCamera;
    public float heightAbovePlayersToSpawnSurface;
    private bool surfacedSpawned;
    private GameObject newSurface;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.IN_PROGRESS: InProgress(); break;
            case GameState.END: End(); break;
        }
    }

    public void InProgress()
    {
        if (gameTimer.gameTime >= GameTimer.gameLength - 5)
        {
            if (!surfacedSpawned)
            {
                SpawnSurface(gameCamera.transform.position + new Vector3(0,heightAbovePlayersToSpawnSurface, 10));
            }
        }
        if (CheckForPlayerDeaths() == true)
        {
            gameState = GameState.END;
            noWinner = false;
        }
        else if (gameTimer.gameTime >= GameTimer.gameLength)
        {
            noWinner = true;
            gameState = GameState.END;
        }
    }

    public void End()
    {
        if (noWinner == false)
        {
            if (surfacedSpawned)
            {
                if (newSurface.GetComponentInChildren<ActivateObject>())
                {
                    ActivateObject flowerActivator = newSurface.GetComponentInChildren<ActivateObject>();
                    flowerActivator.allowActivate = true;
                }
            }
            
            gameUI.ShowEndText(winningPlayer.playerNumber);
            if (!playingSound)
            {
                SpawnSurface(gameCamera.transform.position + new Vector3(0,heightAbovePlayersToSpawnSurface * 0.8f, 10));
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Victory");
                playingSound = true;
            }
        }
        else
        {
            Time.timeScale = endGameTimeScale;
            gameUI.ShowEndText("NO ONE");
            if (!playingSound)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Defeat");
                playingSound = true;
            }
            
        }
        StartCoroutine(Delay(reloadSceneDelayTime, sceneNameToLoadOnEnd));
    }

    public IEnumerator Delay(float time, string sceneName)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(sceneName);
        //! CHANGE THIS EVENTUALLY ^
    }

    public void SpawnSurface(Vector3 spawnPos)
    {
        if (surfacedSpawned) { return; } 
        surfacedSpawned = true;
        //5 ish seconds before game ends, spawn the surface
        newSurface = Instantiate(surface, spawnPos, Quaternion.identity);
        if (gameCamera.GetComponent<Camera_Follow>())
        {
            gameCamera.GetComponent<Camera_Follow>().maxY = newSurface.GetComponentInChildren<Collider2D>().gameObject.transform.position.y;
        }
    }

    public bool CheckForPlayerDeaths()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerHealth.health <= 0)
            {
                losingPlayer = players[i];
                return true;
            }
            else
            {
                winningPlayer = players[i];
            }
        }
        return false;
    }

}
