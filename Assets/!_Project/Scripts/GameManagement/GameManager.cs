using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameManager : MonoBehaviour
{
    private WaveManager _waveManager;

    private MapLocator _mapLocator;

    private void Update()
    {
        if (_mapLocator.EnemiesCollection.Count == 0)
        {
            HandleWaveInitiation();
        }
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
        Debug.Log("Victory!");
    }

    private void HandleDefeat()
    {
        Debug.Log("Defeat!");
    }

    private void HandleWaveInitiation()
    {
        bool canInitiate = _waveManager.TryInitiateWave();

        if (!canInitiate)
        {
            HandleVictory();
        }
    }
}
