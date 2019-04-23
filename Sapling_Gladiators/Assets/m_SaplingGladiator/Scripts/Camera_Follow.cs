using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target1, target2;
    public float lowestY;
    public float maxY;
    private void LateUpdate()
    {
        Vector2 _temp= target1.position + ((target2.position - target1.position) * .5f);
        _temp = new Vector2(_temp.x, Mathf.Clamp(_temp.y, lowestY, maxY));
        transform.position = new Vector3(transform.position.x,_temp.y,-10);
    }
}
