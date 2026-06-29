using UnityEngine;

public abstract class pickUp : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] private AudioClip collectSound;
    const string playerString = "Player";

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            AudioManager.Instance?.PlaySFX(collectSound);
            pickUpEffect();
            Destroy(gameObject);
        }
    }
    
    protected abstract void pickUpEffect();
}
