using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    public bool allowActivate = false;
    public GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if (allowActivate)
            {
                objectToActivate.SetActive(true);
            }
        }
    }

}
