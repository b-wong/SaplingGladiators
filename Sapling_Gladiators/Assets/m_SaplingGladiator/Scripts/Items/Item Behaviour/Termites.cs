using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Termites : Item
{
    [Header("Item Properties")]
    public GameObject spawnEffect;
    private Player myPlayer;
    private GameObject spawnedEffect;

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
                StartCoroutine(TermitesActive(myItem.useTime));
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

    public IEnumerator TermitesActive(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Termites");
        //The amount of time between damage ticks is useTime/numberOfAttacks (attackPower)
        float timeBetweenAttacks = myItem.useTime / AttackPower;
        int numberOfAttacks = 0;
        SpawnEffect(myPlayer.transform.position, useTime);

        while(numberOfAttacks < AttackPower ) 
        {
            numberOfAttacks++;
            myPlayer.playerHealth.Damage(1);
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
        OnActivateEnd(myPlayer);
    }

    public void SpawnEffect(Vector3 position, float useTime)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity, myPlayer.transform);
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
