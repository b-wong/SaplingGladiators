using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornPrefabBehaviour : MonoBehaviour
{
    public Transform owner;
    public string enemyNumber;
    public float duration;
    public float spinningSpeed;
    public Vector3 offset;
    public GameObject damageEffect;
    public float damageEffectDuration;

    void Start()
    {
        StartCoroutine(DestroyMe(duration));
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, spinningSpeed * Time.deltaTime);
        transform.position = owner.position + offset;
    }

    private IEnumerator DestroyMe(float _lifeTime)
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    private IEnumerator DamageEffect(float _duration, Vector3 _position)
    {
        GameObject _temp = Instantiate(damageEffect, _position, Quaternion.identity);
        yield return new WaitForSeconds(_duration);
        Destroy(_temp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Player enemy = collision.GetComponent<Player>();
            if (collision.GetComponent<Player>().enemyPlayer == enemy)
            {
                StartCoroutine(DamageEffect(damageEffectDuration, enemy.transform.position));
                enemy.playerHealth.Damage(1);
            }

        }
    }
}
