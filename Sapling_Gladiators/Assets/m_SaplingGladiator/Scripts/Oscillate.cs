using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{

    public GameObject spriteToMove;
    public float movementSpeed;
    public List<Transform> locations;
    public Transform destination;
    public float destinationRange;

    // Start is called before the first frame update
    void Start()
    {
        destination = locations[0];
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(spriteToMove.transform.position, destination.position);
        if ( distance > destinationRange)
        {
            spriteToMove.transform.position = Vector3.MoveTowards(spriteToMove.transform.position, destination.position, movementSpeed * Time.deltaTime);
        }
        else if (distance <= destinationRange)
        {
            if (destination == locations[0])
            {
                destination = locations[1];
            }
            else
            {
                destination = locations[0];
            }
        }
    }
}
