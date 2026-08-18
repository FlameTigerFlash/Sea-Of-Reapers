using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _sceneChangingDelay = 2f;

    private WaveManager _waveManager;

    private MapLocator _mapLocator;

    private SignalBus _signalBus;

    private bool _gameOver = false;

    private void Update()
    {
        if (_mapLocator.EnemiesCollection.Count == 0)
        {
            HandleWaveInitiation();
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    [Inject]
    public void Construct(MapLocator locator, WaveManager waveManager, SignalBus signalBus)
    {
        _mapLocator = locator;

        _waveManager = waveManager;

        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
    }

    public void OnPlayerDied()
    {
        HandleDefeat();
    }

    private void HandleVictory()
    {
        StartCoroutine(DelayedSceneChangedCoroutine(SceneType.VictoryScene, _sceneChangingDelay));
    }

    private void HandleDefeat()
    {
        StartCoroutine(DelayedSceneChangedCoroutine(SceneType.DefeatScene, _sceneChangingDelay));
    }

    private void HandleWaveInitiation()
    {
        bool canInitiate = _waveManager.TryInitiateWave();

        if (!canInitiate)
        {
            HandleVictory();
        }
    }

    private IEnumerator DelayedSceneChangedCoroutine(SceneType sceneType, float delay = 2f)
    {
        if (!_gameOver)
        {
            _gameOver = true;

            yield return new WaitForSeconds(delay);
            SceneChanger.Instance.ChangeSceneTo(sceneType);
        }
        else
        {
            yield return null;
        }
    }
}
