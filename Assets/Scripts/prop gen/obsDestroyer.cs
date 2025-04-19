using UnityEngine;

public class obsDestroyer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
