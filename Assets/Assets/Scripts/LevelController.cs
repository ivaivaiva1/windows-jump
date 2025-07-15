using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    public List<Window> windows = new List<Window>();

    public Window currentWindow;

    public PlayerController Player;

    public bool player_isGrounded;

    public bool player_isMoving;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCurrentWindow(Window window)
    {
        currentWindow = window;
    }

    private void Update()
    {
        if(Player.FrameInput.x == 0)
        {
            player_isMoving = false;
        }
        else
        {
            player_isMoving = true;
        }

        player_isGrounded = Player._grounded;


        if (currentWindow == null) return;
        if (!player_isGrounded || player_isMoving) 
        { 
            currentWindow.canDrag = false;
        } else
        {
            currentWindow.canDrag = true;
        }
    }

}