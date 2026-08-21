using Character;
using Character.Player;
using UnityEngine;
using Zenject;

public class PlayerInfoPresenter : MonoBehaviour
{
    [SerializeField] private ScoreDisplay _scoreDisplay;
    [SerializeField] private HealthDisplay _healthDisplay;
    [SerializeField] private AimingDisplay _aimingDisplay;

    private SignalBus _signalBus;
    private ScoreManager _scoreManager;

    private PlayerContext _playerContext;
    private CharacterHP _playerHp;

    private void Start()
    {
        _scoreManager.ScoreChangedEvent.AddListener(OnScoreChanged);
    }

    private void OnDestroy()
    {
        _scoreManager.ScoreChangedEvent.RemoveListener(OnScoreChanged);
        if (_playerHp != null)
        {
            _playerHp.HPListener.ValueChangedEvent -= OnHealthChanged;
        }

        if (_playerContext != null)
        {
            _playerContext.AimingStickPosition.ValueChangedEvent -= OnAimingPosChanged;
        }
    }

    [Inject]
    public void Construct(ScoreManager scoreManager, SignalBus signalBus)
    {
        _scoreManager = scoreManager;
        _signalBus = signalBus;

        _signalBus.Subscribe<PlayerFoundSignal>(OnPlayerFound);
        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
    }

    public void OnPlayerFound(PlayerFoundSignal sig)
    {
        var player = sig.Player;
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

    public void OnPlayerDied()
    {
        _aimingDisplay.Hide();
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
