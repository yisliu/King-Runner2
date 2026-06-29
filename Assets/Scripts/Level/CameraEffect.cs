using UnityEngine;
using DG.Tweening;

public class CameraEffect : MonoBehaviour
{
    public static CameraEffect Instance { get; private set; }

    [SerializeField] private float EffectAngle = 1.5f;
    [SerializeField] private float Duration = 4f;

    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalPosition = transform.localPosition;

        transform.DOBlendableLocalRotateBy(new Vector3(0, EffectAngle, 0), Duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        transform.DOBlendableLocalRotateBy(new Vector3(EffectAngle * 0.5f, 0, 0), Duration * 1.3f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void TriggerShake(float duration = 0.4f, float strength = 0.5f, int vibrato = 14)
    {
        transform.DOComplete();
        transform.localPosition = originalPosition;
        transform.DOShakePosition(duration, strength, vibrato, 90f, false, true)
            .OnComplete(() => transform.localPosition = originalPosition);
    }
}
