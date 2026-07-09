using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputHandle : MonoBehaviour
{
    private CameraInputAction cameraInputAction;
    public Vector2 inputVector { get; private set; }
    public Vector2 cancelInputVector { get; private set; }
    public event EventHandler OnCancelEvent;

    void Awake()
    {
        inputVector = Vector2.zero;
    }
    private void OnEnable()
    {
        cameraInputAction = new CameraInputAction();
        cameraInputAction.Camera.Enable();
        cameraInputAction.Camera.Move.performed += OnPressed;
        cameraInputAction.Camera.Move.canceled += Oncancelled;
    }
    void OnDisable()
    {
        cameraInputAction.Camera.Move.performed -= OnPressed;
        cameraInputAction.Camera.Move.canceled -= Oncancelled;
        cameraInputAction.Camera.Disable();
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
}
