using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Ability
{
    //! Hold Left and Right to perform a block. 
    //! A block uses lots of charge and will block any attack and will stun the attacker
    
    [Space(20)]
    [Header("Block Properties")]
    public float blockTickRate;
    public float blockCostPerSecond;
    public float stunTime;
    public bool blocking = false;
    public GameObject blockEffect;
    public GameObject stunEffect;

    private Coroutine abilityCoroutine;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        if (activated)
        {
            if (abilityCoroutine == null)
            {
                blocking = true;
                abilityCoroutine = StartCoroutine(UseAbility(""));
            }
        }
        else
        {
            blocking = false;
        }
    }

    public void StunEnemyPlayer()
    {
        abilityCoroutine = StartCoroutine(StunPlayer());
    }

    public override IEnumerator UseAbility(string methodToCall)
    {
        GameObject newBlockEffect = Instantiate(blockEffect, transform.position, Quaternion.identity, myPlayer.transform);
        bool healthUpdated = false;
        float blockCostPerTick = blockCostPerSecond / (1 / blockTickRate);
        myPlayer.playerSounds.blockSoundEmitter.SetActive(true);
        while (blocking)
        {
            if (healthUpdated == false)
            {
                myPlayer.playerHealth.invulnerable = true;
                healthUpdated = true;
            }
            CheckForAndTakeCost(blockCostPerTick);
            yield return new WaitForSeconds(blockTickRate);
        }
        newBlockEffect.GetComponent<ParticleSystem>().Stop();
        myPlayer.playerSounds.blockSoundEmitter.SetActive(false);
        blocking = false;
        activated = false;
        abilityCoroutine = null;
        myPlayer.playerHealth.invulnerable = false;
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    public IEnumerator StunPlayer()
    {
        yield return new WaitForSeconds(useTime);
        myPlayer.enemyPlayer.playerController.AllowControl(false);
        GameObject newStunEffect = Instantiate(stunEffect, myPlayer.enemyPlayer.transform.position, Quaternion.identity, myPlayer.enemyPlayer.transform);
        Destroy(newStunEffect, stunTime + 0.1f);
        yield return new WaitForSeconds(stunTime);
        myPlayer.enemyPlayer.playerController.AllowControl(true);
    }
}
