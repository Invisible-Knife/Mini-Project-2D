using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player_Controller controller;
    public int health = 100;
    public Animator animator;

    float walkSpeed = 10f;
    float horizontalMove = 0f;
    int count = 0;

    private void Start()
    {
        animator.SetFloat("Health", health);
    }

    private void Update()
    {
        if (health > 0)
        {
            if (count <= 200)
            {
                //Idle state
                horizontalMove = 0 * walkSpeed;
                count++;
            }
            else if (count <= 360)
            {
                // Walk right
                horizontalMove = 1 * walkSpeed;
                count++;
            }
            else if (count <= 560)
            {
                horizontalMove = 0 * walkSpeed;
                count++;
            }
            else if (count <= 720)
            {
                horizontalMove = -1 * walkSpeed;
                count++;
            }
            if (count == 720)
                count = 0;
        }
        else
        {
            horizontalMove = 0f;
        }
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetFloat("Health", health);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_die"))
        { 
            Destroy(gameObject); 
        }

    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
    }
}
