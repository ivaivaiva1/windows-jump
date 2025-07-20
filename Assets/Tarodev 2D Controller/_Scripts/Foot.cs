using TarodevController;
using UnityEngine;

public class Foot : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float BounceForce;

    [SerializeField] private float horizontalBaseForce;
    [SerializeField] private float horizontalForceExponent;
    [SerializeField] private float horizontalBounceMultiplier;
    [SerializeField] private float minHorizontalBounce;
    [SerializeField] private float maxHorizontalBounce;

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.CurrentWindow != null)
        {
            if (player.CurrentWindow.dragging) return;
        }

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

            Trampoline trampoline = other.GetComponent<Trampoline>();
            if(trampoline == null) return;

            float horizontalForce = GetHorizontalBounceForce(other.transform.position.x);
            playerController.HorizontalBounce(horizontalForce);
            playerController.Bounce(trampoline.bounceForce, trampoline.holdMultiplier);
        }

        if (other.CompareTag("Trampoline"))
        {
            Trampoline trampoline = other.GetComponent<Trampoline>();
            playerController.Bounce(trampoline.bounceForce, trampoline.holdMultiplier);
        }
    }

    private float GetHorizontalBounceForce(float enemyX)
    {
        float deltaX = playerController.transform.position.x - enemyX;
        float distance = Mathf.Abs(deltaX);
        float scaledDistance = Mathf.Pow(distance, horizontalForceExponent);

        float force = scaledDistance * horizontalBaseForce;
        force = Mathf.Clamp(force, minHorizontalBounce, maxHorizontalBounce);

        return Mathf.Sign(deltaX) * force;
    }
}
