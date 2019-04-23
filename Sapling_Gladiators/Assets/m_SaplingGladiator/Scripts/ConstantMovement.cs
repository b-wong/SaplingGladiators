using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{

    public Vector2 velocity;
    public float rotateSpeed;
    public GameObject objToRotate;
    public Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        if (rb)
        {
            if (rb.velocity != velocity)
            {
                rb.velocity = velocity;
            }
        }
        objToRotate.transform.Rotate(0,0,rotateSpeed * Time.deltaTime, Space.Self);
    }
}
