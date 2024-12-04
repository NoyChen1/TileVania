using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5F;
    [SerializeField] float climbSpeed = 5F;
    [SerializeField] State state;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);


    bool playerHasHorizontalSpeed;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetColider;
    float gravityAtStart;

    int groundMask;
    int climbingMask;
    int enemiesHazardsMask;


    public enum State
    {
        Idle,
        Run,
        Jump,
        Climb,
        Dead
    }

    void Start()
    {
        state = State.Idle;
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetColider = GetComponent<BoxCollider2D>();
        gravityAtStart = myRigidBody.gravityScale;

        groundMask = LayerMask.GetMask("Ground");
        climbingMask = LayerMask.GetMask("Climbing");
        enemiesHazardsMask = LayerMask.GetMask("Enemies", "Hazards");
    }

    void Update()
    {
        if (state == State.Dead) { return; }
        Run();
        FlipSprite();
        PlayerState();
        ClimbLadder();
        Die();
    }

    void PlayerState()
    {
        if (playerHasHorizontalSpeed)
        {
            state = State.Run;
        }
        else if (myRigidBody.gravityScale == 0 &&
            (myRigidBody.velocity.y == 5 || myRigidBody.velocity.y == -5))
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
        if (state == State.Dead) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (state == State.Dead) { return; }

        if (!myFeetColider.IsTouchingLayers(groundMask)) { return; }
        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetColider.IsTouchingLayers(climbingMask))
        {
            myRigidBody.gravityScale = gravityAtStart;
            return;
        }

        if(Mathf.Abs(moveInput.y) > 0.1f)
        {
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0;
        }
        else
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
        }

        /*
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0;
        */

        animator.SetBool("isClimbing", (state == State.Climb));
    
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(enemiesHazardsMask))
        {
            state = State.Dead;
            animator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            GameSession.Instance.ProcessPlayerDeath();
        }

    }

    public State getState() { return state; }
}
