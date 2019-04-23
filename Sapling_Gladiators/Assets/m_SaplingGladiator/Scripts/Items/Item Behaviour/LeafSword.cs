using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSword : Item
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
                StartCoroutine(LeafSwordActivate(myItem.useTime));
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

    public IEnumerator LeafSwordActivate(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SwordActivate");
        SpawnEffect(myPlayer.enemyPlayer.gameObject.transform.position, useTime);

        int _tempTackle = myPlayer.playerAbilities.tackleAbility.tackleDamage;
        myPlayer.playerAbilities.tackleAbility.tackleDamage++;
        yield return new WaitForSeconds(useTime);
        myPlayer.playerAbilities.tackleAbility.tackleDamage = _tempTackle;
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
