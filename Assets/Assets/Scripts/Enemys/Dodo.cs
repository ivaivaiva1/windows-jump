using DG.Tweening;
using TarodevController;
using UnityEngine;

public class Dodo : MonoBehaviour
{
    private Collectable collectable;
    private bool holdingPlayer;
    private Player heldPlayer;

    [SerializeField] private float spitForce;
    [SerializeField] private float horizontalMultiplier;

    [SerializeField] private Sprite normalSprite; 
    [SerializeField] private Sprite fatSprite;
    private SpriteRenderer spriteRenderer;

    private Vector3 originalScale;


    private void Awake()
    {
        collectable = GetComponent<Collectable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            heldPlayer = collision.GetComponent<Player>();
            heldPlayer.gameObject.SetActive(false);
            holdingPlayer = true;
            EngolindoPlayer();
            spriteRenderer.sprite = fatSprite;
        }
    }

  

    private void Update()
    {
        if (!holdingPlayer) return;

        if (Input.GetButtonDown("Jump"))
        {
            collectable.setCollected();
            heldPlayer.gameObject.SetActive(true);
            heldPlayer.transform.position = transform.position;

            // Captura direção do input (WASD)
            Vector2 inputDir = Vector2.zero;

            if (Input.GetKey(KeyCode.W)) inputDir.y += 1;
            if (Input.GetKey(KeyCode.S)) inputDir.y -= 1;
            if (Input.GetKey(KeyCode.D)) inputDir.x += 1;
            if (Input.GetKey(KeyCode.A)) inputDir.x -= 1;

            // Se não estiver apertando nenhuma direção, cuspir pra direita
            if (inputDir == Vector2.zero)
            {
                inputDir = Vector2.right;
            }

            inputDir.Normalize();

            heldPlayer.playerController.DodoSpit(inputDir.y * spitForce, (inputDir.x * (spitForce * horizontalMultiplier)));

            holdingPlayer = false;
            heldPlayer = null;

            spriteRenderer.sprite = normalSprite;
        }
    }

    private void EngolindoPlayer()
    {
        transform.DOKill(); // Cancela tweens anteriores

        Sequence swallowSeq = DOTween.Sequence();

        float duration = 0.3f;
        int distortions = 2;
        float baseDistortionDuration = duration / (distortions + 1);
        float distortionDuration = baseDistortionDuration * 1.5f; // 1.5x maior

        for (int i = 0; i < distortions; i++)
        {
            float randomX = Random.Range(0.5f, 1.5f);
            float randomY = Random.Range(0.7f, 1.3f);
            Vector3 distortedScale = new Vector3(originalScale.x * randomX, originalScale.y * randomY, originalScale.z);

            swallowSeq.Append(transform.DOScale(distortedScale, distortionDuration).SetEase(Ease.OutQuad));
        }

        swallowSeq.Append(transform.DOScale(originalScale, distortionDuration).SetEase(Ease.OutQuad));
    }





}
