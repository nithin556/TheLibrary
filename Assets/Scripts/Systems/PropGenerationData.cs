using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPropGenerationData", menuName = "ScriptableObjects/PropGenerationData")]
public class PropGenerationData : ScriptableObject
{
    public List<PropSpawn>  spawns;
}

[Serializable]
public class PropSpawn
{
    public GameObject  propPrefab;
    [Min(0)]
    public int instances;
}
