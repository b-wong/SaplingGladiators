using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafAxe : Item
{   

    [Header("Item Properties")]
    public Vector2 axeTravelVelocity;
    public float axeLifetime;
    public GameObject axe;
    public Vector3 spawnDistanceFromPlayer;
    public GameObject spawnEffect;

    public Player myPlayer;
    private GameObject spawnedEffect;
    private GameObject thrownAxe;

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
                StartCoroutine(ThrowAxe(myItem.useTime));
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

    public void AxeHit()
    {
        if (thrownAxe != null)
        {
            SpawnEffect(thrownAxe.transform.position, UseTime);
            if (gameObject.GetComponentInChildren<Collider2D>())
            {
                thrownAxe.GetComponentInChildren<Collider2D>().enabled = false;
            }
            Destroy(thrownAxe);
        }
        myPlayer.enemyPlayer.playerHealth.Damage(AttackPower);
    }

    public IEnumerator ThrowAxe(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Swing");
        thrownAxe = Instantiate(axe, transform.position + spawnDistanceFromPlayer, Quaternion.identity);
        //Calculate direction to enemy player
        Vector2 directionToEnemy = myPlayer.enemyPlayer.transform.position - myPlayer.transform.position;
        Vector2 newDirection = new Vector2(directionToEnemy.x, axeTravelVelocity.y);
        thrownAxe.GetComponent<ConstantMovement>().velocity = newDirection;
        Destroy(thrownAxe, axeLifetime);
        yield return new WaitForSeconds(useTime);
        OnActivateEnd(myPlayer);
    }

    public void SpawnEffect(Vector3 position, float useTime)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity);
        spawnedEffect.transform.LookAt(thrownAxe.transform.position + (Vector3)thrownAxe.GetComponent<ConstantMovement>().velocity);
        StartCoroutine(StopParticleEffect(useTime, spawnedEffect.GetComponent<ParticleSystem>()));
    }

    private void OnDestroy() 
    {
        if (spawnedEffect != null)
        {
            spawnedEffect.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }
}
