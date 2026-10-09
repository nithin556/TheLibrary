using System;
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
    private TextElement nameField;
    private TextElement costField;
    [SerializeField] private Cursor _cursor;
    public enum ActiveMode
    {
        Select,
        Build,
        Destroy
    }

    public ActiveMode selectedMode;

    private void Awake()
    {
        _panelRenderer = GetComponent<PanelRenderer>();
        selectedMode = ActiveMode.Select; // Default mode
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
        nameField =  root.Q<TextElement>("NameField");
        costField =  root.Q<TextElement>("CostField");

        _selectButton.clicked += OnSelectButtonClicked;
        _buildButton.clicked += OnBuildButtonClicked;
        _destroyButton.clicked += OnDestroyButtonClicked;
    }

    private void Update()
    {
        if (_cursor._currentSelectedProp != null)
        {
            TrackObjectMethod();
        }
    }

    private void TrackObjectMethod()
    {
        var screenCoord = mainCamera.WorldToScreenPoint(_cursor._currentSelectedProp.gameObject.transform.position);
        Vector2 uiScreenCoord = new Vector2(screenCoord.x,Screen.height - screenCoord.y);
        _displayElement.style.position = Position.Absolute;
        _displayElement.style.left = uiScreenCoord.x;
        _displayElement.style.top = uiScreenCoord.y;
    }

    private void OnSelectButtonClicked()
    {
        selectedMode = ActiveMode.Select;
    }
    private void OnBuildButtonClicked()
    {
        selectedMode = ActiveMode.Build;
    }
    private void OnDestroyButtonClicked()
    {
        selectedMode = ActiveMode.Destroy;
    }

    public VisualElement CursorOnUI()
    {
        _mousePos = Mouse.current.position.ReadValue();
        var uiPos = new Vector2(_mousePos.x, Screen.height - _mousePos.y);
        return _rootElement?.panel?.Pick(uiPos);
    }

    public void DetailsOnSelect(string nameString,string costString,Vector3 objectPosition)
    {
        _displayElement.style.display = DisplayStyle.Flex;
        nameField.text = nameString;
        costField.text = costString;
    }
    public void DetailsOnDeSelect()
    {
        _displayElement.style.display = DisplayStyle.None;
    }


}
