using UnityEngine;

[System.Serializable]
public class PropData
{
    [SerializeField] private string name;
    [SerializeField] private int cost;
    public int Cost => cost;
    public string Name => name;
    
}
