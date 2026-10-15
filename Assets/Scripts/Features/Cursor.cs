using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject cursorParent;
    private RaycastHit _cursorRaycastHit;
    
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        cursorParent.transform.rotation = Quaternion.identity;
        cursorParent.transform.position = Vector3.zero;
    }

    private void Update()
    {
        CursorMethod();
    }
    
    private void CursorMethod()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out _cursorRaycastHit, 100f);
        cursorParent.transform.position =  _cursorRaycastHit.point;
    }

    public RaycastHit GetCursorRaycastHit()
    {
        CursorMethod();
        return _cursorRaycastHit;
    }
    
    public Transform GetCursorParentTransform()
    {
        return cursorParent.transform;
    }
    
}
