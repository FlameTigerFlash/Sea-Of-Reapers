using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "Scriptable Objects/WaveConfig")]
public class WavesConfig : ScriptableObject
{
    [field: SerializeField] public List<SingleWaveConfig> Waves { get; private set; }

    [Serializable]
    public class SingleWaveConfig
    {
        [field: SerializeField] public List<EnemyTypes> Enemies { get; private set; } = new List<EnemyTypes>();
    }
}
