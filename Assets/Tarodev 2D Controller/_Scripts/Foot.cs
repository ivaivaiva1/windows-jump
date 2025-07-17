using TarodevController;
using UnityEngine;

public class Foot : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float BounceForce;

    [SerializeField] private float horizontalBaseForce;
    [SerializeField] private float horizontalForceExponent;
    [SerializeField] private float horizontalBounceMultiplier;
    [SerializeField] private float minHorizontalBounce;
    [SerializeField] private float maxHorizontalBounce;

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Collectable collectable = other.GetComponent<Collectable>();
            if (collectable != null)
            {
                collectable.setCollected();
            }

            float horizontalForce = GetHorizontalBounceForce(other.transform.position.x);
            playerController.HorizontalBounce(horizontalForce);
            playerController.Bounce(BounceForce);
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
