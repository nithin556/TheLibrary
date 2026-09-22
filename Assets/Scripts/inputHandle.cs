using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputHandle : MonoBehaviour
{
    private CameraInputAction cameraInputAction;
    public Vector2 inputVector { get; private set; }
    public Vector2 cancelInputVector { get; private set; }
    public event EventHandler OnCancelEvent;
    public event EventHandler OnBuildEvent;

    void Awake()
    {
        inputVector = Vector2.zero;
    }
    private void OnEnable()
    {
        cameraInputAction = new CameraInputAction();
        cameraInputAction.Camera.Enable();
        cameraInputAction.Build.Enable();
        cameraInputAction.Camera.Move.performed += OnPressed;
        cameraInputAction.Camera.Move.canceled += Oncancelled;
        cameraInputAction.Build.LeftClick.performed += OnBuild;
    }
    void OnDisable()
    {
        cameraInputAction.Camera.Move.performed -= OnPressed;
        cameraInputAction.Camera.Move.canceled -= Oncancelled;
        cameraInputAction.Build.LeftClick.performed -= OnBuild;
        cameraInputAction.Camera.Disable();
        cameraInputAction.Build.Disable();
    }
    void OnPressed(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }
    void Oncancelled(InputAction.CallbackContext context)
    {
        cancelInputVector = inputVector;
        OnCancelEvent?.Invoke(this, EventArgs.Empty);
    }
    void OnBuild(InputAction.CallbackContext context)
    {
        OnBuildEvent?.Invoke(this, EventArgs.Empty);
    }
}
