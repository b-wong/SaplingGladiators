using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : Ability
{

    //! Press Down (or equivalent) to quickly drop your height in order to dodge
    [Space(20)]
    [Header("Dodge Properties")]
    public float distanceToDodge; //The distance to travel
    public float dodgeTravelTime; //The time it takes to travel their current position to their target distance
    public bool allowHorMovement; //Allow horizontal movement while dodging?
    public AnimationCurve dodgeMovementCurve;
    public Vector2 dodgeDirection;
    public GameObject dodgeEffect;

    private Coroutine abilityCoroutine;

    public override void ActivateAbility()
    {
        if (myPlayer.playerController.knockback == false)
        {
            base.ActivateAbility();
            if (activated)
            {
                if (abilityCoroutine == null)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SaplingDodge");
                    abilityCoroutine = StartCoroutine(UseAbility("CallDodge"));
                }
            }
        }
    }

    public float CalculateVelocity()
    {
        return (distanceToDodge / dodgeTravelTime);
    }

    public void CallDodge()
    {
        //myPlayer.playerController.Dodge(dodgeDirection * CalculateVelocity());
        //GameObject newDodgeEffect = Instantiate(dodgeEffect, transform.position, Quaternion.identity, myPlayer.transform);
        StartCoroutine(myPlayer.playerController.Dodge(distanceToDodge, dodgeTravelTime, dodgeDirection, dodgeMovementCurve));
        if (myPlayer.playerInventory.myCurrentItem != null)
        {
            if (myPlayer.playerInventory.myCurrentItem.GetComponent<LeafAxe>())
            {
                myPlayer.playerInventory.UseItem(myPlayer.playerInventory.myCurrentItem);
            }
        }
    }

    public override IEnumerator UseAbility(string methodToCall)
    {
        //Override PlayerController's velocity for activationTime
        yield return new WaitForSeconds(useTime);
        Invoke(methodToCall, 0);
        yield return new WaitForSeconds(dodgeTravelTime);
        myPlayer.playerController.ResetControlOwnership();
        activated = false;
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
        abilityCoroutine = null;
    }
}
