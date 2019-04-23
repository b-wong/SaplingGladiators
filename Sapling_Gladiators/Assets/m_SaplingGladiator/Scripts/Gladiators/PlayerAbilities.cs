using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{

    [Header("Solar Energy")]
    public float solarEnergy; //The amount of energy this player has
    public float maxSolarEnergy; //The maximum amount of energy this player has
    public float energyRecoveryPerSecond; //The amount of energy the player recovers per second

    [Space]
    [Header("Abilities")]
    public List<Ability> myAbilities;
    public Dodge dodgeAbility;
    public Tackle tackleAbility;
    public Block blockAbility;
    public Ability myActiveAbility;
    public bool allowAbilityUse;

    public Player myPlayer;

    private void Start() 
    {
        StartCoroutine(RecoverEnergy());
    }

    public void ActivateDodge()
    {
        if (!myActiveAbility.activated)
        {
            myActiveAbility = dodgeAbility;
            myActiveAbility.ActivateAbility();
        }
    }

    public void ActivateTackle()
    {
        if (!myActiveAbility.activated)
        {
            myActiveAbility = tackleAbility;
            myActiveAbility.ActivateAbility();
        }
        else
        {
            if (myActiveAbility.CheckForCost() == false)
            {
                StopTackle();
            }
        }
    }

    public void StopTackle()
    {
        tackleAbility.charging = false;
        tackleAbility.activated = false;
        tackleAbility.reticle.SetActive(false);
        tackleAbility.fullReticle.SetActive(false);
    }

    public void ActivateBlock()
    {
        if (!myActiveAbility.activated)
        {
            myActiveAbility = blockAbility;
            myActiveAbility.ActivateAbility();
        }
        else
        {
            if (myActiveAbility.CheckForCost() == false)
            {
                StopBlock();
            }
        }
    }

    public void StopBlock()
    {
        if (myActiveAbility = blockAbility)
        {
            if (myActiveAbility.activated)
            {
                blockAbility.blocking = false;
                blockAbility.activated = false;
            }
        }
    }

    private void OnEnable() 
    {
        if (myPlayer == null)
        {
            myPlayer = GetComponentInParent<Player>();
        }
        if (myActiveAbility == null)
        {
            myActiveAbility = dodgeAbility;
        }
    }

    public IEnumerator RecoverEnergy()
    {
        for(;;)
        {
            solarEnergy = Mathf.Clamp(solarEnergy + energyRecoveryPerSecond/10, 0, maxSolarEnergy);
            yield return new WaitForSeconds(0.1f);
        }
    }


}
