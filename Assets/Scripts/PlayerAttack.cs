using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCoolDown;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject[] fireBalls;

    PlayerMovement PlayerMovement;
    float coolDownTimer = Mathf.Infinity;

    void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        coolDownTimer += Time.deltaTime;
    }

    void OnFire(InputValue value)
    {
        if (PlayerMovement.getState() == PlayerMovement.State.Dead) { return; }
        if (coolDownTimer > attackCoolDown)
        {
            Attack();
        }

    }

    void Attack()
    {
        coolDownTimer = 0;

        fireBalls[FindFireBall()].transform.position = firePoint.position;
        fireBalls[FindFireBall()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    int FindFireBall()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}

