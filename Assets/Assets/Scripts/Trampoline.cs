using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceForce = 25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (player.CurrentWindow.dragging)
                {
                    return;
                }
            }

            var playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Bounce(bounceForce);
            }
        }
    }
}

