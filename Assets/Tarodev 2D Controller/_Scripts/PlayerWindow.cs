using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerWindow : MonoBehaviour
{
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
            if (window != null && window.isAlive)
            {
                SetWindowParent(window);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Window"))
        {
            if (transform.parent != null)
            {
                if (transform.parent == collision.transform)
                {
                    transform.SetParent(null);
                    Player.Instance.NullCurrentWindow();
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
        transform.SetParent(newWindow.transform);
        Player.Instance.SetCurrentWindow(newWindow.gameObject);
        WindowsLayerController.Instance.SetPlayerOrder(newWindow);
    }
}
