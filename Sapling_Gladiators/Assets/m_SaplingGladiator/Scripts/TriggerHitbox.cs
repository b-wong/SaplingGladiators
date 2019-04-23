using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHitbox : MonoBehaviour
{
    public GameObject colliderToActivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        colliderToActivate.SetActive(true);
    }
}
