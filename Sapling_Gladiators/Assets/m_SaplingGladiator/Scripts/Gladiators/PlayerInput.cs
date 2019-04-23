using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public Player myPlayer;

    [Space]
    [Header("Settings")]
    public int inputTimeForUpUsage; //The time that it takes until an ability is considered to be triggered
    public int inputTimeForBlocking;
    private bool hasHandledInputThisFrame = false;

    //Time is measured in frames
    private int leftInputTime;
    private int rightInputTime;
    private int upInputTime;
    private int downInputTime;
    
    private bool leftInput = false;
    private bool wasLeftInput = false;

    private bool rightInput = false;
    private bool wasRightInput = false;

    private bool upInput = false;
    private bool wasUpInput = false;

    private bool downInput = false;
    private bool wasDownInput = false;

    public bool anyHorizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(false);
    }

    void FixedUpdate() 
    {
        HandleInput(true);
    }

    public void HandleInput(bool isFixedUpdate)
    {
        bool hadAlreadyHandled = hasHandledInputThisFrame;
        hasHandledInputThisFrame = isFixedUpdate;
        if (hadAlreadyHandled) { return; }

        //TODO Step 1: Measure input from player and count time it was there
        MeasureInput();

        //TODO Step 2: Take action based on measure input if no more input is detected
        HandleHorizontalInput();
        HandleVerticalInput();
    }

    public void MeasureInput()
    {
        UpdatePreviousInput();

        float horizontalInputValue_Left = Input.GetAxisRaw(myPlayer.playerHorizontalAxisName_Left);
        float horizontalInputValue_Right = Input.GetAxisRaw(myPlayer.playerHorizontalAxisName_Right);

        float verticalInputValue_Up = Input.GetAxisRaw(myPlayer.playerVerticalAxisName_Up);
        float verticalInputValue_Down = Input.GetAxisRaw(myPlayer.playerVerticalAxisName_Down);

        if (myPlayer.playerController.allowControl)
        {
            //Left Input
            if (horizontalInputValue_Left != 0)
            {
                leftInput = true;
                leftInputTime++;
            }
            else
            {
                leftInput = false;
            }

            //Right Input
            if (horizontalInputValue_Right != 0)
            {
                rightInput = true;
                rightInputTime++;
            }
            else
            {
                rightInput = false;
            }

            //Up Input
            if (verticalInputValue_Up != 0)
            {
                upInput = true;
                upInputTime++;
            }
            else
            {
                upInput = false;
            }

            //Down Input
            if (verticalInputValue_Down != 0)
            {
                downInput = true;
                downInputTime++;
            }
            else
            {
                downInput = false;
            }
        }
        else
        {
            leftInput = false;
            rightInput = false;
            upInput = false;
            downInput = false;

            leftInputTime = 0;
            rightInputTime = 0;
            upInputTime = 0;
            downInputTime = 0;

            anyHorizontalInput = false;
        }

        if (leftInput || rightInput)
        {
            anyHorizontalInput = true;
        }
        else
        {
            anyHorizontalInput = false;
        }
    }

    public void UpdatePreviousInput()
    {
        //At start of frame, check last frame's input. If its different than last Frame, update it now.
        if (wasLeftInput != leftInput)
        {
            wasLeftInput = leftInput;
        }

        if (wasRightInput != rightInput)
        {
            wasRightInput = rightInput;
        }

        if (wasUpInput != upInput)
        {
            wasUpInput = upInput;
        }

        if (wasDownInput != downInput)
        {
            wasDownInput = downInput;
        }
    }

    public void HandleHorizontalInput()
    {
        if (leftInput || rightInput)
        {
            if (leftInput && !rightInput || !leftInput && rightInput) //If there is input on only one side, left or right)
            {
                if (rightInput)
                {
                    myPlayer.playerController.direction = 1;
                    myPlayer.playerController.accelerating = true;
                }
                else if (leftInput)
                {
                    myPlayer.playerController.direction = -1;
                    myPlayer.playerController.accelerating = true;
                }
                myPlayer.playerAbilities.StopBlock();
            }
            else if (leftInput && rightInput) //If input on both sides, trigger blocking ability
            {
                myPlayer.playerController.accelerating = false;
                myPlayer.playerController.decelerating = true;

                if (leftInputTime >= inputTimeForBlocking && rightInputTime >= inputTimeForBlocking)
                {
                    //!Debug.LogWarning("Blocking Ability Triggered");
                    myPlayer.playerAbilities.ActivateBlock();
                }
            }
        }
        else //If no horizontal input, cue movement deceleration and anything else needed
        {
            myPlayer.playerController.accelerating = false;
            myPlayer.playerController.decelerating = true;

            if (myPlayer.playerAbilities.blockAbility.blocking)
            {
                myPlayer.playerAbilities.StopBlock();
            }

            //Reset counters
            leftInputTime = 0;
            rightInputTime = 0;
        }
    }

    public void HandleVerticalInput()
    {
        if (upInput)
        {
            if (upInputTime > inputTimeForUpUsage)
            {
                //Charge tackle
                myPlayer.playerAbilities.ActivateTackle();
                //!Debug.LogError("Charging Tackle!");
            }
        }
        else
        {
            //Since there's no up input, if there was up input it could've only been for using an item
            //as long as the inputTime was less than inputTimeForUpUsage. If its higher, they meant to
            //use Tackle, but stopped charging it.
            if (wasUpInput)
            {
                if (upInputTime < inputTimeForUpUsage)
                {
                    //They meant to activate an item
                    //!Debug.LogWarning("Item used!");
                    myPlayer.playerInventory.UseItem(myPlayer.playerInventory.myCurrentItem);
                }
                else
                {
                    myPlayer.playerAbilities.StopTackle();
                    //Cancel tackle charging if it was happening
                }
            }
            upInputTime = 0;
            downInputTime = 0;
        }

        if (downInput)
        {
            myPlayer.playerAbilities.ActivateDodge();
        }
    }
}
