using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private inputHandle inputVectorScript;
    [SerializeField, Min(1)] private int start_Section;
    [SerializeField] private int start_Floor;
    [SerializeField] private int[] floors;
    private Vector2 inputVector;
    private int currentX = 1;
    private int currentY = 0; // 0 is ground floor
    private void OnValidate()
    {
        if (start_Section < 1)
        {
            start_Section = 1;
        }
    }

    void Awake()
    {
        inputVector = Vector2.zero;
        currentX = start_Section;
        currentY = start_Floor;
        Debug.Log("Floor: " + currentY + " Section: " + currentX);

        if (floors.Length == 0)
        {
            Debug.Log("zero floors");
        }
    }

    void OnEnable()
    {
        inputVectorScript.OnCancelEvent += handleMove;
    }
    void OnDisable()
    {
        inputVectorScript.OnCancelEvent -= handleMove;
    }

    private void handleMove(object sender, EventArgs eventArgs)
    {
        Floor_Section_Values();
    }
    private void Floor_Section_Values()
    {
        if (floors.Length == 0)
        {
            Debug.Log("zero floors");
            return;
        }
        inputVector = inputVectorScript.cancelInputVector;
        // W
        if (inputVector.x == 0 && inputVector.y > 0 && currentY < (floors.Length - 1) && floors[currentY + 1] > 0)
        {
            if (currentX > floors[currentY + 1])
            {
                currentX = floors[currentY + 1];
                currentY++;
            }
            else
            {
                currentY++;
            }
        }
        // S
        else if (inputVector.x == 0 && inputVector.y < 0 && currentY > 0)
        {
            if (currentX > floors[currentY - 1])
            {
                currentX = floors[currentY - 1];
                currentY--;
            }
            else
            {
                currentY--;
            }
        }
        // A
        else if (inputVector.x < 0 && inputVector.y == 0 && currentX > 1)
        {
            currentX--;
        }
        // D
        else if (inputVector.x > 0 && inputVector.y == 0 && currentX < floors[currentY])
        {
            currentX++;
        }

        Debug.Log("Floor: " + currentY + " Section: " + currentX);
    }
}