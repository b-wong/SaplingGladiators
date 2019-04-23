using System.Collections;
using UnityEngine;

public class SeedPrefabBehaviour : MonoBehaviour
{
    public bool commence = false;
    public Vector3 relativePos;
    public float movementSpeed;
    public float spinningSpeed;
    public Transform target;
    public float collisionTolerance; // how far does the seed need to be its target to register a collision

    [Header("Obstacle Spawn Options")]
    public GameObject[] obstacles;
    public GameObject spawnEffect;
    public float spawnEffectLifetime;
    private GameObject spawnedEffect;
    private bool isInSeedForm = true;

    void Update()
    {
        Vector3 _targetPosition = target.position + relativePos;
        CheckPosition(_targetPosition);

        if (commence)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, spinningSpeed * Time.deltaTime);
        }
    }

    private void CheckPosition(Vector3 _target)
    {
        if (Vector3.Distance(transform.position, _target) < collisionTolerance)
        {
            commence = false;
            if (isInSeedForm)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                spawnedEffect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
                Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position, Quaternion.identity);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SeedGrow");
                StartCoroutine(StopSpawnEffect(spawnEffectLifetime, spawnedEffect));
                isInSeedForm = false;
            }
        }
    }

    private IEnumerator StopSpawnEffect(float _particleDuration, GameObject _spawnEffect)
    {
        yield return new WaitForSeconds(_particleDuration);
        Destroy(_spawnEffect);
        Destroy(gameObject);
    }
}
