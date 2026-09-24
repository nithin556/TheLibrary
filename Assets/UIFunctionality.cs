using UnityEngine;
using UnityEngine.UIElements;

public class UIFunctionality : MonoBehaviour
{
    private PanelRenderer panelRenderer;
    private Button Selectbutton;
    private Button Buildbutton;
    private Button Destroybutton;
    public enum activeMode
    {
        select,
        build,
        destroy
    }

    public activeMode selectedMode;

    void Awake()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        selectedMode = activeMode.select;
    }
    private void OnEnable()
    {
        // Registers callback invoked on initialization and live reloads
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnDisable()
    {
        panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        Selectbutton.clicked -= OnSelectButtonClicked;
        Buildbutton.clicked -= OnBuildButtonClicked;
        Destroybutton.clicked -= OnDestroyButtonClicked;
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        Selectbutton = root.Q<Button>("Select-Button");
        Buildbutton = root.Q<Button>("Build-Button");
        Destroybutton = root.Q<Button>("Destroy-Button");

        Selectbutton.clicked += OnSelectButtonClicked;
        Buildbutton.clicked += OnBuildButtonClicked;
        Destroybutton.clicked += OnDestroyButtonClicked;
    }

    private void OnSelectButtonClicked()
    {
        selectedMode = activeMode.select;
    }
    private void OnBuildButtonClicked()
    {
        selectedMode = activeMode.build;
    }
    private void OnDestroyButtonClicked()
    {
        selectedMode = activeMode.destroy;
    }
}
