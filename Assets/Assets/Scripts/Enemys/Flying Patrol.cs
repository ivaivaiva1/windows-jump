using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class FlyingPatrol : MonoBehaviour
{
    [SerializeField] private bool moveVertical;
    [SerializeField] private float distance;
    [SerializeField] private float loopDuration;
    [SerializeField] private bool needFlip;
    [SerializeField] private Ease easeType;

    private Vector3 startLocalPosition;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    void Start()
    {
        startLocalPosition = transform.localPosition;

        // Coletar todos os SpriteRenderers que não têm o componente Smoke em si ou em seus pais
        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in allSprites)
        {
            if (sr.GetComponentInParent<Smoke>() == null)
            {
                spriteRenderers.Add(sr);
            }
        }

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

        foreach (var sr in spriteRenderers)
        {
            sr.flipX = !sr.flipX;
        }
    }
}
