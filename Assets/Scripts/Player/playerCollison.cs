/*
using UnityEngine;

public class playerCollison : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Animator animator;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float adjustChangeMoveSpeedAmount = -2f;
    const string hitString = "Hit";
    float cooldownCounter = 0f;
    
    LevelCooker levelGenerator;

    void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelCooker>();
    }
    void Update()
    {
        cooldownCounter += Time.deltaTime;
    }
    void OnCollisionEnter(Collision other) => HandleHit(other.gameObject);
    void OnTriggerEnter(Collider other) => HandleHit(other.gameObject);

    void HandleHit(GameObject other)
    {
        if (!other.CompareTag("Obstacle")) return;
        if (cooldownCounter < cooldown) return;
        levelGenerator.changeChunkSpeed(adjustChangeMoveSpeedAmount);
        animator.SetTrigger(hitString);
        cooldownCounter = 0f;
    }
}
*/

using UnityEngine;

public class playerCollison : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float adjustChangeMoveSpeedAmount = -2f;
    const string hitString = "Hit";
    float cooldownCounter = 0f;

    ILevelCooker levelGenerator;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        levelGenerator = FindFirstObjectByType<IslandLevelCooker>() as ILevelCooker
                      ?? FindFirstObjectByType<LevelCooker>() as ILevelCooker;
    }

    void Update()
    {
        cooldownCounter += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other) => HandleHit(other.gameObject);
    void OnTriggerEnter(Collider other) => HandleHit(other.gameObject);

    void HandleHit(GameObject other)
    {
        if (!other.CompareTag("Obstacle")) return;
        if (cooldownCounter < cooldown) return;
        
        if (levelGenerator != null)
        {
            levelGenerator.ChangeSpeed(adjustChangeMoveSpeedAmount);
        }

        animator.SetTrigger(hitString);
        cooldownCounter = 0f;
    }
}