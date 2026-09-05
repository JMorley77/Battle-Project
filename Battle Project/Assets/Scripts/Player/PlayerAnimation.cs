using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerPickUp playerPickUp;
    private bool attackAnimationPlaying;
    private float targetSpeed;

    private void Update()
    {


        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);

        animator.SetBool("IsJumping", !playerMovement.isGrounded);

        if (playerPickUp.equippedWeapon == null)
        {
            animator.ResetTrigger("IsAttacking");
        }

        if (attackAnimationPlaying)
        {
            targetSpeed = 0.6f;
        }
        else
        {
            targetSpeed = playerMovement.isSprinting ? 1f : 0.5f;
        }

        if (playerMovement.moveInput.magnitude < 0.1f)
        {
            targetSpeed = 0f;
        }


        if (playerCombat.isAttacking && playerMovement.isSprinting)
        {
            playerMovement.sprintSpeed = playerMovement.attackSprintSpeed;
        }

        if (playerCombat.isAttacking)
        {
            attackAnimationPlaying = true;
            animator.SetTrigger("IsAttacking");
            playerCombat.isAttacking = false;
        }

        if (playerCombat.isDead)
        {
            DeathAnimation();
        }

    }
    
    public void DeathAnimation()
    {
        animator.SetTrigger("IsDead");
    }
    public void AttackFinished()
    {
        attackAnimationPlaying = false;
        playerMovement.sprintSpeed = playerMovement.origionalSprintSpeed;
    }

}
