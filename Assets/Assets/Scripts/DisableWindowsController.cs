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

        if (!LevelController.Instance.isDraggingWindows)
            return;

        CheckForDeactivation();
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

            bool isTouchingActiveWindow = false;

            foreach (var otherWindow in allWindows)
            {
                if (otherWindow == window)
                    continue;

                // Ignorar janelas desativadas
                if (!otherWindow.isAlive)
                    continue;

                Bounds boundsB = GetBounds(otherWindow);

                if (boundsA.Intersects(boundsB))
                {
                    isTouchingActiveWindow = true;
                    break;
                }
            }

            // Se não estiver tocando nenhuma janela ativa, pode reviver
            if (!isTouchingActiveWindow)
            {
                window.ReviveWindow();
                disabledWindows.RemoveAt(i);
            }
        }
    }

    private float minX = -36f;
    private float maxX = 36f;
    private float minY = -20f;
    private float maxY = 20f;

    private void LimitWindowsToBounds()
    {
        foreach (var window in allWindows)
        {
            if (!window.isAlive || !window.dragging) continue;

            Vector3 pos = window.transform.position;
            bool outOfBounds = false;

            if (pos.x < minX)
            {
                pos.x = minX;
                outOfBounds = true;
            }
            else if (pos.x > maxX)
            {
                pos.x = maxX;
                outOfBounds = true;
            }

            if (pos.y < minY)
            {
                pos.y = minY;
                outOfBounds = true;
            }
            else if (pos.y > maxY)
            {
                pos.y = maxY;
                outOfBounds = true;
            }

            if (outOfBounds)
            {
                window.transform.position = pos;
            }
        }
    }

    private void LimitWindowsToScreen()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 screenMin = cam.ViewportToWorldPoint(Vector2.zero);
        Vector2 screenMax = cam.ViewportToWorldPoint(Vector2.one);

        foreach (var window in allWindows)
        {
            if (!window.isAlive) continue;

            Bounds bounds = GetBounds(window);
            Vector3 pos = window.transform.position;
            Vector3 size = bounds.extents;

            float clampedX = Mathf.Clamp(pos.x, screenMin.x + size.x, screenMax.x - size.x);
            float clampedY = Mathf.Clamp(pos.y, screenMin.y + size.y, screenMax.y - size.y);

            window.transform.position = new Vector3(clampedX, clampedY, pos.z);
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
