using UnityEngine;

public class MainCameraMouseTracker : MonoBehaviour
{
    public static MainCameraMouseTracker Instance { get; private set; }

    public Vector3 MouseWorldPosition;

    private Camera _camera;

    private void Awake()
    {
        Instance = this;

        _camera = GetComponent<Camera>();
        if (_camera == null) _camera = Camera.main;
    }

    private void Update()
    {
        MouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        MouseWorldPosition.z = 0f;
        print(MouseWorldPosition);
    }
}
