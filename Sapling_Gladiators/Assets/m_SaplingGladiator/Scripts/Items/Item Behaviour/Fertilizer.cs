using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : Item
{
    [Header("Item Properties")]
    public float speedUpFactor;
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
                StartCoroutine(FertilizerActive(myItem.useTime));
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

    public IEnumerator FertilizerActive(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/FertilizerActivate");
        float _initalMaxVelocity = myPlayer.playerController.maxVelocity;
        SpawnEffect(myPlayer.gameObject.transform.position, useTime);

        myPlayer.playerController.maxVelocity = _initalMaxVelocity * speedUpFactor;
        yield return new WaitForSeconds(useTime);
        
        myPlayer.playerController.maxVelocity = _initalMaxVelocity;
        OnActivateEnd(myPlayer);
    }

    public void SpawnEffect(Vector3 position, float useTime)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity, myPlayer.gameObject.transform);
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
