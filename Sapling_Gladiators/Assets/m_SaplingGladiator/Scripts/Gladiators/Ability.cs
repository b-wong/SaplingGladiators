using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    public Player myPlayer;

    public bool activated; //Is this ability activated?
    public bool onCooldown; //Is this ability on cooldown?
    public float useTime; //The amount of time it takes until the ability's effects are activated
    public float activationTime; //The amount of time the ability is active
    public float cooldownTime; //The amount of time it takes for the ability to be re-used
    
    public float cost; //The energy cost of this ability

    public virtual void ActivateAbility()
    {
        if (CheckForCost())
        {
            if (myPlayer.playerAbilities.allowAbilityUse != false && onCooldown == false && activated == false) 
            {
                //Activate Ability
                TakeCost();
                activated = true;
            }
        }
        else
        {
            activated = false;
        }
    }

    public virtual void ActivateAbility(Vector3 position)
    {
        if (CheckForCost())
        {
            if (myPlayer.playerAbilities.allowAbilityUse != false && onCooldown == false) 
            {
                //Activate Ability
                TakeCost();
                activated = true;
            }
        }
    }

    public virtual bool CheckForAndTakeCost()
    {
        if (CheckForCost())
        {
            TakeCost(cost);
            return true;
        }
        return false;
    }

    public virtual bool CheckForAndTakeCost(float customAmount)
    {
        if (CheckForCost())
        {
            TakeCost(customAmount);
            return true;
        }
        return false;
    }

    public virtual bool CheckForCost()
    {
        if (myPlayer.playerAbilities.solarEnergy >= cost)
        {
            return true;
        }
        return false;
    }

    public virtual void TakeCost()
    {
        myPlayer.playerAbilities.solarEnergy -= cost;
    }

    public virtual void TakeCost(float customCost)
    {
        myPlayer.playerAbilities.solarEnergy -= customCost;
    }

    public virtual IEnumerator UseAbility(string methodToCall)
    {
        yield return new WaitForSeconds(useTime);
        Invoke(methodToCall, 0);
        yield return new WaitForSeconds(activationTime);
        activated = false;
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}
