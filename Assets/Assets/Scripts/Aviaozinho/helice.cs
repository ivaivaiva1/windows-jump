using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class helice : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            Collectable collectable = other.GetComponent<Collectable>();
            if (collectable != null)
            {
                collectable.setCollected();
                print("pisei na cobra e matei ela");
            }

            SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.QuicandoInimigo);
        }
    }
}
