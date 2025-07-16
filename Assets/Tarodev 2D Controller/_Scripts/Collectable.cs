using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private CleaningController thisWindow;
    public bool isCollected = false;
   

    public void setCollected()
    {
        gameObject.SetActive(false);
        isCollected = true;
    }

    public void ResetCollectable()
    {
        gameObject.SetActive(true);
        isCollected = false;
    }

    public void SetWindow(CleaningController newWindow)
    {
        thisWindow = newWindow;
    }

}
