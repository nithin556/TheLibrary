using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisuals : MonoBehaviour
{
    [SerializeField] private inputHandle inputHandle;
    [SerializeField] private Cursor cursorScript;
    private GameObject selectedCursor;
    [SerializeField] private List<GameObject> cursors;
    private RaycastHit _cursorRaycastHit;
    private GameObject currentCursor;
    
    public Quaternion cursorTargetRot;
    [SerializeField] private float rotationSpeed;
    private Coroutine activeCoroutine;
    

    private void Awake()
    {
        selectedCursor = cursors[0]; // runtime selecting yet to develop
        currentCursor = Instantiate(selectedCursor, Vector3.zero, Quaternion.identity);
    }

    private void Start()
    {
        inputHandle.OnRotateEvent += OnRotatePerformed;
    }

    private void Update()
    {
        _cursorRaycastHit = cursorScript._CursorRaycastHit;
        currentCursor.transform.position = _cursorRaycastHit.point;
    }
    private void OnRotatePerformed(object sender, EventArgs e)
    {
        CursorRotateMethod();
    }
    private void CursorRotateMethod()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            currentCursor.transform.rotation = cursorTargetRot;
        }
        activeCoroutine = StartCoroutine((CursorRotateCoroutine()));
    }

    private IEnumerator CursorRotateCoroutine()
    {
        cursorTargetRot = currentCursor.transform.rotation * Quaternion.Euler(0f, 90f, 0f);
        while (currentCursor.transform.rotation != cursorTargetRot)
        {
            currentCursor.transform.rotation = Quaternion.RotateTowards(currentCursor.transform.rotation, cursorTargetRot,
                Time.deltaTime * rotationSpeed);
            yield return null;
        }
        activeCoroutine = null;
    }
}
