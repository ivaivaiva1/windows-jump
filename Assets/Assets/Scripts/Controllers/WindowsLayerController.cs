using System.Collections.Generic;
using UnityEngine;

public class WindowsLayerController : MonoBehaviour
{
    public static WindowsLayerController Instance { get; private set; }

    [SerializeField] private int maxSortingLayers = 5;
    [SerializeField] private string sortingLayerPrefix = "Window_";
    [SerializeField] private List<Window> allWindows = new();

    private void Awake()
    {
        Instance = this;

        allWindows = new List<Window>(FindObjectsOfType<Window>(includeInactive: true));

        for (int i = 0; i < allWindows.Count; i++)
        {
            int layerIndex = Mathf.Clamp(i + 1, 1, maxSortingLayers);
            SetWindowSortingLayer(allWindows[i], layerIndex);
            allWindows[i].Order = layerIndex;
        }
    }

    public void SetWindowAsTop(Window targetWindow)
    {
        foreach (var window in allWindows)
        {
            if (window == targetWindow)
            {
                SetWindowSortingLayer(window, maxSortingLayers);
                window.Order = maxSortingLayers;
                window.UpdateHeaderSprite(true);
            }
            else
            {
                int newLayerIndex = Mathf.Clamp(window.Order - 1, 1, maxSortingLayers - 1);
                SetWindowSortingLayer(window, newLayerIndex);
                window.Order = newLayerIndex;
                window.UpdateHeaderSprite(false);
            }
        }
    }

    private void SetWindowSortingLayer(Window window, int layerIndex)
    {
        string layerName = sortingLayerPrefix + layerIndex;

        var renderers = window.GetComponentsInChildren<Renderer>(includeInactive: true);
        foreach (var r in renderers)
        {
            r.sortingLayerName = layerName;
        }

        var canvases = window.GetComponentsInChildren<Canvas>(includeInactive: true);
        foreach (var c in canvases)
        {
            c.sortingLayerName = layerName;
        }
    }

    public void SetPlayerOrder(Window window)
    {
        if (Player.Instance == null)
            return;

        string playerSortingLayer = sortingLayerPrefix + window.Order;

        // Aplica ao renderer do jogador
        var renderers = Player.Instance.GetComponentsInChildren<Renderer>(includeInactive: true);
        foreach (var r in renderers)
        {
            r.sortingLayerName = playerSortingLayer;
        }

        // Se o jogador usar Canvas (ex: barra de vida)
        var canvases = Player.Instance.GetComponentsInChildren<Canvas>(includeInactive: true);
        foreach (var c in canvases)
        {
            c.sortingLayerName = playerSortingLayer;
        }
    }
}
