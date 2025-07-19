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
    private List<SpriteRenderer> childSpriteRenderers = new();
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
        PopulateSpriteRenderers();
        cleaningController = GetComponent<CleaningController>();
        rawImage = GetComponentInChildren<RawImage>(includeInactive: true);
        initialOrder = Order;
    }

    private void Update()
    {
        if (dragging)
        {
            if (!canDrag)
            {
                SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.MovimentoNegado);
                dragging = false;
                LevelController.Instance.isDraggingWindows = false;
                return;
            }
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
            SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.MovimentoNegado);
            print($"{name}: canDrag == false, não vai arrastar");
            return;
        }

        Vector3 clickWorldPos = MainCameraMouseTracker.Instance.MouseWorldPosition;
        offset = transform.position - clickWorldPos;

        SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.AgarrandoJanela);
        WindowsLayerController.Instance.SetWindowAsTop(this);
        dragging = true;
        

        print($"{name}: Começou a arrastar");
        LevelController.Instance.isDraggingWindows = true;
    }

    private void OnMouseUp()
    {
        if (dragging)
        {
            SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.SoltandoJanela);
            dragging = false;
            LevelController.Instance.isDraggingWindows = false;
        }
    }

    public void KillWindow()
    {
        float transparencyValue = 0.5f;
        isAlive = false;

        foreach (var sr in childSpriteRenderers)
        {
            Color color = sr.color;
            color.a = transparencyValue;
            sr.color = color;
        }

        foreach (var col in childColliders)
            col.enabled = false;

        if(rawImage != null)
        {
            Color color = rawImage.color;
            color.a = transparencyValue;
            rawImage.color = color;
        }
    }

    public void ReviveWindow()
    {
        isAlive = true;

        foreach (var sr in childSpriteRenderers)
        {
            Color color = sr.color;
            color.a = 1f;
            sr.color = color;
        }

        foreach (var col in childColliders)
            col.enabled = true;

        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = 1f;
            rawImage.color = color;
        }
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

    private void PopulateSpriteRenderers()
    {
        childSpriteRenderers.Clear();
        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);

        foreach (SpriteRenderer sr in allSprites)
        {
            if (sr.gameObject != this.gameObject)
                childSpriteRenderers.Add(sr);
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


    [SerializeField] private SpriteRenderer HeaderRenderer;
    [SerializeField] private Sprite trueHeaderSprite;
    [SerializeField] private Sprite falseHeaderSprite;
    public void UpdateHeaderSprite(bool state)
    {
        HeaderRenderer.sprite = state ? trueHeaderSprite : falseHeaderSprite;
    }

}
