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
            if(enemy == null || collectable == null)
            {
                if (enemy == null) enemy = other.GetComponentInParent<Enemy>();
                if (collectable == null) collectable = other.GetComponentInParent<Collectable>();
            }

            collectable.setCollected();

            SoundController.Instance.PlaySfxOneShot(SoundController.SfxType.QuicandoInimigo);
        }
    }
}
