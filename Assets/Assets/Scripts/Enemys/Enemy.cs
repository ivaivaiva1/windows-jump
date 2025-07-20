using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool startReseting = false;
    private Collectable collectable;
    private Collider2D col;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] private GameObject smoke;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();
        col = GetComponent<Collider2D>();
        sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    public void EnemyCollect()
    {
        collectable.isCollected = true;
        collectable.thisWindow.IamCollected();
        col.enabled = false;

        foreach (var spr in sprites)
        {
            spr.enabled = false;
        }

        smoke.SetActive(true);
    }

    public void EnemyReset()
    {
        if (!collectable.isCollected) return;
        collectable.isCollected = false;
        startReseting = true;
        foreach (var spr in sprites)
        {
            spr.enabled = true;
        }
        smoke.SetActive(true);
    }

    public void SmokeIsFinish() 
    {
        startReseting = false;
        col.enabled = true;
    }

}
