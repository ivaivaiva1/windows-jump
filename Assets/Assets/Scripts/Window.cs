using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    private List<BoxCollider2D> childBoxColliders = new List<BoxCollider2D>();

    public List<Collider2D> windowsInContact = new List<Collider2D>();

    public bool canDrag = true;
    public bool dragging = false;
    private Vector3 offset;

    public bool isAlive = true;

    private void Awake()
    {
        PopulateSprites();
        PopulateColliders();
    }

    private void Update()
    {
        if (!canDrag)
        {
            if (dragging) dragging = false;
            return;
        }

        if (dragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos + offset;
        }
    }

    private void OnMouseDown()
    {
        if (!canDrag) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        offset = transform.position - mousePos;
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void KillWindow()
    {
        isAlive = false;

        foreach (var sprite in allSprites)
        {
            Color color = sprite.color;
            color.a = 0.3f;
            sprite.color = color;
        }

        foreach (var col in childBoxColliders)
        {
            col.enabled = false;
        }
    }

    private void ReviveWindow()
    {
        isAlive = true;

        foreach (var sprite in allSprites)
        {
            Color color = sprite.color;
            color.a = 1f;
            sprite.color = color;
        }

        foreach (var col in childBoxColliders)
        {
            col.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Window"))
        {
            if (!windowsInContact.Contains(collision))
            {
                windowsInContact.Add(collision);
            }

            if (dragging)
            {
                if (LevelController.Instance.playerWindow == this)
                {
                    collision.GetComponent<Window>().KillWindow();
                }
                else 
                {
                    KillWindow();
                }      
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Window"))
        {
            if (windowsInContact.Contains(collision))
            {
                windowsInContact.Remove(collision);
            }

            if (windowsInContact.Count == 0)
            {
                ReviveWindow();
            }
        }
    }

    private void PopulateSprites()
    {
        allSprites.Clear();

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        allSprites.AddRange(sprites);
    }

    private void PopulateColliders()
    {
        childBoxColliders.Clear();

        BoxCollider2D[] allBoxColliders = GetComponentsInChildren<BoxCollider2D>(includeInactive: true);

        foreach (BoxCollider2D col in allBoxColliders)
        {
            if (col.gameObject != this.gameObject)
            {
                childBoxColliders.Add(col);
            }
        }
    }
}
