using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerPickUp playerPickUp;
    public bool attackAnimationPlaying;
    private float targetSpeed;

    private void Update()
    {


        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);

        animator.SetBool("IsJumping", !playerMovement.isGrounded);

        if (playerPickUp.isHolding)
        {
            animator.SetBool("IsPickingUp", true);
        }
        else
        {
            animator.SetBool("IsPickingUp", false);
        }


        if (playerPickUp.equippedWeapon == null && attackAnimationPlaying)
        {
            StopAttackAnimation();
        }


        #region Movement Speed Animation Blend Tree
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
        #endregion

        #region Attack Animation
        //play animation if the player is attacking and the attack animation is not already playing
        if (playerCombat.isAttacking && !attackAnimationPlaying) // the !attackanimationplaying plays 1 animaiton not the combo
        {
            attackAnimationPlaying = true;
            ComboAnimation();
        }
        #endregion


        #region Death Animation
        if (playerCombat.isDead)
        {
            DeathAnimation();
        }
        #endregion
    }
    #region Death Animation 
    public void DeathAnimation()
    {
        animator.SetTrigger("IsDead");
    }
    #endregion

    public void ComboAnimation() 
    {
        switch(playerCombat.currentCombo)
        {
            case 1:
                animator.SetTrigger("IsAttacking");
                AttackFinished();
                break;
            case 2:
                animator.SetTrigger("Attack2");
                AttackFinished();
                break;
            case 3:
                animator.SetTrigger("Attack3");
                AttackFinished();
                break;
            default:
                break;
        }
    }
    public void AttackFinished()
    {
        attackAnimationPlaying = false;
        playerCombat.isAttacking = false; 
        Debug.Log("Attack animation finished");
    }

    public void StopAttackAnimation()
    {
        if (!attackAnimationPlaying)
            return;
        attackAnimationPlaying = false;
        playerCombat.isAttacking = false;
        playerMovement.sprintSpeed = playerMovement.origionalSprintSpeed;
        animator.ResetTrigger("IsAttacking");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
    }
    
}
