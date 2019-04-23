using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{   

    //! This is really dirty , I will clean this.
    //! this is future you, you probably won't

    public List<Player> players;

    public Camera gameCamera;
    
    public GameObject canvasGameObject;

    [Space]
    public Image player1ItemImage;
    public Image player2ItemImage;
    public float imageInUseAlpha;
    public float imageAlpha;
    public float tickRate;

    [Space]
    [Header("Text")]
    public GameObject endGameText;
    public Text healthPrefab;
    private Text newHealthText;
    public Text timerText;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerHealth.onHealthChanged.AddListener(SpawnHealthChangeText);
        }
        StartCoroutine(UpdatePlayerUI());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowEndText(string winningPlayerName)
    {
        endGameText.GetComponent<Text>().text = winningPlayerName.ToUpper() + " WINS!";
        endGameText.SetActive(true);
    }

    public void SpawnHealthChangeText(Transform targetTransform, float value)
    {
        newHealthText = Instantiate(healthPrefab, gameCamera.WorldToScreenPoint(targetTransform.position), Quaternion.identity, canvasGameObject.transform);
        TextFollow followScript = newHealthText.GetComponent<TextFollow>();
        followScript.target = targetTransform;
        followScript.gameCamera = gameCamera;
        followScript.value = value;
    }

    public void SetImageSettings(Image targetImage, Sprite replacementSprite, Color color)
    {
        targetImage.sprite = replacementSprite;
        targetImage.color = color;
    }

    public void SetImageSettings(Image targetImage, Color color)
    {
        targetImage.color = color;
    }

    public IEnumerator UpdatePlayerUI()
    {
        for(;;)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].playerInventory.myCurrentItem != null)
                {
                    if (players[i].playerInventory.myCurrentItem.Active == false)
                    {
                        if (players[i].playerInventory.myCurrentItem.ignoreMeInUI == false)
                        {
                            switch (i)
                            {
                                case 0: SetImageSettings(player1ItemImage, players[i].playerInventory.myCurrentItem.myItem.itemSprite, Color.white); break;
                                case 1: SetImageSettings(player2ItemImage, players[i].playerInventory.myCurrentItem.myItem.itemSprite, Color.white); break;
                            }
                        }
                    }
                    else
                    {
                        Color myColor = new Color(0.5f,0.5f,0.5f, imageInUseAlpha);
                        switch (i)
                        {
                            case 0: SetImageSettings(player1ItemImage, players[i].playerInventory.myCurrentItem.myItem.itemSprite, myColor); break;
                            case 1: SetImageSettings(player2ItemImage, players[i].playerInventory.myCurrentItem.myItem.itemSprite, myColor); break;
                        }
                    }
                    
                }
                else
                {
                    Color myColor = new Color(0,0,0, imageAlpha);
                    switch (i)
                    {
                        case 0: SetImageSettings(player1ItemImage, myColor); break;
                        case 1: SetImageSettings(player2ItemImage, myColor); break;
                    }
                }
            }
            yield return new WaitForSeconds(tickRate);
        }
    }
}
