using UnityEngine;
using DG.Tweening;
public class ShipAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Animation Settings")]
    [SerializeField] private float animationDistance = 0.2f;
    [SerializeField] private float animationDuration = 0.2f;
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationRange = new Vector3(1f, 1f, 2f);

    [SerializeField] private float rotationDuration = 3f;
    
    void Start()
    {
        transform.DOMoveY(transform.position.y +  animationDistance, animationDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        transform.DOBlendableLocalRotateBy(rotationRange, rotationDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}
