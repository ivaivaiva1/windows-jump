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
            Controller.SpawnPlayer();
            transform.position = spawnPoint.position;
        }
    }
}
