using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    public InputActionReference attackAction;
    public InputActionReference pickUpAction;


    public void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        attackAction.action.Enable();
        pickUpAction.action.Enable();

    }

    public void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        attackAction.action.Disable();
        pickUpAction.action.Disable();
    }


}
