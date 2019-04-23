using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GustOfWind : Item
{
    [Header("Item Properties")]
    public GameObject spawnEffect;

    private Player myPlayer;
    private GameObject spawnedEffect;
    private float timeAbilityActivated;
    private float timeDifference;

    private void OnEnable() {
        ApplyProperties(myItem);
    }
    public override bool OnActivate(Player player)
    {
        if (Active == false)
        {
            if (NumOfUses > 0)
            {
                myPlayer = player;
                StartCoroutine(WindActivate(myItem.useTime));
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

    public IEnumerator WindActivate(float useTime)
    {
        Active = true;
        SpawnEffect(myPlayer.enemyPlayer.gameObject.transform.position, useTime);
        if (myPlayer.enemyPlayer.playerInventory.myCurrentItem != null)
        {
            myPlayer.enemyPlayer.playerInventory.myCurrentItem.OnActivateEnd(myPlayer.enemyPlayer);
            yield return new WaitForSeconds(UseTime/2);
            myPlayer.enemyPlayer.playerInventory.RemoveItem(myPlayer.enemyPlayer.playerInventory.myCurrentItem);
        }
        yield return new WaitForSeconds(UseTime/2);
        OnActivateEnd(myPlayer);
    }

    public void SpawnEffect(Vector3 position, float useTime)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity, myPlayer.enemyPlayer.gameObject.transform);
        StartCoroutine(StopParticleEffect(useTime, spawnedEffect.GetComponent<ParticleSystem>()));
    }

    private void OnDestroy() 
    {
        if (spawnedEffect != null)
        {
            spawnedEffect.GetComponent<ParticleSystem>().Stop();
        }
    }
}
