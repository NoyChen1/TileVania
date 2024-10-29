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
    [SerializeField] State state = State.Idle;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;


    bool playerHasHorizontalSpeed;
    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator animator; 
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetColider;
    float gravityAtStart;

    enum State
    {
        Idle,
        Run,
        Jump,
        Climb,
        Dead
    }
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetColider = GetComponent<BoxCollider2D>();
        gravityAtStart = myRigidBody.gravityScale;
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
        if (state == State.Dead) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (state == State.Dead) { return; }

        if (!myFeetColider.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return; }
        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void ClimbLadder()
    {
        if (!myFeetColider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidBody.gravityScale = gravityAtStart;
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0;

        animator.SetBool("isClimbing", (state == State.Climb)); 
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            state = State.Dead;
            animator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
        }
    }

    void OnFire(InputValue value)
    {
        if (state == State.Dead) { return; }
        Instantiate(bullet, bulletSpawn.position, transform.rotation);
    }

}
