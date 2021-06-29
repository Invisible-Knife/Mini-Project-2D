using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    public Player_Controller controller;
    public Animator animator;

    float walkSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * walkSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJump", true);
        }
        if(Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJump", false);
    }

    public void onCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouch", isCrouching);
    }

    private void FixedUpdate()
    {
        // Move the player
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
