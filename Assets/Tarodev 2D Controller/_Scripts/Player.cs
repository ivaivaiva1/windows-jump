using TarodevController;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerController playerController { get; private set; }
    [SerializeField] private PlayerWindow playerWindow;

    public bool IsGrounded => playerController != null && playerController._grounded;
    public bool IsMoving => playerController != null && playerController.FrameInput.x != 0;

    public Window CurrentWindow;

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;

        playerController = GetComponent<PlayerController>();

        if (spawnPoint == null)
        {
            GameObject obj = GameObject.Find("PlayerSpawn");
            if (obj != null)
            {
                spawnPoint = obj.transform;
            }
            else
            {
                Debug.LogWarning("SpawnPoint n�o encontrado na cena!");
            }
        }
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

    public void Die()
    {
        playerWindow.isVirgin = true;
        LevelController.Instance.PlayerDie();
        playerController.SpawnPlayer();
        transform.position = spawnPoint.position;
    }
}
