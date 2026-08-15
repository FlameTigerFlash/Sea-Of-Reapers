using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "Scriptable Objects/WaveConfig")]
public class WavesConfig : ScriptableObject
{
    [field: SerializeField] public List<int> EnemiesCount { get; private set; }

    private void OnValidate()
    {
        for (int i = 0; i < EnemiesCount.Count; i++)
        {
            EnemiesCount[i] = Mathf.Max(1, EnemiesCount[i]);
        }
    }
}
