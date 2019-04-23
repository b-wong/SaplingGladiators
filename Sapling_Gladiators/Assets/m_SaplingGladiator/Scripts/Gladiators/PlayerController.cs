using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerController : MonoBehaviour
{
    //This script allows players to control their gladiators - only allows for horizontal movement left and right.
    //Additionally, this script perpetually forces gladiators upwards.
    public Rigidbody2D rb;
    public bool allowControl;
 
    [Header("Speed Settings")]
    public float maxVelocity;
    public float accelerationTime;
    public float decelerationTime;
    [Space]
    public bool accelerating;
    public bool decelerating;
    [Space]
    public float verticalVelocity;
    private float defaultVerticalVelocity;
    [Space]
    [Header("Knockback Settings")]
    public float knockbackForce;
    private float correctedKnockbackForce;
    public float knockbackDuration;
    public bool knockback;
 
    [Space]
    public Vector3 calculatedVelocity;
 
    public float direction;
    public float velocity;
    public float knockbackVelocity;
 
    [Space(20)]
    public Player myPlayer;
    public float maxSpriteTiltInDegrees;
 
    private Coroutine knockbackReset;
    private bool controlsOverriden;
    private float lerpedValue;
    private float easeOutValue;
 
    // Start is called before the first frame update
    void Start()
    {
        defaultVerticalVelocity = verticalVelocity;
        velocity = 0;
        direction = 1;
        myPlayer = GetComponentInChildren<Player>();
    }

    void FixedUpdate() 
    {
        if (!knockback)
        {
            //If not experiencing knockback
            if (accelerating)
            {
                Accelerate();
            }
            else if (decelerating)
            {
                Decelerate();
            }
        }
        else
        {
            //If Player is accelerating towards the direction of the knockback, let them accelerate normally.
            if (accelerating && direction == Mathf.Sign(correctedKnockbackForce))
            {
                Accelerate();
            }
            else //Set velocity to 0 to prevent bumpy behavior
            {
               velocity = 0;
            }
        }

        if (knockback)
        {
            DecelerateKnockback();
        }
        
        if (controlsOverriden == false)
        {
            calculatedVelocity = new Vector3(velocity + knockbackVelocity, verticalVelocity, 0);
        }
        rb.velocity = calculatedVelocity;
        
        if (knockback)
        {
            myPlayer.sprite.gameObject.transform.eulerAngles = new Vector3(0,0, -Mathf.Clamp(Mathf.Pow(knockbackVelocity, 3), -maxSpriteTiltInDegrees, maxSpriteTiltInDegrees));
        }
        else
        {
            myPlayer.sprite.gameObject.transform.eulerAngles = new Vector3(0,0, -Mathf.Clamp(Mathf.Pow(velocity, 3), -maxSpriteTiltInDegrees, maxSpriteTiltInDegrees)); 
        }
    }

#region Movement
    public void Accelerate()
    {
        float acceleration = (maxVelocity / accelerationTime) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + (acceleration * direction), -maxVelocity, maxVelocity);
    }
 
    public void Decelerate()
    {
        float deceleration = (maxVelocity / decelerationTime) * Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity + (deceleration * -direction), -maxVelocity, maxVelocity);
 
        if (Mathf.Sign(velocity) == -direction)
        {
            velocity = 0;
        }
    }

    public void DecelerateKnockback()
    {
        //If player is currently experiencing knockback, apply deceleration to that knockback force
        if (knockbackReset != null)
        {
            float deceleration = (maxVelocity / decelerationTime) * Time.fixedDeltaTime;
            knockbackVelocity += deceleration * -Mathf.Sign(correctedKnockbackForce);
        }
    }
 
    public void Knockback(float force)
    {
        knockback = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SaplingCollide");
        Vector3 vectorToEnemy = myPlayer.enemyPlayer.transform.position - myPlayer.transform.position;
        float forceDirection = Mathf.Sign(vectorToEnemy.x);
        float correctedForce = Mathf.Abs(force) * -forceDirection;
        myPlayer.playerAbilities.allowAbilityUse = false;
        knockbackVelocity = Mathf.Clamp(knockbackVelocity + correctedForce, -maxVelocity, maxVelocity);
        knockbackReset = StartCoroutine(KnockbackTimer(knockbackDuration));
        correctedKnockbackForce = knockbackVelocity + correctedForce;
    }
 
    public IEnumerator KnockbackTimer(float time)
    {
        yield return new WaitForSeconds(time);
        knockbackVelocity = 0;
        knockback = false;
        knockbackReset = null;
        myPlayer.playerAbilities.allowAbilityUse = true;
    }

    public IEnumerator LerpValue(float initialValue, float targetValue, float timeToLerp)
    {
		for (float i = 0; i < 1; i+= 1 / (timeToLerp / Time.deltaTime) )
		{
			lerpedValue = Mathf.Lerp(initialValue, targetValue, i);
			yield return null;
		}
		lerpedValue = targetValue;
    }

    public IEnumerator EaseOutValue(float currentValue, float targetValue, float sharpness)
    {
        float blend = 1f - Mathf.Pow(1f - sharpness, Time.deltaTime * 30f);
        for (;;)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, blend);
            easeOutValue = currentValue;
            yield return null;
        }
    }
 
#endregion

#region Abilities

    // public void Dodge(Vector2 newVelocity)
    // {
    //     //Overrides current velocity and applies a new one

    //     if (!knockback) //If not being knocked back, allow a dodge
    //     {
    //         controlsOverriden = true;
    //         //calculatedVelocity = new Vector3(velocity + knockbackVelocity, verticalVelocity, 0);
    //         if (myPlayer.playerAbilities.dodgeAbility.allowHorMovement)
    //         {
    //             calculatedVelocity = new Vector3(velocity, newVelocity.y, 0);
    //         }
    //         else
    //         {
    //             calculatedVelocity = newVelocity;
    //         }
    //     }
    // }

    public IEnumerator Dodge(float travelDistance, float travelTime, Vector2 direction, AnimationCurve animCurve)
    {
        controlsOverriden = true;
        float timer = 0;

        while (timer < travelTime)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, travelTime);
            float percentageComplete = timer/travelTime;
            float newVelocity = travelDistance * animCurve.Evaluate(percentageComplete);
            Vector2 newVelocityVector = direction * newVelocity;

            if (myPlayer.playerAbilities.dodgeAbility.allowHorMovement)
            {
                calculatedVelocity = new Vector3(velocity, newVelocityVector.y, 0);
            }
            else
            {
                calculatedVelocity = newVelocityVector;
            }
            yield return new WaitForFixedUpdate();
        }
        ResetControlOwnership();
    }

    public IEnumerator Tackle(Vector3 targetLocation, float travelTime, AnimationCurve animCurve)
    {
        controlsOverriden = true;
        float timer = 0;
        //!Debug.Log(myPlayer.enemyPlayer.transform.position + "    " + transform.position);
        Vector2 vectorToTarget = targetLocation - transform.position;
        //!Debug.Break();
        //!Debug.LogWarning(vectorToTarget);
        float xDistance = vectorToTarget.x;
        float yDistance = vectorToTarget.y;
        float completeTravelTime = 1 / (travelTime / Time.fixedDeltaTime);
        while (timer < travelTime)
        {
            timer = Mathf.Clamp(timer + Time.fixedDeltaTime, 0, travelTime);
            float percentageComplete = timer/completeTravelTime;
            float newVelocityX = (xDistance/travelTime) * animCurve.Evaluate(percentageComplete);
            float newVelocityY = (yDistance/travelTime) * animCurve.Evaluate(percentageComplete);
            
            Vector2 newVelocityVector = new Vector3(newVelocityX, newVelocityY, 0);
            calculatedVelocity = newVelocityVector;
            myPlayer.sprite.gameObject.transform.eulerAngles = new Vector3(0,0, -Mathf.Clamp(Mathf.Pow(newVelocityVector.x, 3), -maxSpriteTiltInDegrees, maxSpriteTiltInDegrees));
            
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(myPlayer.playerAbilities.tackleAbility.TackleComplete(0.2f));
        ResetControlOwnership();
    }

    // public void DecelerateDodge()
    // {
    //     if (controlsOverriden == false)
    //     {
    //         float deceleration = (maxVelocity / decelerationTime) * Time.fixedDeltaTime;
    //         knockbackVelocity += deceleration * -Mathf.Sign(knockbackForce);
    //     }
    // }

    public void ResetControlOwnership()
    {
        controlsOverriden = false;
    }

    public void AllowControl(bool state)
    {
        allowControl = state;
    }

#endregion

}