using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 distanceFromTarget = Vector3.one;

    public Camera gameCamera;
    public bool useValue;
    public float value;

    private Text myText;
    public bool destroyThis;
    public bool moveThis;

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponentInChildren<Text>();
        if (useValue)
        {
            if (value > 0)
            {
                myText.color = Color.green;
                myText.text = "+" + value.ToString();
            }
            else
            {
                myText.color = Color.red;
                myText.text = value.ToString();
            }
        }
        
        if (destroyThis)
        {
            Destroy(gameObject, 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moveThis)
        {
            transform.position = gameCamera.WorldToScreenPoint(target.position) + distanceFromTarget;
            distanceFromTarget += new Vector3(0,0.5f,0);
        }
        else
        {
            transform.position = gameCamera.WorldToScreenPoint(target.position) + distanceFromTarget;
        }
    }
}
