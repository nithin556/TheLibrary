using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisuals : MonoBehaviour
{
    [SerializeField] private List<GameObject> cursors;
    [SerializeField] private float rotationSpeed;
    private GameObject _selectedCursor;
    private GameObject _currentCursor;
    private Coroutine _activeCoroutine;
    private  Quaternion _prevTargetRot;
    

    private void Awake()
    {
        _selectedCursor = cursors[0]; // runtime selecting yet to develop
        _currentCursor = Instantiate(_selectedCursor,transform,false);
        _activeCoroutine = null;
    }

    public void RotateTo(Quaternion  cursorTargetRot)
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _currentCursor.transform.rotation = _prevTargetRot;
        }

        _prevTargetRot = cursorTargetRot;
        _activeCoroutine = StartCoroutine((CursorRotateCoroutine(cursorTargetRot)));
    }

    private IEnumerator CursorRotateCoroutine(Quaternion  cursorTargetRot)
    {
        while (_currentCursor.transform.rotation != cursorTargetRot)
        {
            _currentCursor.transform.rotation = Quaternion.RotateTowards(_currentCursor.transform.rotation,cursorTargetRot,
                Time.deltaTime * rotationSpeed);
            yield return null;
        }
        _activeCoroutine = null;
    }
}
