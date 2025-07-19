using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Collectable collectable;
    private Collider2D col;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

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
    }

    public void EnemyReset()
    {
        collectable.gameObject.SetActive(true);
        collectable.isCollected = false;
        col.enabled = true;

        foreach (var spr in sprites)
        {
            spr.enabled = true;
        }
    }
}
