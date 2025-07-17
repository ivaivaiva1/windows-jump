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
        LimitWindowsToBounds();
        CheckForReactivation();

        //if (!LevelController.Instance.isDraggingWindows)
        //    return;

        CheckForDeactivation();
    }

    private void CheckForDeactivation()
    {
        foreach (var draggingWindow in allWindows)
        {
            if (!draggingWindow.dragging)
                continue;

            Bounds draggingBounds = GetBounds(draggingWindow, true); // com safe zone

            foreach (var otherWindow in allWindows)
            {
                if (otherWindow == draggingWindow)
                    continue;

                if (disabledWindows.Contains(otherWindow))
                    continue;

                Bounds otherBounds = GetBounds(otherWindow, false);

                if (draggingBounds.Intersects(otherBounds))
                {
                    // Se a janela sendo arrastada for a do player, desativa a outra
                    if (draggingWindow == Player.Instance.CurrentWindow)
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
            Bounds boundsA = GetBounds(window, true); // com safe zone

            bool isTouchingActiveWindow = false;

            foreach (var otherWindow in allWindows)
            {
                if (otherWindow == window)
                    continue;

                if (!otherWindow.isAlive)
                    continue;

                Bounds boundsB = GetBounds(otherWindow, false);

                if (boundsA.Intersects(boundsB))
                {
                    isTouchingActiveWindow = true;
                    break;
                }
            }

            if (!isTouchingActiveWindow)
            {
                window.ReviveWindow();
                disabledWindows.RemoveAt(i);
            }
        }
    }

    private float minX = -34f;
    private float maxX = 34f;
    private float minY = -18f;
    private float maxY = 18f;

    private void LimitWindowsToBounds()
    {
        foreach (var window in allWindows)
        {
            //if (!window.isAlive || !window.dragging) continue;

            Vector3 pos = window.transform.position;

            Bounds bounds = GetBounds(window, false);
            Vector3 size = bounds.size;

            bool outOfBounds = false;

            if (pos.x < minX - size.x / 2f)
            {
                pos.x = minX - size.x / 2f;
                outOfBounds = true;
            }
            else if (pos.x > maxX + size.x / 2f)
            {
                pos.x = maxX + size.x / 2f;
                outOfBounds = true;
            }

            if (pos.y < minY - size.y / 2f)
            {
                pos.y = minY - size.y / 2f;
                outOfBounds = true;
            }
            else if (pos.y > maxY + size.y / 2f)
            {
                pos.y = maxY + size.y / 2f;
                outOfBounds = true;
            }

            if (outOfBounds)
            {
                window.transform.position = pos;
            }
        }
    }

    private Bounds GetBounds(Window window, bool withSafeZone)
    {
        Collider2D col = window.GetComponentInChildren<Collider2D>();
        if (col == null)
            return new Bounds(window.transform.position, Vector3.zero);

        Bounds bounds = col.bounds;

        if (withSafeZone)
            bounds.Expand(safeZonePadding);

        return bounds;
    }
}
