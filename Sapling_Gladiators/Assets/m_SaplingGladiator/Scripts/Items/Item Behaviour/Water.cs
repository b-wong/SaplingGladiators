using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Item
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
                StartCoroutine(WaterActive(myItem.useTime));
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
            // myPlayer.GetComponent<Inventory>().myCurrentItem = null;
            Destroy(gameObject);
        }
        return false;
    }

    public IEnumerator WaterActive(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/WaterSplash");
        SpawnEffect(myPlayer.gameObject.transform.position, useTime);

        myPlayer.playerHealth.Heal(1);
        yield return new WaitForSeconds(useTime);
        
        OnActivateEnd(myPlayer);
    }

    public void SpawnEffect(Vector3 position, float useTime)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity, myPlayer.gameObject.transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/WaterActive");
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
