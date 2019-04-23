using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopper : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInChildren<Player>())
            {
                other.gameObject.GetComponentInChildren<Player>().playerController.verticalVelocity = 0;
            }
        }
    }
}
