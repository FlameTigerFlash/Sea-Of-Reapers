using UnityEngine;
using Zenject;

public class PlayerInfoPresenter : MonoBehaviour
{
    [SerializeField] private ScoreDisplay _scoreDisplay;
    [SerializeField] private HealthDisplay _healthDisplay;
    [SerializeField] private AimingDisplay _aimingDisplay;

    [Inject] private MapLocator _mapLocator;
    [Inject] private ScoreManager _scoreManager;

    private PlayerContext _playerContext;
    private CharacterHP _playerHp;

    private void Awake()
    {
        _scoreManager.ScoreChangedEvent.AddListener(OnScoreChanged);
        _mapLocator.PlayerListener.ValueChangedEvent += OnPlayerFound;
    }

    private void OnDestroy()
    {
        _scoreManager.ScoreChangedEvent.RemoveListener(OnScoreChanged);
        _mapLocator.PlayerListener.ValueChangedEvent -= OnPlayerFound;
        if (_playerHp != null)
        {
            _playerHp.HPListener.ValueChangedEvent -= OnHealthChanged;
        }

        if (_playerContext != null)
        {
            _playerContext.AimingStickPosition.ValueChangedEvent -= OnAimingPosChanged;
        }
    }

    public void OnPlayerFound(GameObject player)
    {
        _playerHp = player.GetComponentInChildren<CharacterHP>();
        if (_playerHp != null)
        {
            OnHealthChanged(_playerHp.HPListener.Value);
            _playerHp.HPListener.ValueChangedEvent += OnHealthChanged;
        }

        _playerContext = player.GetComponentInChildren<PlayerContextHandler>()?.Context;
        if (_playerContext != null)
        {
            _playerContext.AimingStickPosition.ValueChangedEvent += OnAimingPosChanged;
        }
    }

    public void OnAimingPosChanged(Vector2 pos)
    {
        if (!_playerContext.IsAiming)
        {
            _aimingDisplay?.Hide();
            return;
        }
        _aimingDisplay?.Display(pos);
    }

    public void OnScoreChanged(int score)
    {
        _scoreDisplay?.OnDisplay(score);
    }

    public void OnHealthChanged(float health)
    {
        _healthDisplay?.OnDisplay(health);
    }
}
