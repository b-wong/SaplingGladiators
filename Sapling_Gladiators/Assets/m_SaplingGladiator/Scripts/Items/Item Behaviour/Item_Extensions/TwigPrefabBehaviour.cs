using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigPrefabBehaviour : MonoBehaviour
{

    public float spinningSpeed = 10f;
    public float movementSpeed = 10f;


    void Update()
    {
        transform.Rotate(Vector3.forward, spinningSpeed * Time.deltaTime);
        transform.position += Vector3.up * movementSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/TwigHit");
            Destroy(gameObject);
        }
    }

}
