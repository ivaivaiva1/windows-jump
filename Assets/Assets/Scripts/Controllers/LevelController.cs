using System.Collections.Generic;
using TarodevController;
using UnityEngine.SceneManagement;
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

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0; // Desativa VSync para que o targetFrameRate funcione
    }

    private void Update()
    {
        if (Player.Instance.CurrentWindow == null) return;

        if (!Player.Instance.IsGrounded || Player.Instance.IsMoving)
        {
            print("gounded: " + Player.Instance.IsGrounded);
            print("isMoving: " + Player.Instance.IsMoving);
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
}
