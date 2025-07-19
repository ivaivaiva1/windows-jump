using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private Collectable collectable;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();
    }

    public void CoinCollect()
    {
        collectable.isCollected = true;
        collectable.thisWindow.IamCollected();
        collectable.gameObject.SetActive(false);
    }

    public void CoinReset()
    {
        collectable.gameObject.SetActive(true);
        collectable.isCollected = false;
    }

}
