using UnityEngine;

public class ShipArrival : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movement References")]
    [SerializeField] private float flyingSpeed = 20f;

    [SerializeField] private Vector3 spawnOffset = new Vector3(10f, 20f, 0f);
    [Header("Hover Adjustments")]
    [SerializeField] private float hoverSpeed = 2f;

    [SerializeField] private float hoverHeight = 0.5f;
    private Vector3 targetPosition;
    private Vector3 intialPosition;
    private bool isMoving = false;
    private bool isHovering = false;
    private float hoverTimer = 0f;

    public void startArrival()
    {
        targetPosition = transform.position;
        transform.position = targetPosition + spawnOffset;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flyingSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
                isHovering = true;
                intialPosition = transform.position;
            }
        }
        else if (isHovering)
        {
            hoverTimer += Time.deltaTime*hoverSpeed;
            float newY = intialPosition.y + (Mathf.Sin(hoverTimer)*hoverHeight);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
