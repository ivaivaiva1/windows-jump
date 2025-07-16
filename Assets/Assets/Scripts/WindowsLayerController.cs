using System.Collections.Generic;
using UnityEngine;

public class WindowsLayerController : MonoBehaviour
{
    public static WindowsLayerController Instance { get; private set; }

    [SerializeField] private int startingOrder = 100;
    [SerializeField] private int step = 10;
    [SerializeField] private List<Window> allWindows = new();

    private void Awake()
    {
        Instance = this;

        allWindows = new List<Window>(FindObjectsOfType<Window>(includeInactive: true));

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
        }
    }
}