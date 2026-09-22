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
    private bool ground = false;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        inputHandle.OnBuildEvent += OnBuildPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool ground = Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer);
        if (ground)
        {
            cursorPrefab.transform.position = hit.point;
        }
    }
    void OnBuildPerformed(object sender, EventArgs e)
    {
        bool check = Physics.BoxCast(cursorPrefab.transform.position + Vector3.up * 3f, new Vector3(0.5f, 0.5f, 0.5f), Vector3.down, quaternion.identity, 2.0f, anotherLayer);
        if (!check)
        {
            Instantiate(buildPrefab, cursorPrefab.transform.position, quaternion.identity);
        }
        else
        {
            Debug.Log("already there");
        }
    }
}
