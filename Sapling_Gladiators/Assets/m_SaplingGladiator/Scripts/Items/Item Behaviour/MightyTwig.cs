using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightyTwig : Item
{
    [Header("Item Properties")]
    public GameObject spawnEffect;

    private Player myPlayer;
    private GameObject spawnedEffect;

    private void OnEnable()
    {
        ApplyProperties(myItem);
    }
    public override bool OnActivate(Player player)
    {
        if (Active == false)
        {
            if (NumOfUses > 0)
            {
                myPlayer = player;
                StartCoroutine(MightyTwigActivate());
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public override bool OnActivateEnd(Player player)
    {
        Active = false;
        NumOfUses--;
        if (NumOfUses <= 0 && gameObject != null)
        {
            ignoreMeInUI = true;
            Destroy(gameObject);
        }
        return false;
    }

    public IEnumerator MightyTwigActivate()
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Swing");
        SpawnEffect(myPlayer.gameObject.transform.position);
        OnActivateEnd(myPlayer);
        yield return null;
    }

    public void SpawnEffect(Vector3 position)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity);
        Destroy(spawnedEffect, 4);
    }
}
