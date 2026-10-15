using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIFunctionality : MonoBehaviour
{
    private PanelRenderer _panelRenderer;
    private Button _selectButton;
    private Button _buildButton;
    private Button _destroyButton;
    private Vector2 _mousePos;
    [SerializeField] private Camera mainCamera;
    private VisualElement _rootElement;
    private VisualElement _displayElement;
    private TextElement _nameField;
    private TextElement _costField;
    [SerializeField] private Cursor cursor;
    
    [SerializeField] private InteractionController  interactionController;
    private Prop _currentSelectedProp;
    private Vector2 _uiScreenCoord;


    private void Awake()
    {
        _panelRenderer = GetComponent<PanelRenderer>();
    }

    private void Start()
    {
        DetailsOnDeSelect();
    }


    private void OnEnable()
    {
        _panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        _panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        _selectButton.clicked -= OnSelectButtonClicked;
        _buildButton.clicked -= OnBuildButtonClicked;
        _destroyButton.clicked -= OnDestroyButtonClicked;
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        _rootElement = root;
        _selectButton = root.Q<Button>("Select-Button");
        _buildButton = root.Q<Button>("Build-Button");
        _destroyButton = root.Q<Button>("Destroy-Button");

        _displayElement = root.Q<VisualElement>("DetailsPanel");
        _nameField =  root.Q<TextElement>("NameField");
        _costField =  root.Q<TextElement>("CostField");

        _selectButton.clicked += OnSelectButtonClicked;
        _buildButton.clicked += OnBuildButtonClicked;
        _destroyButton.clicked += OnDestroyButtonClicked;
    }

    private void Update()
    {
        UpdateVisualElementPos();
    }



    private void OnSelectButtonClicked()
    {
        interactionController.SetMode(ActiveMode.Select);
    }
    private void OnBuildButtonClicked()
    {
        interactionController.SetMode(ActiveMode.Build);
    }
    private void OnDestroyButtonClicked()
    {
        interactionController.SetMode(ActiveMode.Destroy);
    }

    public VisualElement CursorOnUI()
    {
        _mousePos = Mouse.current.position.ReadValue();
        var uiPos = new Vector2(_mousePos.x, Screen.height - _mousePos.y);
        return _rootElement?.panel?.Pick(uiPos);
    }

    public void DetailsOnSelect(string nameString,string costString,Prop currentSelectedProp)
    {
        if (currentSelectedProp == null) return;

        _currentSelectedProp = currentSelectedProp; // trackObject in update
        
        _displayElement.style.display = DisplayStyle.Flex;
        _nameField.text = "Name: "+nameString;
        _costField.text = "Cost: "+costString;
        
        _uiScreenCoord =  TrackObjectMethod(currentSelectedProp.transform.position);// to avoid update frame gap 
        
    }
    public void DetailsOnDeSelect()
    {
        Debug.Log("Details on Deselect");
        _currentSelectedProp = null;
        _displayElement.style.display = DisplayStyle.None;
    }
    private void UpdateVisualElementPos()
    {
        if (_currentSelectedProp)
        {
            _uiScreenCoord = TrackObjectMethod(_currentSelectedProp.transform.position);
            _displayElement.style.position = Position.Absolute;
            _displayElement.style.left = _uiScreenCoord.x;
            _displayElement.style.top = _uiScreenCoord.y;
        }
    }
    private Vector2 TrackObjectMethod(Vector3  objectPosition)
    {
        var screenCoord = mainCamera.WorldToScreenPoint(objectPosition);
        Vector2 coord = new Vector2(screenCoord.x,Screen.height - screenCoord.y);
        return  coord;
    }


}
