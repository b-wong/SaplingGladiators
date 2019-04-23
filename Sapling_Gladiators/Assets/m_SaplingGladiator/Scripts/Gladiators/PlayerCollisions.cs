using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    public Player myPlayer;
    public Collider2D playerCollider;

    private void OnEnable() 
    {
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }
    }

#region Collision
 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") 
        {
            myPlayer.playerController.Knockback(myPlayer.playerController.knockbackForce);
            Player otherPlayer = other.gameObject.GetComponent<Player>();
            otherPlayer.playerController.Knockback(otherPlayer.playerController.knockbackForce);

            if (myPlayer.playerAbilities.myActiveAbility.activated)
            {
                if (myPlayer.playerAbilities.myActiveAbility == myPlayer.playerAbilities.blockAbility)
                {
                    myPlayer.playerAbilities.blockAbility.StunEnemyPlayer();
                }
            }
        }
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            if (other.gameObject.GetComponent<Item>())
            {
                myPlayer.playerInventory.AddItem(other.gameObject.GetComponent<Item>());
                other.gameObject.transform.parent = gameObject.transform;
            } 
        }

        if (other.CompareTag("Attack"))
        {
            if (myPlayer.enemyPlayer.playerInventory.myCurrentItem != null)
            {
                if (myPlayer.enemyPlayer.playerInventory.myCurrentItem.GetComponent<LeafAxe>())
                {
                    myPlayer.enemyPlayer.playerInventory.myCurrentItem.gameObject.GetComponent<LeafAxe>().myPlayer = myPlayer.enemyPlayer;
                    myPlayer.enemyPlayer.playerInventory.myCurrentItem.gameObject.GetComponent<LeafAxe>().AxeHit();
                }
            }
        }

        if (other.CompareTag("Tackle"))
        {
            if (myPlayer.enemyPlayer.playerInventory.myCurrentItem != null)
            {
                if (myPlayer.enemyPlayer.playerInventory.myCurrentItem.gameObject.GetComponentInChildren<LeafSword>())
                {
                    myPlayer.playerHealth.Damage(myPlayer.enemyPlayer.playerAbilities.tackleAbility.tackleDamage + myPlayer.enemyPlayer.playerInventory.myCurrentItem.gameObject.GetComponentInChildren<LeafSword>().AttackPower);
                }
                else
                {
                    myPlayer.playerHealth.Damage(myPlayer.enemyPlayer.playerAbilities.tackleAbility.tackleDamage);
                }  
            }
            else
            {
                myPlayer.playerHealth.Damage(myPlayer.enemyPlayer.playerAbilities.tackleAbility.tackleDamage);
            }
        }
    }
 
#endregion
}
