using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    public Window playerWindow;

    public GameObject Player;

    public PlayerController playerController;

    public bool player_isGrounded;

    public bool player_isMoving;

    public bool isDraggingWindows = false;

    private void Awake()
    {
        Instance = this;
    }       

    public void SetCurrentWindow(GameObject window)
    {
        if (playerWindow != null) playerWindow.canDrag = true;
        playerWindow = window.GetComponent<Window>();
    }

    public void NullCurrentWindow()
    {
        if (playerWindow != null) playerWindow.canDrag = true;
        playerWindow = null;
    }

    private void Update()
    {
        if(playerController.FrameInput.x == 0)
        {
            player_isMoving = false;
        }
        else
        {
            player_isMoving = true;
        }

        player_isGrounded = playerController._grounded;


        if (playerWindow == null) return;
        if (!player_isGrounded || player_isMoving) 
        {
            playerWindow.canDrag = false;
        } else
        {
            playerWindow.canDrag = true;
        }
    }

}