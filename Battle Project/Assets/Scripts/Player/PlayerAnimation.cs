using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerPickUp playerPickUp;

    public bool attackAnimationPlaying;
    private float targetSpeed;
    private int lastPlayedCombo = 0;

    private void Update()
    {
        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);
        animator.SetBool("IsJumping", !playerMovement.isGrounded);
        animator.SetBool("IsPickingUp", playerPickUp.isHolding);

        if (playerPickUp.equippedWeapon == null && attackAnimationPlaying)
        {
            StopAttackAnimation();
        }

        // play next animation whenever currentCombo increases to a new step
        if (playerCombat.isAttacking && playerCombat.currentCombo != lastPlayedCombo)
        {
            attackAnimationPlaying = true;
            lastPlayedCombo = playerCombat.currentCombo;
            ComboAnimation();
        }

        if (playerCombat.isDead)
        {
            DeathAnimation();
        }

        Movement();
    }

    #region Movement
    public void Movement()
    {
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

    }
    #endregion

    #region Attack Animation
    public void ComboAnimation()
    {
        switch (playerCombat.currentCombo)
        {
            case 1:
                animator.SetTrigger("IsAttacking");
                break;
            case 2:
                animator.SetTrigger("Attack2");
                break;
            case 3:
                animator.SetTrigger("Attack3");
                break;
        }
    }
    #endregion

    #region Attack Animation Finish
    // used in animation event at the end of the swing clip
    public void AttackFinished()
    {
        attackAnimationPlaying = false;
        playerCombat.isAttacking = false;
        playerCombat.currentCombo = 0; // Reset combo count in combat script
        lastPlayedCombo = 0;
    }

    public void StopAttackAnimation()
    {
        attackAnimationPlaying = false;
        playerCombat.isAttacking = false;
        playerCombat.currentCombo = 0;
        lastPlayedCombo = 0;

        playerMovement.sprintSpeed = playerMovement.origionalSprintSpeed;

        animator.ResetTrigger("IsAttacking");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
    }
    #endregion

    #region Death Animation
    public void DeathAnimation()
    {
        animator.SetTrigger("IsDead");
    }
    #endregion
}