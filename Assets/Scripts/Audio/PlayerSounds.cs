using UnityEngine;

[RequireComponent(typeof(PlayerPhysicalProgress))]
public class PlayerSounds : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip pantingClip;

    [Header("Panting")]
    [SerializeField] private float pantingDelay = 10f;

    private AudioSource pantingSource;
    private PlayerPhysicalProgress movement;
    private bool wasGrounded;
    private float runTimer;
    private bool isPanting;

    void Awake()
    {
        movement = GetComponent<PlayerPhysicalProgress>();

        pantingSource = gameObject.AddComponent<AudioSource>();
        pantingSource.clip = pantingClip;
        pantingSource.loop = true;
        pantingSource.spatialBlend = 0f;
    }

    void Update()
    {
        bool grounded = movement.Grounded;

        if (!wasGrounded && grounded && footstepClip != null)
            AudioManager.Instance?.PlaySFX(footstepClip);

        wasGrounded = grounded;

        runTimer += Time.deltaTime;
        if (!isPanting && runTimer >= pantingDelay && pantingClip != null)
        {
            isPanting = true;
            pantingSource.Play();
        }
    }

    public void StopPanting()
    {
        pantingSource.Stop();
        isPanting = false;
        runTimer = 0f;
    }
}