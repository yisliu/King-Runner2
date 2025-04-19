using UnityEngine;

public abstract class pickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float rotationSpeed = 100f;
    const string playerString = "Player";

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            pickUpEffect();
            Destroy(gameObject);
        }
        
    }
    
    protected abstract void pickUpEffect();
}
