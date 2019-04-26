using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : Item
{
    [Header("Item Properties")]
    public GameObject spawnEffect;
    private Player myPlayer;
    private GameObject spawnedEffect;
    public GameObject thornsPrefab;

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
                StartCoroutine(ThornsActivate(UseTime));
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

    public IEnumerator ThornsActivate(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ThornsActivate");
        SpawnEffect(myPlayer.transform.position, useTime);
        GameObject _tempGameObject = Instantiate(thornsPrefab, myPlayer.transform.position,Quaternion.identity);
        _tempGameObject.GetComponent<ThornPrefabBehaviour>().owner = transform;
        _tempGameObject.GetComponent<ThornPrefabBehaviour>().duration=useTime;
        _tempGameObject.GetComponent<ThornPrefabBehaviour>().myPlayer = myPlayer;
        yield return new WaitForSeconds(useTime);
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
            if (spawnedEffect.GetComponent<ParticleSystem>())
            {
                spawnedEffect.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
