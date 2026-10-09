using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PropGenerator _propGenerator;

    private void Awake()
    {
        _propGenerator = GetComponent<PropGenerator>();
    }

    private void Start()
    {
        _propGenerator.PropGenerationMethod();
    }
}
