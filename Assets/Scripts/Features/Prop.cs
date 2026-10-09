using UnityEngine;

public abstract class Prop : MonoBehaviour
{
    [SerializeField] private PropData propData;
    public PropData PropData => propData;
}
