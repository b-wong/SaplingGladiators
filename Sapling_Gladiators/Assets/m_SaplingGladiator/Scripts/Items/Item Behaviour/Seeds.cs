using UnityEngine;
using System.Collections;

public class Seeds : Item
{
    [Header("Seed Spawn Options")]
    public int numberOfSeeds;
    public float delayBetweenSpawns; // time of delay between the spawn of the seeds if there is more than 1 seed.
    public float scatterFactor;// distance between the farthest seeds.
    public float spawnDistanceFromEnemy; // in Y axis
    public GameObject seedPrefab;
    public GameObject spawnEffect;
    private GameObject seedTemp;
    private GameObject spawnedEffect;


    public Player myPlayer;

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
                StartCoroutine(ThrowSeeds(numberOfSeeds));
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

    public IEnumerator ThrowSeeds(int _numOfSeeds)
    {
        Active = true;

        if (numberOfSeeds < 1)
        {
            numberOfSeeds = 1;
        }

        Vector3 _spawnOrigin = Vector3.left * scatterFactor / 2;
        float _distanceBetweenSeeds = scatterFactor / (_numOfSeeds - 1);

        for (int i = 0; i < _numOfSeeds; i++)
        {
            Vector3 relativePos = new Vector3(_spawnOrigin.x + (_distanceBetweenSeeds * i), _spawnOrigin.y + spawnDistanceFromEnemy, _spawnOrigin.z);
            seedTemp = Instantiate(seedPrefab, myPlayer.transform.position, Quaternion.identity);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SeedShoot");
            seedTemp.GetComponent<SeedPrefabBehaviour>().target = myPlayer.enemyPlayer.transform;
            seedTemp.GetComponent<SeedPrefabBehaviour>().relativePos = relativePos;
            seedTemp.GetComponent<SeedPrefabBehaviour>().commence = true;
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        OnActivateEnd(myPlayer);
    }

    public void SpawnEffect(Vector3 position, float useTime)
    {
        spawnedEffect = Instantiate(spawnEffect, position, Quaternion.identity);
        spawnedEffect.transform.LookAt(seedPrefab.transform.position + (Vector3)seedPrefab.GetComponent<ConstantMovement>().velocity);
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
