using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [Header("Autodelay Settings")]
    public bool autoCalculateDelay = true; // if true the delay for destroy will be calculated based on the girth(health) of the player.
    public float maxDelayTime = .6f;
    public float minDelayTime = .2f;
    [Header("Custom Delay Settings")]
    public float delayBeforeDestroy = .5f; //if auto calculation is false

    public GameObject spawnEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Twig")
        {
            if (gameObject.GetComponentInChildren<Obstacle>())
            {
                gameObject.GetComponentInChildren<Obstacle>().OnBreak();
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            Instantiate(spawnEffect, transform.position, Quaternion.identity);
            if (autoCalculateDelay)
            {
                delayBeforeDestroy = (maxDelayTime - minDelayTime) / collision.gameObject.GetComponent<Health>().maxHealth *
                    (collision.gameObject.GetComponent<Health>().maxHealth - collision.gameObject.GetComponent<Health>().health);
            }
            Invoke("DestroyObstacle", delayBeforeDestroy);
        }
    }

    public void DestroyObstacle()
    {
        if (gameObject.GetComponentInChildren<Obstacle>())
        {
            gameObject.GetComponentInChildren<Obstacle>().OnBreak();
        }
        Destroy(gameObject);
    }

}
