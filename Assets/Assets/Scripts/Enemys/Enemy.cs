using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Collectable collectable;
    private Collider2D col;
    private SpriteRenderer sprite;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void EnemyCollect()
    {
        collectable.isCollected = true;
        collectable.thisWindow.IamCollected();
        col.enabled = false;
        sprite.enabled = false;
    }

    public void EnemyReset()
    {
        collectable.gameObject.SetActive(true);
        collectable.isCollected = false;
        col.enabled = true;
        sprite.enabled = true;
    }

}
