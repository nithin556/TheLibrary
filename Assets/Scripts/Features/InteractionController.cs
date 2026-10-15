using System;
using UnityEngine;
public enum ActiveMode
            {
                Select,
                Build,
                Destroy
            }
public class InteractionController : MonoBehaviour
{
    
    private ActiveMode _activeMode;
    private inputHandle _inputHandle;
    [SerializeField] private Cursor cursor;
    [SerializeField] private UIFunctionality  uiFunctionality;
    
    public Prop CurrentSelectedProp { get; private set; }
    private RaycastHit  _cursorRaycastHit;
    
    [SerializeField] private GameObject buildPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask propsLayer;
    private Prop _prop;
    
    private Quaternion currentRot;
    public Quaternion CursorTargetRot { get; private set; }
    [SerializeField] private CursorVisuals cursorVisuals;

    private void Awake()
    {
        _activeMode = ActiveMode.Select;
        _inputHandle = GetComponent<inputHandle>();
    }

    private void OnEnable()
    {
        _inputHandle.OnLeftClickEvent += OnLeftClickPerformed;
        _inputHandle.OnRotateEvent += OnRotatePerformed;
    }

    private void OnDisable()
    {
        _inputHandle.OnLeftClickEvent -= OnLeftClickPerformed;
        _inputHandle.OnRotateEvent -= OnRotatePerformed;
    }

    private void Start()
    {
        currentRot = cursor.GetCursorParentTransform().rotation;
        CursorTargetRot = Quaternion.identity;
    }
    
    private void OnRotatePerformed(object sender, EventArgs e)
    {
        CursorRotateMethod();
    }
    private void CursorRotateMethod()
    {
        if (_activeMode != ActiveMode.Build) return;
        CursorTargetRot = currentRot * Quaternion.Euler(0f, 90f, 0f);
        currentRot = CursorTargetRot;
        cursorVisuals.RotateTo(CursorTargetRot);
    }


    public void SetMode(ActiveMode mode)
    {
        _activeMode = mode;
    }
    private void OnLeftClickPerformed(object sender, EventArgs e)
    {
        _cursorRaycastHit = cursor.GetCursorRaycastHit();
        ToolModeMethod();
    }

    private void ToolModeMethod()
    {
        if (uiFunctionality.CursorOnUI() != null) return;
        //not buildable/clickable
        if (!_cursorRaycastHit.collider)
        {
            uiFunctionality.DetailsOnDeSelect();
            CurrentSelectedProp = null;
            return;
        }
        switch (_activeMode)
        {
            case ActiveMode.Select:
            {
                SelectModeMethod();
                break;
            }
            case ActiveMode.Build:
            {
                BuildModeMethod();
                break;
            }
            case ActiveMode.Destroy:
            {
                DestroyModeMethod();
                break;
            }
        }
    }

    private void DestroyModeMethod()
    {
        uiFunctionality.DetailsOnDeSelect();
        CurrentSelectedProp = null;
        if (_cursorRaycastHit.collider.gameObject.layer != LayerMask.NameToLayer("props")) return;
        Destroy(TryDestroyBuilding()); // Handles null automatically
    }

    private void BuildModeMethod()
    {
        uiFunctionality.DetailsOnDeSelect();
        CurrentSelectedProp = null;
        if (_cursorRaycastHit.collider.gameObject.layer != LayerMask.NameToLayer("Buildable")) return;
        if (TryPlaceBuilding())
        {
            //already created at start so just set active
            Instantiate(buildPrefab, _cursorRaycastHit.point, CursorTargetRot);
        }
    }

    private void SelectModeMethod()
    {
        // if objecthit does not belong to prop_layer , return
        if (_cursorRaycastHit.collider.gameObject.layer != LayerMask.NameToLayer("props"))
        {
            uiFunctionality.DetailsOnDeSelect();
            CurrentSelectedProp = null;
            return;
        }

        //check for prop component
        var propHit = _cursorRaycastHit.collider.gameObject.TryGetComponent<Prop>(out _prop);
        
        if (propHit)
        {
            //first time selection
            if (CurrentSelectedProp == null)
            {
                SetSelectedProp();
                return;
            }
            
            //if previous prop(currentselectedprop) not the same as current(prop)
            if (CurrentSelectedProp != _prop)
            {
                uiFunctionality.DetailsOnDeSelect();
                SetSelectedProp();
            }

        }
    }
    private void SetSelectedProp()
    {
        CurrentSelectedProp = _prop;
        uiFunctionality.DetailsOnSelect(CurrentSelectedProp.PropData.Name, CurrentSelectedProp.PropData.Cost.ToString(),CurrentSelectedProp);
        Debug.Log("New Selection: " + CurrentSelectedProp.PropData.Name);
    }
    
    private bool TryPlaceBuilding()
    {
        var prefabBounds = buildPrefab.GetComponent<BoxCollider>().size;
        
        if (!Physics.BoxCast(_cursorRaycastHit.point + Vector3.up * 5f, prefabBounds*0.5f, Vector3.down, out RaycastHit hit,
                Quaternion.identity, 10.0f, propsLayer)) return true;
        return false;
        
    }

    private GameObject TryDestroyBuilding()
    {
        return _cursorRaycastHit.collider.gameObject.CompareTag("Destructible")?_cursorRaycastHit.collider.gameObject:null;
    }
}
