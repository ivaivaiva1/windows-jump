using TarodevController;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerController Controller { get; private set; }
    public PlayerWindow Window { get; private set; }

    public bool IsGrounded => Controller != null && Controller._grounded;
    public bool IsMoving => Controller != null && Controller.FrameInput.x != 0;

    public Window CurrentWindow;

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;

        Controller = GetComponent<PlayerController>();
        Window = GetComponent<PlayerWindow>();
    }

    private void Start()
    {
        transform.position = spawnPoint.position;
    }

    private void Update()
    {
        CheckIfPlayerFall();
    }

    public void SetCurrentWindow(GameObject window)
    {
        if (CurrentWindow != null) CurrentWindow.canDrag = true;
        CurrentWindow = window.GetComponent<Window>();
    }

    public void NullCurrentWindow()
    {
        if (CurrentWindow != null) CurrentWindow.canDrag = true;
        CurrentWindow = null;
    }

    private void CheckIfPlayerFall()
    {
        if (transform.position.y < -22f && CurrentWindow == null)
        {
            Die();
        }   
    }

    private void Die()
    {
        transform.position = spawnPoint.position;
        Controller.SpawnPlayer();
        LevelController.Instance.PlayerDie();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            SoundController.Instance.PlaySfxOneShot("coin");
            Collectable collectable = other.GetComponent<Collectable>();
            if (collectable != null)
            {
                collectable.setCollected();
            }
        }
    }

}
