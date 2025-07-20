using UnityEngine;

public class SmokeModelated : MonoBehaviour
{

    public void SmokeSetUp(Vector3 spawnPosition, Vector3 scale, int orderInLayer, int targetOrderingLayer, Window window_parent, float animSpeed = 0.5f)
    {

        transform.position = spawnPosition;
        transform.localScale = scale;

        if(window_parent != null)
        {
            transform.SetParent(window_parent.transform);
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = animSpeed;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = orderInLayer;
            sr.sortingLayerID = targetOrderingLayer;
        }
    }


    public void OnSmokeAnimationEnd()
    {
        Destroy(this.gameObject);
    }
}
