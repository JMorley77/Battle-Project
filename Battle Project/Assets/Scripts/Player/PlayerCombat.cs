using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private InputController input;

    void Update()
    {
        if (input.attackAction.action.triggered)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Implement attack logic here
    }
}
