using UnityEngine;

public class Fenix : MonoBehaviour
{
    private Collectable collectable;

    [SerializeField] private float timeToRebornLikeAFenix;

    private float count = 0f;
    private bool startCounting = false;

    private void Start()
    {
        collectable = GetComponent<Collectable>();
    }

    private void Update()
    {
        CheckIfFenixAlreadyReborn();
        Countdown();
    }

    private void TimeToRebornFenix()
    {
        collectable.ResetCollectable();
        collectable.thisWindow.IamCollected();
    }

    private void ResetCountdown()
    {
        Debug.Log("Contador resetado: coletável não está mais coletado.");
        startCounting = false;
        count = 0f;
    }

    private void CheckIfFenixAlreadyReborn()
    {
        if (startCounting && collectable != null && !collectable.isCollected)
        {
            ResetCountdown();
        }
    }

    private void Countdown()
    {
        if (!startCounting)
        {
            if (collectable != null && collectable.isCollected)
            {
                startCounting = true;
                count = 0f;
            }
            return;
        }

        count += Time.deltaTime;

        if (count >= timeToRebornLikeAFenix)
        {
            startCounting = false;
            TimeToRebornFenix();
        }
    }
}
