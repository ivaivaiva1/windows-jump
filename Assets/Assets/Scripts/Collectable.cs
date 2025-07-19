using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public CleaningController thisWindow;
    public bool isCollected = false;
   

    public void setCollected()
    {
        print("sou a cobra e fui coletada...");
        isCollected = true;
        thisWindow.IamCollected();
        gameObject.SetActive(false);  
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
