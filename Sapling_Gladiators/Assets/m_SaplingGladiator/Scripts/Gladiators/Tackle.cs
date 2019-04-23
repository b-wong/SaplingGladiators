using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : Ability
{
    //! Hold Up to charge , it needs to use a certain amount of charge meter,
    //! while charging it will slowly lock onto a target. Once ready, it will "Lock", 
    //! and a second later will launch the player towards that position. The time 
    //! between lock and launch is about 1 second.
    [Space(20)]
    public float chargeCostPerSecond;
    public int tackleDamage;
    public float chargeTime;
    public float delayBeforeAttacking;
    public float tackleTravelTime;
    public bool dynamicTravelTime;
    public float dynamicTimePerUnityUnit;
    public AnimationCurve tackleMovementCurve;
    private Vector3 targetLocation;
    public GameObject reticle;
    public GameObject fullReticle;
    public GameObject tackleHitbox;

    private Coroutine abilityCoroutine;

    [Space]
    public bool charging;
    public bool charged;
    
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        if (activated)
        {
            if (abilityCoroutine == null)
            {
                charging = true;
                abilityCoroutine = StartCoroutine(ChargeTackle());
            }
        }
        else
        {
            charging = false;
            reticle.SetActive(false);
            fullReticle.SetActive(false);
        }
    }

    public IEnumerator TackleComplete(float delay)
    {
        yield return new WaitForSeconds(delay);
        activated = false;
        tackleHitbox.SetActive(false);
    }

    public IEnumerator ChargeTackle()
    {
        charged = false;
        float timer = 0;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SaplingCharge");
        while (charging)
        {
            float chargeCostPerLoop = chargeCostPerSecond / (chargeTime / Time.deltaTime);
            //!Debug.Log(chargeCostPerSecond + "            " + chargeTime + "          " + Time.deltaTime);
            reticle.SetActive(true);
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, chargeTime);
            CheckForAndTakeCost(chargeCostPerLoop);
            reticle.transform.position = myPlayer.enemyPlayer.transform.position;
            if (timer == chargeTime)
            {
                charging = false;
                charged = true;
                targetLocation = myPlayer.enemyPlayer.transform.position;
            }
            yield return new WaitForEndOfFrame();
        }
        if (charged)
        {
            StartCoroutine(UseAbility(""));
        }
        abilityCoroutine = null;
    }

    public override IEnumerator UseAbility(string methodToCall)
    {
        reticle.SetActive(false);
        fullReticle.transform.position = myPlayer.enemyPlayer.transform.position;
        fullReticle.SetActive(true);
        yield return new WaitForSeconds(delayBeforeAttacking);
        fullReticle.SetActive(false);

        tackleHitbox.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SaplingLaunch");
        if (dynamicTravelTime)
        {
            float distanceToTarget = Vector3.Distance(myPlayer.enemyPlayer.tackleSpot.transform.position, myPlayer.transform.position);
            float dynamicTime = dynamicTimePerUnityUnit * distanceToTarget;
            StartCoroutine(myPlayer.playerController.Tackle(myPlayer.enemyPlayer.tackleSpot.transform.position, dynamicTime, tackleMovementCurve));
        }
        else
        {
            StartCoroutine(myPlayer.playerController.Tackle(myPlayer.enemyPlayer.tackleSpot.transform.position, tackleTravelTime, tackleMovementCurve));
        }
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}
