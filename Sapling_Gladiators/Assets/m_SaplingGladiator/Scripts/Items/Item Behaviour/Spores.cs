using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spores : Item
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
                StartCoroutine(SporesActive(myItem.useTime));
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

    public IEnumerator SporesActive(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SporesActivate");
        SpawnEffect(myPlayer.enemyPlayer.gameObject.transform.position, useTime);

        //Stealing Logic
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
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity, myPlayer.gameObject.transform);
        //! Uncomment this code if the spawned effect has multiple particle systems that need to be stopped when finishing item usage.
        // List<ParticleSystem> particles = new List<ParticleSystem>();
        // foreach( Transform childTransform in spawnedEffect.transform)
        // {
        //     if (childTransform.GetComponentInChildren<ParticleSystem>())
        //     {
        //         particles.Add(childTransform.GetComponentInChildren<ParticleSystem>());
        //     }
        // }
        // StartCoroutine(StopParticleEffect(useTime, particles));
        StartCoroutine(StopParticleEffect(useTime, spawnedEffect.GetComponentInChildren<ParticleSystem>()));
    }

    private void OnDestroy() 
    {
        if (spawnedEffect != null)
        {
            if (spawnedEffect.GetComponent<ParticleSystem>())
            {
                spawnedEffect.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
