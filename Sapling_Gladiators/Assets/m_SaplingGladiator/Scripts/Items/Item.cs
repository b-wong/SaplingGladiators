using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public abstract class Item : MonoBehaviour, IWeapon, IActivatable
{
    public ItemSO myItem;
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    
    public float UseTime { get; set; }
    public float NumOfUses { get; set; }

    public int AttackPower {get; set;}

    public bool Active { get; set; }
    public bool AutoUse = false;

    [HideInInspector]
    public bool ignoreMeInUI;

    public virtual bool OnActivate(Player player)
    {
        if (Active == false)
        {
            if (NumOfUses > 0)
            {
                return true;
                //Start your custom implementation
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual bool OnActivateEnd(Player player)
    {
        NumOfUses--;
        Active = false;

        if (NumOfUses <= 0)
        {
            Destroy(gameObject);
        }
        return true;
    }

    public virtual bool ApplyProperties(ItemSO itemProperties)
    {
        if (itemProperties != null)
        {
            ItemName = itemProperties.itemName;
            ItemDescription = itemProperties.itemDescription;
            UseTime = itemProperties.useTime;
            NumOfUses = itemProperties.numOfUses;
            if (this is IWeapon)
            {
                AttackPower = itemProperties.attackPower;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator StopParticleEffect(float duration, ParticleSystem particleToStop)
    {
        yield return new WaitForSeconds(duration);
        particleToStop.Stop();
    }

    public IEnumerator StopParticleEffect(float duration, List<ParticleSystem> particleToStop)
    {
        yield return new WaitForSeconds(duration);
        foreach(ParticleSystem particle in particleToStop)
        {
            particle.Stop();
        }
    }
}
