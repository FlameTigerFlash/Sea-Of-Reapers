using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

public class ScoreManager : MonoBehaviour
{
    [SerializeField, Min(0)] private int _scoreForEnemy = 1;

    public UnityEvent<int> ScoreChangedEvent;

    public int Score
    {
        get => _score;
        set
        {
            float temp = _score;
            _score = value;
            if (temp != _score)
            {
                ScoreChangedEvent.Invoke(_score);
            }
        }
    }

    private MapLocator _mapLocator;

    private int _score = 0;

    private void OnDestroy()
    {
        _mapLocator.EnemyDataChangedEvent -= OnEnemyInfoChanged;
    }

    [Inject]
    public void Construct(MapLocator locator)
    {
        _mapLocator = locator;
        _mapLocator.EnemyDataChangedEvent += OnEnemyInfoChanged;
    }

    public void OnEnemyInfoChanged(GameObject _, bool added)
    {
        if (!added)
        {
            AddScore(_scoreForEnemy);
        }
    }

    public void AddScore(int toAdd)
    {
        Score += Mathf.Abs(toAdd);
    }
}
