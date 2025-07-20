using System.Collections.Generic;
using TarodevController;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    public bool isDraggingWindows = false;

    private List<Window> allWindows = new List<Window>();

    private float holdRTime = 0f;
    private float holdPTime = 0f;
    private const float holdThreshold = 2f;

    private void Awake()
    {
        Instance = this;

        Window[] findWindows = FindObjectsOfType<Window>(true);
        allWindows.AddRange(findWindows);
    }

    private void Update()
    {
        HandleDragState();
        HandleSafetyKeys();
    }

    private void HandleDragState()
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

    private void HandleSafetyKeys()
    {
        // Verifica se R está sendo segurado
        if (Input.GetKey(KeyCode.R))
        {
            holdRTime += Time.deltaTime;
            if (holdRTime >= holdThreshold)
            {
                ReloadScene();
                holdRTime = 0f;
            }
        }
        else
        {
            holdRTime = 0f;
        }

        // Verifica se P está sendo segurado
        if (Input.GetKey(KeyCode.P))
        {
            holdPTime += Time.deltaTime;
            if (holdPTime >= holdThreshold)
            {
                NextLevel();
                holdPTime = 0f;
            }
        }
        else
        {
            holdPTime = 0f;
        }
    }

    public void PlayerDie()
    {
        foreach (Window window in allWindows)
        {
            window.UpdateHeaderSprite(false);
            CleaningController cleaner = window.GetComponent<CleaningController>();
            if (cleaner != null)
            {
                cleaner.ResetWindow();
            }
        }
    }

    public void WindowIsClean(Window window)
    {
        window.Clean = true;

        foreach (Window w in allWindows)
        {
            if (!w.Clean)
                return;

        }

        Debug.Log("Todas as janelas estão limpas! Próxima fase!");
        NextLevel();
    }

    private void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
