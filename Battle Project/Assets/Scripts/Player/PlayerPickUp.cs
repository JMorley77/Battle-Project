using UnityEngine;


public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private Transform playerHand;
    [SerializeField] private GameObject weaponPrefab;

    private GameObject equippedWeapon;

    private void Update()
    {
        if (inputController.pickUpAction.action.triggered)
        {
            if (equippedWeapon == null)
                PickUp();
            else
                PutDown();
        }
    }

    private void PickUp()
    {
        equippedWeapon = Instantiate(weaponPrefab, playerHand);
        AlignToHand(equippedWeapon.transform);
    }

    private void PutDown()
    {
        Destroy(equippedWeapon);
        equippedWeapon = null;
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

        weapon.localScale = Vector3.one;
    }
}

