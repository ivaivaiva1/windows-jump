using UnityEngine;

public class Smoke : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void OnSmokeAnimationEnd()
    {
        if (enemy != null) 
        {
            if (enemy.startReseting)
            {
                enemy.SmokeIsFinish();
            }
        }
        gameObject.SetActive(false);
    }
}
