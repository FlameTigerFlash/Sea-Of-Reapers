using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _sceneChangingDelay = 2f;

    private WaveManager _waveManager;

    private MapLocator _mapLocator;

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
    public void Construct(MapLocator locator, WaveManager waveManager)
    {
        _mapLocator = locator;
        _mapLocator.PlayerListener.ValueChangedEvent += OnPlayerFound;

        _waveManager = waveManager;
    }

    public void OnPlayerFound(GameObject player)
    {
        var deathHandler = player.GetComponentInChildren<DeathHandler>();
        if (deathHandler == null)
        {
            Debug.LogError("Player must have a Death Handler component.");
            return;
        }

        deathHandler.DeathEvent.AddListener(OnPlayerDied);
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
