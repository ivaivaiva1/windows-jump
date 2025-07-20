using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType { None, Enemy, Coin }
    private CollectableType collectableType = CollectableType.None;
    private Enemy enemy;
    private Coin coin;
    

    public CleaningController thisWindow;
    public bool isCollected = false;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        coin = GetComponent<Coin>();

        if (enemy != null)
        {
            collectableType = CollectableType.Enemy;
        }
        else if (coin != null)
        {
            collectableType = CollectableType.Coin;
        }
    }

    public void setCollected()
    {
        //print("sou a cobra e fui coletada...");
        //isCollected = true;
        //thisWindow.IamCollected();
        //gameObject.SetActive(false);

        if (collectableType == CollectableType.Enemy)
        {
            enemy.EnemyCollect();
        }
        else if (collectableType == CollectableType.Coin)
        {
            coin.CoinCollect();
        }
    }

    public void ResetCollectable()
    {
        //gameObject.SetActive(true);
        //isCollected = false;

        if (collectableType == CollectableType.Enemy)
        {
            enemy.EnemyReset();
        }
        else if (collectableType == CollectableType.Coin)
        {
            coin.CoinReset();
        }
    }

    public void SetWindow(CleaningController newWindow)
    {
        thisWindow = newWindow;
    }
}
