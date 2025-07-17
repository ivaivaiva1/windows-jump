using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public bool Clean = false;
    public bool isCleaning = false;
    public CleaningController cleaningController;

    public int Order;
    private int initialOrder;
    private List<Collider2D> childColliders = new();
    public List<Collider2D> windowsInContact = new();
    [SerializeField] private LayerMask targetLayerMask;

    public bool canDrag = true;
    public bool dragging = false;
    private Vector3 offset;

    public bool isAlive = true;

    private RawImage rawImage;

    private void Awake()
    {
        SetLayerToChildren();
        PopulateColliders();
        cleaningController = GetComponent<CleaningController>();
        rawImage = GetComponentInChildren<RawImage>(includeInactive: true);
        initialOrder = Order;
    }

    private void Update()
    {
        if (!canDrag)
        {
            if (dragging) print($"{name}: Parou de arrastar porque canDrag == false");
            if (dragging) dragging = false;
            LevelController.Instance.isDraggingWindows = false;
            return;
        }

        if (dragging)
        {
            print($"{name}: Arrastando...");
            Vector3 mouseWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;
            transform.position = mouseWorldPos + offset;
        }
    }

    private void OnMouseDown()
    {
        print($"{name}: OnMouseDown chamado");

        if (!canDrag)
        {
            print($"{name}: canDrag == false, não vai arrastar");
            return;
        }

        Vector3 clickWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;
        offset = transform.position - clickWorldPos;

        WindowsLayerController.Instance.SetWindowAsTop(this);
        dragging = true;

        print($"{name}: Começou a arrastar");
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

        foreach (var col in childColliders)
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

        foreach (var col in childColliders)
            col.enabled = true;
    }

    private void PopulateColliders()
    {
        childColliders.Clear();
        Collider2D[] allColliders = GetComponentsInChildren<Collider2D>(includeInactive: true);

        foreach (Collider2D col in allColliders)
        {
            if (col.gameObject != this.gameObject)
                childColliders.Add(col);
        }
    }


    public void SetOrderInLayer(int order)
    {
        Order = order;

        SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
        if (sprite != null)
            sprite.sortingOrder = order;

        Canvas canvas = GetComponentInChildren<Canvas>(includeInactive: true);
        if (canvas != null)
            canvas.sortingOrder = order;
    }

    private void SetLayerToChildren()
    {
        int layerIndex = Mathf.RoundToInt(Mathf.Log(targetLayerMask.value, 2));

        foreach (Transform child in GetComponentsInChildren<Transform>(includeInactive: true))
        {
            if (child == this.transform) continue; 
            if (child.GetComponent<Canvas>() != null) continue;
            if (child.GetComponent<RawImage>() != null) continue;

            child.gameObject.layer = layerIndex;
        }
    }
}
