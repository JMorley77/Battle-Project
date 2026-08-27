using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private PlayerMovement playerMovement;

    private void Update()
    {
        float targetSpeed = playerMovement.isSprinting ? 1f : 0.5f;

        if (playerMovement.moveInput.magnitude < 0.1f)
        {
            targetSpeed = 0f;
        }

        animator.SetFloat("Speed", targetSpeed, 0.1f, Time.deltaTime);

        animator.SetBool("IsJumping", !playerMovement.isGrounded);
    }

}
