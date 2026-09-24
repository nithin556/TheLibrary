using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private GameObject buildPrefab;
    [SerializeField] private inputHandle inputHandle;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask anotherLayer;
    [SerializeField] private UIFunctionality uIFunc;
    private Ray ray;
    private RaycastHit rayHit;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        inputHandle.OnLeftClickEvent += OnLeftClickPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        CursorMethod();
    }

    private void CursorMethod()
    {
        ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out rayHit, 100f);
        cursorPrefab.transform.position = rayHit.point;
    }

    void OnLeftClickPerformed(object sender, EventArgs e)
    {
        if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Buildable") || rayHit.collider.gameObject.layer == LayerMask.NameToLayer("props"))
        {
            if (uIFunc.selectedMode == UIFunctionality.activeMode.select)
            {
                Debug.Log("Select Functionality yet to develop");
                return;
            }
            else if (uIFunc.selectedMode == UIFunctionality.activeMode.build)
            {
                bool check = Physics.BoxCast(cursorPrefab.transform.position + Vector3.up * 3f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, out RaycastHit hit, quaternion.identity, 5.0f, anotherLayer);
                if (!check)
                {
                    Instantiate(buildPrefab, cursorPrefab.transform.position, quaternion.identity);
                }
            }
            else
            {
                bool hitTable = Physics.BoxCast(cursorPrefab.transform.position + Vector3.up * 3f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, out RaycastHit hit, quaternion.identity, 5.0f, anotherLayer);
                if (hitTable && hit.collider.gameObject.CompareTag("Destructable"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
