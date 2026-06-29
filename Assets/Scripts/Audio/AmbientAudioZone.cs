using UnityEngine;

public class AmbientAudioZone : MonoBehaviour
{
    [SerializeField] private AudioClip ambientClip;

    void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayAmbient(ambientClip);
    }
}