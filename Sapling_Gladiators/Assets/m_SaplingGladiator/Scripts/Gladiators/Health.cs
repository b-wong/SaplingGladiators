using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    public Player myPlayer;

    public OnHealthChanged onHealthChanged = new OnHealthChanged();

    public int health;
    public int maxHealth;
    public bool invulnerable;
    public GameObject blockEffect;
    public GameObject deathEffect;
    private GameObject spawnedEffect;

    void Awake()
    {
        if (onHealthChanged == null)
        {
            onHealthChanged = new OnHealthChanged();
        }

        if (blockEffect == null)
        {
            blockEffect = myPlayer.playerAbilities.blockAbility.blockEffect;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        if (!invulnerable)
        {
            health -= damage;
            if (Random.value < 0.25)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sapling_Sad");
            }
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SaplingHurt");
            if (health <= 0)
            {
                //! This seed is dead
                OnDeath();
            }
            onHealthChanged.Invoke(transform, -damage);
        }
        else
        {
            GameObject newBlockEffect = Instantiate(blockEffect, transform.position, Quaternion.identity, transform);
        }   
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (Random.value < 0.25)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sapling_Cheers");
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/SaplingHeal");
        
        onHealthChanged.Invoke(transform, amount);
    }

    public void OnDeath()
    {
        spawnedEffect = Instantiate(deathEffect, transform.position, Quaternion.identity, transform);
        myPlayer.sprite.color = new Color(0,0,0,0.5f);
        EnemyIsWinner();
        Destroy(spawnedEffect, 2f);
    }
    
    public void EnemyIsWinner()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Dialogue/Sapling_Cheer_IWin");
    }
}

public class OnHealthChanged : UnityEvent<Transform, float> { }
