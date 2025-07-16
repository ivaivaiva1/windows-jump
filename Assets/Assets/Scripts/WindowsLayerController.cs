using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowsLayerController : MonoBehaviour
{
    public static WindowsLayerController Instance { get; private set; }

    [SerializeField] private int startingOrder = 100;
    [SerializeField] private int step = 10;
    [SerializeField] private List<Window> allWindows = new();

    private Canvas playerCanvas;

    private void Awake()
    {
        Instance = this;

        allWindows = new List<Window>(FindObjectsOfType<Window>(includeInactive: true));
        playerCanvas = GameObject.Find("PlayerCanvas")?.GetComponent<Canvas>();

        int currentOrder = startingOrder;

        foreach (var window in allWindows)
        {
            window.Order = currentOrder;
            window.SetOrderInLayer(currentOrder);
            currentOrder -= step;
        }
    }


    public void SetWindowAsTop(Window targetWindow)
    {
        foreach (var window in allWindows)
        {
            if (window == targetWindow)
            {
                window.Order = startingOrder;
            }
            else
            {
                window.Order -= step;
            }

            window.SetOrderInLayer(window.Order);

            if (Player.Instance.CurrentWindow == window && playerCanvas != null)
            {
                int newOrder = window.Order + 1;
                playerCanvas.sortingOrder = newOrder;
            }
        }
    }

    public void SetPlayerOrder(Window window)
    {
        if (playerCanvas != null)
        {
            int newOrder = window.Order + 1;
            playerCanvas.sortingOrder = newOrder;
        }
    }
}
