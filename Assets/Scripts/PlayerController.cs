using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 10;
    public float airWalkSpeed = 3f;

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;

    public float currentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    //Ground move
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                    /**if(touchingDirections.IsGrounded)
                    {
                       //Ground move
                       if (IsRunning)
                       {
                           return runSpeed;
                       }
                       else
                       {
                          return walkSpeed;
                       }
                       }
                    else
                    {
                        //Air move
                        return airWalkSpeed;
                    } **/
                }
                else
                {
                    // 0 = not moving
                    return 0;
                }
            }
            else
            {
                // 0 = not moving
                return 0;
            }
        }
    }

    Vector2 moveInput;
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get 
        { 
            return _isMoving; 
        } 
        private set 
        { 
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    [SerializeField]
    private bool _isWallSliding = false;
    public bool IsWallSliding
    {
        get
        {
            return _isWallSliding;
        }
        private set
        {
            _isWallSliding = value;
            animator.SetBool(AnimationStrings.isWallSliding, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight { get
        {
            return _isFacingRight;
        } private set { 
            if(_isFacingRight != value)
            {
                //Flip the local scal to make player face oposite
                transform.localScale *= new Vector2(-1, 1); 
            }

            _isFacingRight = value;
        } 
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        if(IsMoving && touchingDirections.IsOnWall)
        {
            IsWallSliding = true;
        }
        else
        {
            IsWallSliding = false;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // return vector (-1,0) if move left, (1,0) if move right
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } private set { 

        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) 
        {
            // Face the right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight) 
        {
            // Face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started) 
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO check if alive
        if (context.started && (touchingDirections.IsGrounded || IsWallSliding) && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x,jumpHeight);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        // TODO check if alive
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
