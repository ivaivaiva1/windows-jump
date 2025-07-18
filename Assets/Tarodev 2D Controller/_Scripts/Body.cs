using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Body : MonoBehaviour
{

    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.PegandoMoeda);
            Collectable collectable = other.GetComponent<Collectable>();
            if (collectable != null)
            {
                collectable.setCollected();
            }
        }

        if (other.CompareTag("Enemy"))
        {
            print("encostei na cobra");
            Collectable enemy = other.GetComponent<Collectable>();
            if (enemy != null)
            {
                if (!enemy.isCollected)
                {
                    StartCoroutine(DelayedHitCheck(enemy));
                    //player.Die();
                }
                else
                {
                    print("ufa, ela tava coletada ja rs");
                }
            }
        }
    }

    private IEnumerator DelayedHitCheck(Collectable enemy)
    {
        yield return null; 

        if (!enemy.isCollected)
        {
            print("cobra n esta coletada, morri :c");
            player.Die();
        }
        else
        {
            print("ufa, ela tava coletada ja rs");
        }
    }
}
