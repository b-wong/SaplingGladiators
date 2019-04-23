using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //! Class to hold references to other functionality inside of player
    
    public PlayerInput playerInput;
    public Health playerHealth;
    public PlayerController playerController;
    public PlayerAbilities playerAbilities;
    public Inventory playerInventory;
    public PlayerCollisions playerCollisions;
    public PlayerSounds playerSounds;
    public PlayerUI playerUI;
    public SpriteRenderer sprite;
    public string playerNumber;
    public TrailRenderer playerTrail;

    [Space]

    public Player enemyPlayer;

    [Space]

    public Color playerColor;
    public string playerHorizontalAxisName_Left;
    public string playerHorizontalAxisName_Right;
    public string playerVerticalAxisName_Up;
    public string playerVerticalAxisName_Down;

    [Space(20)]
    public GameObject tackleSpot;

    private void OnEnable() {
        if (playerInput == null)
        {
            playerInput = GetComponentInChildren<PlayerInput>();
        }
        if (playerHealth == null)
        {
            playerHealth = GetComponentInChildren<Health>();
        }
        if (playerController == null)
        {
            playerController = GetComponentInChildren<PlayerController>();
        }
        if (playerAbilities == null)
        {
            playerAbilities = GetComponentInChildren<PlayerAbilities>();
        }
        if (playerInventory == null)
        {
            playerInventory = GetComponentInChildren<Inventory>();
        }
        if (playerCollisions == null)
        {
            playerCollisions = GetComponentInChildren<PlayerCollisions>();
        }
        if (playerSounds == null)
        {
            playerSounds = GetComponentInChildren<PlayerSounds>();
        }
        if (playerUI == null)
        {
            playerUI = GetComponentInChildren<PlayerUI>();
        }
        if (sprite == null)
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
        if (playerTrail == null)
        {
            playerTrail = GetComponentInChildren<TrailRenderer>();
        }
        
        sprite.color = playerColor;
    }
}
