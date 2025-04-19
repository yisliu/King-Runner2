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
    void OnCollisionEnter(Collision other)
    {
        if (cooldownCounter < cooldown)
        {
            return;
        }

        levelGenerator.changeChunkSpeed(adjustChangeMoveSpeedAmount);
        animator.SetTrigger(hitString);
        cooldownCounter = 0f;
    }
}
