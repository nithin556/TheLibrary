using System.Collections.Generic;
using UnityEngine;

public class PropGenerator : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private PropGenerationData  data;
    private List<PropSpawn>  _spawns;

    public void PropGenerationMethod()
    {
        _spawns = data.spawns;
        foreach (var propSpawn in _spawns)
        {
            for(int i = 0;i < propSpawn.instances;i++)
            {
                Instantiate(propSpawn.propPrefab, parentTransform.position, Quaternion.identity,parentTransform);
            }
        }
    }
    
}
