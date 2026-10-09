using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputHandle : MonoBehaviour
{
    private CameraInputAction _cameraInputAction;
    public Vector2 InputVector { get; private set; }
    public Vector2 CancelInputVector { get; private set; }
    public event EventHandler OnCancelEvent;
    public event EventHandler OnLeftClickEvent;
    public event EventHandler OnRotateEvent;

    private void Awake()
    {
        InputVector = Vector2.zero;
    }
    private void OnEnable()
    {
        _cameraInputAction = new CameraInputAction();
        _cameraInputAction.Camera.Enable();
        _cameraInputAction.Build.Enable();
        _cameraInputAction.Camera.Move.performed += OnPressed;
        _cameraInputAction.Camera.Move.canceled += OnCancelled;
        _cameraInputAction.Build.LeftClick.performed += OnBuild;
        _cameraInputAction.Build.Rotate.performed += OnRotate;
    }
    private void OnDisable()
    {
        _cameraInputAction.Camera.Move.performed -= OnPressed;
        _cameraInputAction.Camera.Move.canceled -= OnCancelled;
        _cameraInputAction.Build.LeftClick.performed -= OnBuild;
        _cameraInputAction.Camera.Disable();
        _cameraInputAction.Build.Disable();
    }
    private void OnPressed(InputAction.CallbackContext context)
    {
        InputVector = context.ReadValue<Vector2>();
    }
    private void OnCancelled(InputAction.CallbackContext context)
    {
        CancelInputVector = InputVector;
        OnCancelEvent?.Invoke(this, EventArgs.Empty);
    }
    private void OnBuild(InputAction.CallbackContext context)
    {
        OnLeftClickEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        OnRotateEvent?.Invoke(this,EventArgs.Empty);
    }
}
