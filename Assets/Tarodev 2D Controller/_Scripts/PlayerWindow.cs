using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerWindow : MonoBehaviour
{
    public Player player;
    public bool isVirgin = true;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Window"))
        {
            Window window = collision.GetComponent<Window>();

            if (player.CurrentWindow != null && window.dragging) return;

            if (window != null && window.isAlive)
            {
                if (isVirgin)
                {
                    isVirgin = false;
                    window.UpdateHeaderSprite(true);
                }

                SetWindowParent(window);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Window"))
        {
            Window actualWindow = player.transform.parent.GetComponent<Window>();
            if (actualWindow != null)
            {
                Window collisionWindow = collision.GetComponent<Window>();
                if (player.CurrentWindow == collisionWindow)
                {
                    player.transform.SetParent(null);
                    player.NullCurrentWindow();
                }
            }
            else
            {
                Collider2D[] overlappingWindows = GetOverlappingWindows();

                foreach (var col in overlappingWindows)
                {
                    Window window = col.GetComponent<Window>();
                    if (window != null && window.isAlive)
                    {
                        SetWindowParent(window);
                        break;
                    }
                }
            }
        }
    }

    private Collider2D[] GetOverlappingWindows()
    {
        Collider2D[] results = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.NoFilter();

        int count = col.OverlapCollider(filter, results);

        List<Collider2D> windows = new List<Collider2D>();

        for (int i = 0; i < count; i++)
        {
            if (results[i] != null && results[i].CompareTag("Window"))
            {
                windows.Add(results[i]);
            }
        }

        return windows.ToArray();
    }

    private void SetWindowParent(Window newWindow)
    {
        player.transform.SetParent(newWindow.transform);
        player.SetCurrentWindow(newWindow.gameObject);
        WindowsLayerController.Instance.SetPlayerOrder(newWindow);
    }
}
