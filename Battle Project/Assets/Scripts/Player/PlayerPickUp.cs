using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private Transform playerHand;

    [Header("Hold Settings")]
    [SerializeField] private float holdDuration = 1f;

    private float holdTimer; 
    private bool isHolding;


    private GameObject nearbyWeapon;
    public GameObject equippedWeapon;
    private ItemHover itemHover;

    private void Update()
    { 
        if (isHolding)
        { 
            holdTimer += Time.deltaTime; 
            
            if (holdTimer >= holdDuration) 
            { 
                isHolding = false; holdTimer = 0f; Interact(); 
            } 
        } 
    }


    private void OnEnable() 
    { 
        inputController.pickUpAction.action.started += StartHolding; 
        inputController.pickUpAction.action.canceled += StopHolding; 
    }
    private void OnDisable() 
    { 
        inputController.pickUpAction.action.started -= StartHolding; 
        inputController.pickUpAction.action.canceled -= StopHolding; 
    }
    private void StartHolding(InputAction.CallbackContext context)
    { 
        if (equippedWeapon == null && nearbyWeapon != null) 
        { 
            isHolding = true; holdTimer = 0f; 
        }  
        else if (equippedWeapon != null) 
        { 
            isHolding = true; holdTimer = 0f; 
        } 
    }
    private void StopHolding(InputAction.CallbackContext context)
    {
        if (isHolding) 
        {
            isHolding = false; holdTimer = 0f; 
        }
    }
    private void Interact()
    {
        if (equippedWeapon == null)
        {
            PickUp();
        }
        else
        {
            PutDown();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon") && equippedWeapon == null)
        {
            nearbyWeapon = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }
    private void PickUp()
    {
        if (nearbyWeapon == null)
        {
            Debug.Log("No weapon nearby.");
            return;
        }

        equippedWeapon = nearbyWeapon;

        equippedWeapon.transform.SetParent(playerHand);

        AlignToHand(equippedWeapon.transform);
        itemHover = equippedWeapon.GetComponent<ItemHover>();

        // Disable hovering
        if (itemHover != null)
            itemHover.enabled = false;
        nearbyWeapon = null;
    }

    private void PutDown()
    {
        Vector3 dropPosition = equippedWeapon.transform.position;

        equippedWeapon.transform.SetParent(null);

        itemHover.enabled = true;
        itemHover.StartHover(dropPosition);

        equippedWeapon = null;
        itemHover = null;
    }

    private void AlignToHand(Transform weapon)
    {
        Transform grip = weapon.Find("GripPoint");
        if (grip != null)
        {
            weapon.localPosition = -grip.localPosition;
            weapon.localRotation = Quaternion.Inverse(grip.localRotation);
        }
        else
        {
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
        }

        weapon.localScale = Vector3.one * 0.001f;
    }

}

