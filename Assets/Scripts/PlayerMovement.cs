using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5F;
    [SerializeField] float climbSpeed = 5F;
    [SerializeField] State state = State.Idle;


    bool playerHasHorizontalSpeed;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator animator; 
    CapsuleCollider2D capsuleCollider;
    float gravityAtStart;

    enum State
    {
        Idle,
        Run,
        Jump,
        Climb
    }
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityAtStart = myRigidBody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        PlayerState();
        ClimbLadder();
    }

    
    void PlayerState()
    {
        if (playerHasHorizontalSpeed)
        {
            state = State.Run;
        }
        else if(myRigidBody.gravityScale == 0 && 
            myRigidBody.velocity.y != 0)
        {
            state = State.Climb;
        }
        else
        {
            state = State.Idle;
        }
    }

    void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        animator.SetBool("isRunning", (state == State.Run));
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            return;
        }
        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void ClimbLadder()
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravityAtStart;
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0;

        animator.SetBool("isClimbing", (state == State.Climb)); 
    }

}
