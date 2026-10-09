using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private GameObject buildPrefab;
    [SerializeField] private inputHandle inputHandle;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask propsLayer;
    [SerializeField] private UIFunctionality uIFunc;
    [SerializeField] private CursorVisuals cursorVisuals;
    private Ray _ray;
    public RaycastHit _CursorRaycastHit;
    private Camera _mainCamera;
    
    public Prop _currentSelectedProp { get; private set; }
    private Prop prop;
    
    private void Awake()
    {
        _currentSelectedProp = null;
    }

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        inputHandle.OnLeftClickEvent += OnLeftClickPerformed;
    }

    private void Update()
    {
        CursorMethod();
    }


    private void OnLeftClickPerformed(object sender, EventArgs e)
    {
        ToolModeMethod();
    }

    private void ToolModeMethod()
    {
        if (uIFunc.CursorOnUI() != null) return;
        switch (uIFunc.selectedMode)
        {
            case UIFunctionality.ActiveMode.Select:
            {
                SelectModeMethod();
                break;
            }
            case UIFunctionality.ActiveMode.Build:
            {
                BuildModeMethod();
                break;
            }
            case UIFunctionality.ActiveMode.Destroy:
            {
                DestroyModeMethod();
                break;
            }
        }
    }


    private void DestroyModeMethod()
    {
        uIFunc.DetailsOnDeSelect();
        if (_CursorRaycastHit.collider.gameObject.layer != LayerMask.NameToLayer("props")) return;
        Destroy(TryDestroyBuilding()); // Handles null automatically
    }

    private void BuildModeMethod()
    {
        if (_CursorRaycastHit.collider.gameObject.layer != LayerMask.NameToLayer("Buildable")) return;
        if (TryPlaceBuilding())
        {
            //already created at start so just set active
            Instantiate(buildPrefab, _CursorRaycastHit.point, cursorVisuals.cursorTargetRot);
        }
    }

    private void SelectModeMethod()
    {
        // if objecthit does not belong to prop_layer , return
        if (_CursorRaycastHit.collider.gameObject.layer != LayerMask.NameToLayer("props"))
        {
            uIFunc.DetailsOnDeSelect();
            _currentSelectedProp = null;
            return;
        }

        //check for prop component
        var propHit = _CursorRaycastHit.collider.gameObject.TryGetComponent<Prop>(out prop);
        
        if (propHit)
        {
            //first time selection
            if (_currentSelectedProp == null)
            {
                temp();
                return;
            }
            
            //if previous prop(currentselectedprop) not the same as current(prop)
            if (_currentSelectedProp != prop)
            {
                uIFunc.DetailsOnDeSelect();
                temp();
            }

        }


    }

    private void temp()
    {
        _currentSelectedProp = prop;
        uIFunc.DetailsOnSelect(_currentSelectedProp.PropData.Name, _currentSelectedProp.PropData.Cost.ToString(),_currentSelectedProp.transform.position);
        Debug.Log("New Selection: " + _currentSelectedProp.PropData.Name);
    }

    private bool TryPlaceBuilding()
    {
        if (!Physics.BoxCast(cursorPrefab.transform.position + Vector3.up * 3f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, out RaycastHit hit, Quaternion.identity, 5.0f, propsLayer)) return true;
        return false;
    }

    private GameObject TryDestroyBuilding()
    {
        return _CursorRaycastHit.collider.gameObject.CompareTag("Destructible")?_CursorRaycastHit.collider.gameObject:null;
    }
    private void CursorMethod()
    {
        _ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(_ray, out _CursorRaycastHit, 100f);
    }
}
