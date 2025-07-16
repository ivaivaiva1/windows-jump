using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    public bool isDraggingWindows = false;

    private List<Window> allWindows = new List<Window>();

    private void Awake()
    {
        Instance = this;

        Window[] findWindows = FindObjectsOfType<Window>(true); 
        allWindows.AddRange(findWindows);
    }

    private void Update()
    {
        if (Player.Instance.CurrentWindow == null) return;

        if (!Player.Instance.IsGrounded || Player.Instance.IsMoving)
        {
            Player.Instance.CurrentWindow.canDrag = false;
        }
        else
        {
            Player.Instance.CurrentWindow.canDrag = true;
        }
    }

    public void PlayerDie()
    {
        foreach (Window window in allWindows)
        {
            CleaningController cleaner = window.GetComponent<CleaningController>();
            if (cleaner != null)
            {
                cleaner.ResetWindow();
            }
        }
    }
}
