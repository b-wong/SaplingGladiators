using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : Item
{
    [Header("Item Properties")]
    public int numOfObstacles; //The number of obstacles the earthquake should spawn
    public Vector2 spawnBox;
    public GameObject debrisPrefab;
    public GameObject spawnEffect;
    private Player myPlayer;
    private GameObject spawnedEffect;

    public float shakeAmount = 0.3f;

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
                StartCoroutine(EarthquakeActive(UseTime));
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

    public IEnumerator EarthquakeActive(float useTime)
    {
        Active = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Earthquake");
        SpawnEffect(myPlayer.gameObject.transform.position, useTime);

        //! SPAWN EARTHQUAKE OBSTACLES IN HERE PROBABLY, DO AS YOU LIKE MUSTAFA

        for (int i = 0; i < numOfObstacles; i++)
        {
            Instantiate(debrisPrefab,new Vector3(Random.Range(-spawnBox.x/2,spawnBox.x/2),
                Random.Range(transform.position.y+spawnBox.y-10f, transform.position.y + spawnBox.y + 10f),
                0),Quaternion.identity);
        }
        StartCoroutine(ShakeCam(useTime));
        yield return new WaitForSeconds(useTime);
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
            spawnedEffect.GetComponent<ParticleSystem>().Stop();
        }
    }

    private IEnumerator ShakeCam(float _shakeDuration)
    {
        Transform _camera = Camera.main.transform;
        float _cameraX = _camera.position.x;
        while (_shakeDuration > 0)
        {
            _camera.position += Random.insideUnitSphere * shakeAmount;
            _shakeDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            _camera.position = new Vector3(_cameraX, _camera.position.y, _camera.position.z);
        }
        
    }
}
