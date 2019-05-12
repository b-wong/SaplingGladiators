using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleShrinker : MonoBehaviour
{
    public float maxSize;
    public float minSize;

    Vector3 maxScale;
    Vector3 minScale;

    public float shrinkDuration;

    public bool isShrinking;
    public bool isFullyShrunk = false;

    float startTimer;
    public AnimationCurve animationCurve;

    // Use this for initialization
    void Start()
    {
        // Create Vector3s for both the min and max sizes
        maxScale = new Vector3(maxSize, maxSize, 1);
        minScale = new Vector3(minSize, minSize, 1);
        // set the scale of the object to the minimum scale
        transform.localScale = maxScale;
        // initially, the object should grow
        isShrinking = true;

        ResetTimer();

    }

    void ResetTimer()
    {
        startTimer = Time.time;
    }

    public void Shrinker()
    {
        // calculate the time elapsed since the timer was started
        float timeElapsed = Time.time - startTimer;
        // get the percent of time elapsed in relation to the grow duration
        float percent = timeElapsed / shrinkDuration;

        // if the timer has elapsed, switch states
        if (percent > 1)
        {
            transform.localScale = minScale;
            isShrinking = false;
            ResetTimer();
            return;
        }
        if (percent > 0.2)
        {
            isFullyShrunk = true;
        }

        if (percent < 0.2)
        {
            isFullyShrunk = false;
        }
        // calculate the current scale of the square using the percent and animation curve
        Vector3 newScale = Vector3.Lerp(maxScale, minScale, animationCurve.Evaluate(percent));
        transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        Shrinker();
    }
}
