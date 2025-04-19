using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //[SerializeField] private float checkpointTimeExtension = 5f;
    gameManager GameManager;
    const string playerTag = "Player";
    
    void Start()
    {
        GameManager = FindFirstObjectByType<gameManager>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("(o 9 0 9)");

        if (other.CompareTag(playerTag))
        {
            Debug.Log("( 9 0 9)");
            GameManager.IncreaseTime(5f);
        }
    }
}
