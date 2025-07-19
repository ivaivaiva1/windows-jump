using UnityEngine;
using DG.Tweening;

public class FlyingPatrol : MonoBehaviour
{
    [SerializeField] private bool moveVertical;
    [SerializeField] private float distance;
    [SerializeField] private float loopDuration;
    [SerializeField] private bool needFlip;
    [SerializeField] private Ease easeType;

    private Vector3 startLocalPosition;
    private Transform spriteTransform;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        spriteTransform = transform;
        StartPatrol();
    }

    void StartPatrol()
    {
        Vector3 endLocalPosition = moveVertical
            ? startLocalPosition + new Vector3(0, distance, 0)
            : startLocalPosition + new Vector3(distance, 0, 0);

        transform.DOLocalMove(endLocalPosition, loopDuration / 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(easeType)
            .OnStepComplete(FlipIfNeeded);
    }

    void FlipIfNeeded()
    {
        if (!needFlip || moveVertical) return;

        Vector3 scale = spriteTransform.localScale;
        scale.x *= -1;
        spriteTransform.localScale = scale;
    }
}
