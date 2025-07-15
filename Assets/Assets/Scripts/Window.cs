using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [Header("Câmera da janela (usada no clique apenas)")]
    public Camera windowCamera;

    private List<SpriteRenderer> allSprites = new();
    private List<BoxCollider2D> childBoxColliders = new();

    public List<Collider2D> windowsInContact = new();

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
            // Usa o valor atualizado da MainCamera
            Vector3 mouseWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;
            transform.position = mouseWorldPos + offset;
        }
    }

    private void OnMouseDown()
    {
        if (!canDrag) return;

        // Usa a MainCamera para pegar a posição do mouse no momento do clique
        Vector3 clickWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;

        offset = transform.position - clickWorldPos;
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
        if (!collision.CompareTag("Window")) return;

        if (!windowsInContact.Contains(collision))
            windowsInContact.Add(collision);

        if (dragging)
        {
            if (LevelController.Instance.playerWindow == this)
                collision.GetComponent<Window>()?.KillWindow();
            else
                KillWindow();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Window")) return;

        if (windowsInContact.Contains(collision))
            windowsInContact.Remove(collision);

        if (windowsInContact.Count == 0)
            ReviveWindow();
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
                childBoxColliders.Add(col);
        }
    }
}
