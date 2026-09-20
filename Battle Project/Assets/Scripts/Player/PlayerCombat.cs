using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private InputController input;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerPickUp pickUp;

    [Header("Health Settings")]
    [SerializeField][Min(0)] private int maxHealth = 100;
    [SerializeField][Min(0)] private float currentHealth;
    [SerializeField][Min(0)] private float healthRegenRate = 1f;

    [Header("Attack Settings")]
    [SerializeField][Min(0)] private int attackDamage = 10;
    [SerializeField][Min(0)] private float attackRange = 1;
    [SerializeField][Min(0)] private float attackSpeed = 1f;

    [Header("Attack Combo Settings")]
    [SerializeField][Min(0)] private int maxCombo = 3;
    [SerializeField][Min(0)] public int currentCombo = 0;
    [SerializeField][Min(0)] private float comboResetTime = 1f;
    [SerializeField][Min(0)] private float comboTimer = 1.2f;

    [Header("Enemy Detection")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackRadius = 0.75f;


    public bool isAttacking = false;
    public bool isDead = false;
    public bool canCombo = false;

    void Update()
    {
        
        comboTimer -= Time.deltaTime;
        comboTimer = Mathf.Clamp(comboTimer, 0, comboResetTime);

        if (currentHealth <= 0 && !isDead)
        {
            Death();
            return;
        }        
        if(isDead)
        {
            return;
        }

        if (input.attackAction.action.triggered && player.isGrounded && pickUp.equippedWeapon != null)
        {
            Attack();
            Combo();
        }
        // heals you until 80 health if you are not dead and below max health
        if (currentHealth < maxHealth && !isDead)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, 80);
        }
    }

    private void Attack()
    {
        isAttacking = true;

        // Position the detection area in front of the player
        Vector3 attackPosition = transform.position + transform.forward * attackRange;

        // Find everything inside the attack area
        Collider[] enemies = Physics.OverlapSphere(
            attackPosition,
            attackRadius,
            enemyLayer
        );

        foreach (Collider enemy in enemies)
        {

            Debug.Log("Enemy hit: " + enemy.name);

            // Deal damage here later
            // enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }

    public void Combo()
    {
        if(!isAttacking)
            return;

        if(comboTimer <= 0)
        {
            currentCombo = 0;
            comboTimer = comboResetTime;
            Debug.Log("Combo reset");
        }
        currentCombo++;
        if (currentCombo > maxCombo)
        {
            currentCombo = 1;
        }
        Debug.Log("Attack triggered. Current combo: " + "<color=green>" + "<b>" + currentCombo + "</b>" + "</color>");
    }





    public void Death()
    {
        isDead = true;
        Debug.Log("Player is dead");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 attackPosition =
            transform.position + transform.forward * attackRange;

        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }

}
