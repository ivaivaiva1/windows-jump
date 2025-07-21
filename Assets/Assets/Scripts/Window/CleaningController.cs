using System.Collections.Generic;
using UnityEngine;

public class CleaningController : MonoBehaviour
{
    private Window window;
    private List<Collectable> collectables = new List<Collectable>();

    public int cleaningItems = 0;
    public int leftItems = 0;

    void Awake()
    {
        window = GetComponent<Window>();

        Collectable[] foundCollectables = GetComponentsInChildren<Collectable>(true);

        foreach (Collectable c in foundCollectables)
        {
            collectables.Add(c);
            c.SetWindow(this);
        }

        cleaningItems = 0;
        leftItems = collectables.Count;
    }

    public void ResetWindow()
    {
        window.Clean = false;
        window.isCleaning = false;
        print("reset window");

        foreach (Collectable c in collectables)
        {
            c.ResetCollectable();
        }

        cleaningItems = 0;
        leftItems = collectables.Count;
    }

    public void IamCollected()
    {
        cleaningItems = 0;
        leftItems = 0;

        foreach (Collectable c in collectables)
        {
            if (c.isCollected)
                cleaningItems++;
            else
                leftItems++;
        }

        if(leftItems > 0 && window.Clean)
        {
            window.Clean = false;
        }

        if (leftItems == 0)
        {
            //window.isCleaning = true;
            //window.Clean = true;
            LevelController.Instance.WindowIsClean(window);
        }
    }
}
