using TarodevController;
using UnityEngine;

public class Aviaozinho : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float maxTurboMultiplier;
    [SerializeField] private float turboAcceleration;

    [Header("Rotação")]
    [SerializeField] private float baseTurnSpeed;
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float rotationDecay;
    [SerializeField] private float rotationTurboMultiplier;

    [Header("Sprites")]
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite turningLeftSprite;
    [SerializeField] private Sprite turningRightSprite;
    [SerializeField] private float spriteMomentumThreshold = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Helice")]
    [SerializeField] private Animator heliceAnimator;
    [SerializeField] private float heliceNormalSpeed;
    [SerializeField] private float heliceTurboSpeed;
    [SerializeField] private GameObject heliceCollider;

    private float verticalHoldTime = 0f;
    private float currentTurnSpeed = 0f;
    private float rotationMomentum = 0f;
    private float currentTurbo = 1f;
    private float tempoForaTela = 0f;

    private bool inputHabilitado = false;
    private float tempoBloqueio = 0f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    private bool turboPressed;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        inputHabilitado = false;
        tempoBloqueio = 2f;
    }

    private void Update()
    {
        VerificarInputHabilitado();

        UpdateTurbo();
        HandleRotation();

        MoveForward();
        UpdateSprite();
        VerificarLimitesTela(); 
    }

    private void UpdateTurbo()
    {
        if (inputHabilitado)
        {
            turboPressed = Input.GetKey(KeyCode.Space);
        }
        else
        {
            turboPressed = false;
        }


        float targetTurbo = turboPressed ? maxTurboMultiplier : 1f;
        currentTurbo = Mathf.MoveTowards(currentTurbo, targetTurbo, turboAcceleration * Time.deltaTime);

        if (heliceAnimator != null)
        {
            heliceAnimator.speed = turboPressed ? heliceTurboSpeed : heliceNormalSpeed;
        }

        if (heliceCollider != null)
        {
            heliceCollider.SetActive(turboPressed);
        }
    }

    private void HandleRotation()
    {
        if (!inputHabilitado) return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Combina os dois, mas inverte o sinal do horizontal aqui
        float input = Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput) ? verticalInput : -horizontalInput;

        float turboTurnMultiplier = Mathf.Lerp(1f, rotationTurboMultiplier, (currentTurbo - 1f) / (maxTurboMultiplier - 1f));

        if (input != 0)
        {
            verticalHoldTime += Time.deltaTime;
            currentTurnSpeed = (baseTurnSpeed + verticalHoldTime * turnAcceleration) * turboTurnMultiplier;
            currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, baseTurnSpeed, maxTurnSpeed * turboTurnMultiplier);
            rotationMomentum = input * currentTurnSpeed;
        }
        else
        {
            verticalHoldTime = 0f;
            currentTurnSpeed = baseTurnSpeed * turboTurnMultiplier;
            rotationMomentum = Mathf.MoveTowards(rotationMomentum, 0f, rotationDecay * Time.deltaTime);
        }

        transform.Rotate(0f, 0f, rotationMomentum * Time.deltaTime);
    }



    private void MoveForward()
    {
        float speed = forwardSpeed * currentTurbo;
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        if (rotationMomentum > spriteMomentumThreshold)
        {
            spriteRenderer.sprite = turningLeftSprite;
        }
        else if (rotationMomentum < -spriteMomentumThreshold)
        {
            spriteRenderer.sprite = turningRightSprite;
        }
        else
        {
            spriteRenderer.sprite = neutralSprite;
        }
    }

    private void VerificarLimitesTela()
    {
        float xMin = -38f;
        float xMax = 38f;
        float yMin = -22f;
        float yMax = 22f;

        Vector2 pos = transform.position;
        bool foraDaTela = pos.x < xMin || pos.x > xMax || pos.y < yMin || pos.y > yMax;

        if (foraDaTela)
        {
            tempoForaTela += Time.deltaTime;

            if (tempoForaTela >= 5f)
            {
                tempoForaTela = 0f;
                AviaozinhoMorreu();
            }
        }
        else
        {
            tempoForaTela = 0f;
        }
    }

    private void VerificarInputHabilitado()
    {
        if (!inputHabilitado)
        {
            tempoBloqueio -= Time.deltaTime;
            if (tempoBloqueio <= 0f)
            {
                inputHabilitado = true;
                Debug.Log("Input habilitado novamente.");
            }
        }
    }

    public void AviaozinhoMorreu()
    {
        LevelController.Instance.PlayerDie();
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        inputHabilitado = false;
        tempoBloqueio = 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AviaozinhoMorreu();
    }
}
