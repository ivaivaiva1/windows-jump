using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public int Order;
    private int initialOrder;
    private List<BoxCollider2D> childBoxColliders = new();
    public List<Collider2D> windowsInContact = new();

    public bool canDrag = true;
    public bool dragging = false;
    private Vector3 offset;

    public bool isAlive = true;

    private RawImage rawImage;

    private void Awake()
    {
        PopulateColliders();
        rawImage = GetComponentInChildren<RawImage>(includeInactive: true);
        initialOrder = Order;
    }

    private void Update()
    {
        if (!canDrag)
        {
            if (dragging) dragging = false;
            LevelController.Instance.isDraggingWindows = false;
            return;
        }

        if (dragging)
        {
            Vector3 mouseWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;
            transform.position = mouseWorldPos + offset;
        }
    }

    private void OnMouseDown()
    {
        if (!canDrag) return;

        Vector3 clickWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;
        offset = transform.position - clickWorldPos;
        WindowsLayerController.Instance.SetWindowAsTop(this);
        dragging = true;
        LevelController.Instance.isDraggingWindows = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
        LevelController.Instance.isDraggingWindows = false;
    }

    public void KillWindow()
    {
        isAlive = false;

        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = 0.5f;
            rawImage.color = color;
        }

        foreach (var col in childBoxColliders)
            col.enabled = false;
    }

    public void ReviveWindow()
    {
        isAlive = true;

        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = 1f;
            rawImage.color = color;
        }

        foreach (var col in childBoxColliders)
            col.enabled = true;
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

    public void SetOrderInLayer(int order)
    {
        Order = order;

        // Sprite
        SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.sortingOrder = order;

        // Canvas
        Canvas canvas = GetComponentInChildren<Canvas>(includeInactive: true);
        if (canvas != null)
            canvas.sortingOrder = order;
    }
}
