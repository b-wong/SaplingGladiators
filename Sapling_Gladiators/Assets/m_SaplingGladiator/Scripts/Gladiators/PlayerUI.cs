using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{   

    public Player myPlayer;
    public Canvas myCanvas;
    public float canvasResetDelay;

    [Space]
    [Header("UI Elements")]
    public List<Image> healthImages;
    public GameObject healthImageParent;
    public Image energyImage;
    public Image itemImage;

    [Space]
    public float imageInUseAlpha;
    public float imageAlpha;

    [Space]
    public float tickRate = 0.1f;

    private void Awake() 
    {
        if (myCanvas == null)
        {
            myCanvas = GetComponentInChildren<Canvas>();
        }
        // if (myCanvas)
        // {
        //     myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        healthImages = new List<Image>();
        foreach (Transform childrenTransforms in healthImageParent.transform)
        {
            if (childrenTransforms.GetComponent<Image>())
            {
                healthImages.Add(childrenTransforms.GetComponent<Image>());
            }
        }
        StartCoroutine(UpdateUI());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetCanvasPos()
    {
        if (myCanvas)
        {
            myCanvas.transform.position = Vector3.zero;
        }
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

    public void UpdateHealth()
    {
        float healthMissing = (myPlayer.playerHealth.maxHealth - myPlayer.playerHealth.health);
        float healthHidden = 0;
        //Debug.Log(myPlayer.playerHealth.maxHealth + "    " + myPlayer.playerHealth.health + "    "  + healthMissing);
        for (int i = healthImages.Count - 1; i >= 0; i--)
        {
            healthImages[i].enabled = true;
            if (healthHidden < healthMissing)
            {
                healthImages[i].enabled = false;
                healthHidden++;
            }
        }
    }

    public void UpdateEnergy()
    {
        energyImage.fillAmount = myPlayer.playerAbilities.solarEnergy / myPlayer.playerAbilities.maxSolarEnergy;
    }

    public void UpdateItem()
    {
        if (myPlayer.playerInventory.myCurrentItem != null)
        {
            if (myPlayer.playerInventory.myCurrentItem.Active == false)
            {
                if (myPlayer.playerInventory.myCurrentItem.ignoreMeInUI == false)
                {
                    SetImageSettings(itemImage, myPlayer.playerInventory.myCurrentItem.myItem.itemSprite, Color.white);
                }
            }
            else
            {
                Color myColor = new Color(0.5f,0.5f,0.5f, imageInUseAlpha);
                SetImageSettings(itemImage, myPlayer.playerInventory.myCurrentItem.myItem.itemSprite, myColor);
            }
        }
        else
        {
            Color myColor = new Color(0,0,0, imageAlpha);
            SetImageSettings(itemImage, myColor);
        }
    }

    public IEnumerator UpdateUI()
    {
        for(;;)
        {
            UpdateHealth();
            UpdateEnergy();
            //UpdateItem();
            yield return new WaitForSeconds(tickRate);
        }
    }

}
