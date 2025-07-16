using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    public bool isDraggingWindows = false;

    private void Awake()
    {
        Instance = this;
    }       

    private void Update()
    {
        if (Player.Instance.CurrentWindow == null) return;
        if (!Player.Instance.IsGrounded || Player.Instance.IsMoving) 
        {
            Player.Instance.CurrentWindow.canDrag = false;
        } else
        {
            Player.Instance.CurrentWindow.canDrag = true;
        }
    }

}