using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationAtStart : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(Vector3.forward, Random.Range(-60f,60f));
    }
}
