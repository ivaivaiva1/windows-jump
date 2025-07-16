using System.Collections.Generic;
using UnityEngine;

public class DisableWindowsController : MonoBehaviour
{
    [SerializeField] private float safeZonePadding;

    private List<Window> allWindows = new();
    private List<Window> disabledWindows = new();

    private void Start()
    {
        allWindows.AddRange(FindObjectsOfType<Window>());
    }

    private void Update()
    {
        if (!LevelController.Instance.isDraggingWindows)
            return;

        CheckForDeactivation();
        CheckForReactivation();
    }

    private void CheckForDeactivation()
    {
        foreach (var draggingWindow in allWindows)
        {
            if (!draggingWindow.dragging)
                continue;

            Bounds draggingBounds = GetExpandedBounds(draggingWindow);

            foreach (var otherWindow in allWindows)
            {
                if (otherWindow == draggingWindow)
                    continue;

                if (disabledWindows.Contains(otherWindow))
                    continue;

                Bounds otherBounds = GetBounds(otherWindow);

                if (draggingBounds.Intersects(otherBounds))
                {
                    // Se a janela sendo arrastada for a do player, desativa a outra
                    if (draggingWindow == LevelController.Instance.playerWindow)
                    {
                        otherWindow.KillWindow();
                        disabledWindows.Add(otherWindow);
                    }
                    else
                    {
                        draggingWindow.KillWindow();
                        disabledWindows.Add(draggingWindow);
                        break; // Já desativamos essa, pode sair do loop interno
                    }
                }
            }
        }
    }


    private void CheckForReactivation()
    {
        for (int i = disabledWindows.Count - 1; i >= 0; i--)
        {
            Window window = disabledWindows[i];
            Bounds boundsA = GetExpandedBounds(window);

            bool isTouching = false;

            foreach (var otherWindow in allWindows)
            {
                if (otherWindow == window)
                    continue;

                Bounds boundsB = GetBounds(otherWindow);

                if (boundsA.Intersects(boundsB))
                {
                    isTouching = true;
                    break;
                }
            }

            if (!isTouching)
            {
                window.ReviveWindow();
                disabledWindows.RemoveAt(i);
            }
        }
    }

    private Bounds GetBounds(Window window)
    {
        Collider2D col = window.GetComponentInChildren<Collider2D>();
        if (col != null)
            return col.bounds;

        return new Bounds(window.transform.position, Vector3.zero);
    }

    private Bounds GetExpandedBounds(Window window)
    {
        Bounds bounds = GetBounds(window);
        bounds.Expand(safeZonePadding);
        return bounds;
    }
}
