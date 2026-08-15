using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> _spawners = new();

    [SerializeField, NotNull] private WavesConfig _wavesConfig;

    private int _waveNum = 0;

    public bool CanInitiateWave()
    {
        return _waveNum < _wavesConfig.EnemiesCount.Count;
    }

    public bool TryInitiateWave()
    {
        if (!CanInitiateWave())
        {
            return false;
        }
        if (_wavesConfig.EnemiesCount[_waveNum] <= 0)
        {
            Debug.LogError($"Invalid wave {_waveNum} size: {_wavesConfig.EnemiesCount[_waveNum]}.");
            return false;
        }

        InitiateWave();
        _waveNum++;

        return true;
    }

    private void InitiateWave()
    {
        int enemiesCount = _wavesConfig.EnemiesCount[_waveNum];

        if (enemiesCount >= _spawners.Count)
        {
            Debug.LogWarning($"Enemies count per wave should not exceed spawners count.");
        }

        bool[] used = new bool[_spawners.Count];
        int curEnemy = 0;
        while (curEnemy < enemiesCount)
        {
            int curChoice = Random.Range(0, _spawners.Count - 1 - curEnemy);
            int cnt = 0;
            for (int i = 0; i < _spawners.Count; i++)
            {
                if (used[i])
                {
                    continue;
                }
                if (cnt == curChoice)
                {
                    used[i] = false;
                    _spawners[i].SpawnEnemy();
                }
                cnt++;
            }
            curEnemy++;
        }
    }
}
