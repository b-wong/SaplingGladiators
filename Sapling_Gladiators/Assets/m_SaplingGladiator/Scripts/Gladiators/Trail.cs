using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{

    private float trailWidth;
    private Color trailColor;

    public Player myPlayer;
    public TrailRenderer myTrailRenderer;
    public float tickRate;
    public float minWidth = 0.2f;
    [Range(0,1f)]
    public float healthMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GladiatorTrail());
    }

    public IEnumerator GladiatorTrail()
    {
        for(;;)
        {
            myTrailRenderer.widthMultiplier = Mathf.Clamp(myPlayer.playerHealth.health * healthMultiplier, minWidth, myPlayer.playerHealth.maxHealth * healthMultiplier);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
