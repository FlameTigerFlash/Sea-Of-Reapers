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
        if (_wavesConfig == null)
        {
            return false;
        }
        return _waveNum < _wavesConfig.Waves.Count;
    }

    public bool TryInitiateWave()
    {
        if (!CanInitiateWave())
        {
            return false;
        }
        if (_wavesConfig.Waves[_waveNum].Enemies.Count == 0)
        {
            Debug.LogError($"Invalid wave {_waveNum} size.");
            return false;
        }

        InitiateWave();
        _waveNum++;

        return true;
    }

    private void InitiateWave()
    {
        var curWave = _wavesConfig.Waves[_waveNum];
        int enemiesCount = curWave.Enemies.Count;

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
                    used[i] = true;
                    _spawners[i].SpawnEnemy(curWave.Enemies[curEnemy]);
                    break;
                }
                cnt++;
            }
            curEnemy++;
        }
    }
}
