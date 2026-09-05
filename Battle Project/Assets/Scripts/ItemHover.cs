using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(SphereCollider))]
public class ItemHover : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 0.15f;
    [SerializeField] private float bobAmount = 0.05f;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private LayerMask groundLayer;

    private Vector3 basePosition;
    private float hoverOffset;
    private void Awake()
    {
        basePosition = transform.position;
    }
    public void StartHover(Vector3 dropPosition)
    {
        // Cast from the dropped position downward
        if (Physics.Raycast(
            dropPosition + Vector3.up * 0.5f,
            Vector3.down,
            out RaycastHit hit,
            100f,
            groundLayer))
        {
            basePosition = hit.point + Vector3.up * hoverHeight;
        }
        else
        {
            // No ground found
            basePosition = dropPosition;
        }

        hoverOffset = Random.Range(0f, Mathf.PI * 2f);

        // Immediately move to the correct height
        transform.position = basePosition;
    }

    private void Update()
    {
        float bob = Mathf.Sin(
            Time.time * hoverSpeed + hoverOffset
        ) * bobAmount;

        transform.position =
            basePosition + Vector3.up * bob;

        transform.rotation = Quaternion.Euler(-80f, transform.eulerAngles.y, 0f);
        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.World
        );
    }
}