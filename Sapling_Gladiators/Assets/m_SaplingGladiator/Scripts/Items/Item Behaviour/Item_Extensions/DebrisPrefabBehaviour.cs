using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisPrefabBehaviour : MonoBehaviour
{
    public float movementSpeed;
    public float spinningSpeed;
    public float lifeTime;
    public float minSize;
    public float maxSize;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        transform.Rotate(Vector3.forward, Random.Range(0,359));
        transform.localScale = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (movementSpeed * Time.deltaTime), transform.position.z);
        transform.Rotate(Vector3.forward, spinningSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>()!=null)
        {
            collision.GetComponent<Health>().Damage(damage);
        }
    }
}
