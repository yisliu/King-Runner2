using UnityEngine;
using DG.Tweening;

public class HUDEntrance : MonoBehaviour
{
    [System.Serializable]
    public class Entry
    {
        public RectTransform rect;
        public Vector2 slideOffset = new Vector2(0f, 120f); // direction to slide FROM
        public float delay = 0f;
        public float duration = 0.55f;
        public Ease ease = Ease.OutBack;
    }

    [SerializeField] private Entry[] elements;

    void Start()
    {
        foreach (Entry e in elements)
        {
            if (e.rect == null) continue;
            Vector2 finalPos = e.rect.anchoredPosition;
            e.rect.anchoredPosition = finalPos + e.slideOffset;
            e.rect.DOAnchorPos(finalPos, e.duration)
                  .SetDelay(e.delay)
                  .SetEase(e.ease);
        }
    }
}