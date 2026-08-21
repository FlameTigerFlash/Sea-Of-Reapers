using Character;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Zenject;

namespace Character.Player
{
    public class PlayerContextHandler : MonoBehaviour
    {
        #region fields
        [SerializeField, NotNull] private CharacterHP _healthSystem;

        [SerializeField, NotNull] private DeathHandler _deathHandler;

        [SerializeField, NotNull] private PlayerMovementController _movementController;

        [SerializeField, NotNull] private PlayerInputHandler _inputHandler;

        [SerializeField, NotNull] private PlayerCannonController _cannonController;

        [SerializeField, NotNull] private PlayerMainCameraController _mainCamController;

        public PlayerContext Context => _context;

        private PlayerContext _context;

        private readonly List<IUpdateControls> _controllers = new();

        private AimingController _aimingController;

        private SignalBus _signalBus;
        #endregion

        private void OnValidate()
        {
            _healthSystem = _healthSystem != null ? _healthSystem : GetComponentInChildren<CharacterHP>();
            _deathHandler = _deathHandler != null ? _deathHandler : GetComponentInChildren<DeathHandler>();
            _movementController = _movementController != null ? _movementController : GetComponentInChildren<PlayerMovementController>();
            _inputHandler = _inputHandler != null ? _inputHandler : GetComponentInChildren<PlayerInputHandler>();
            _cannonController = _cannonController != null ? _cannonController : GetComponentInChildren<PlayerCannonController>();
            _mainCamController = _mainCamController != null ? _mainCamController : GetComponentInChildren<PlayerMainCameraController>();
        }

        private void Awake()
        {
            _context = new(gameObject);
            SetupFields();
            SetupComponents();
        }

        private void Start()
        {
            _signalBus.TryFire<PlayerFoundSignal>(new PlayerFoundSignal { Player = gameObject });
        }

        private void Update()
        {
            foreach (var controller in _controllers)
            {
                controller.UpdateControls();
            }
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void SetupFields()
        {
            _context.Health = _healthSystem.HPListener;
        }

        private void SetupComponents()
        {
            _context.Health.ValueChangedEvent += (float newHp) =>
            {
                if (newHp <= 0)
                {
                    HandleDeath();
                }
            };

            _movementController.Initialize(_context);
            _inputHandler.Initialize(_context);
            _cannonController.Initialize(_context);
            _mainCamController.Initialize(_context);

            _aimingController = new(_context);
            _controllers.Add(_aimingController);
        }

        private void HandleDeath()
        {
            _signalBus.TryFire<PlayerDiedSignal>();
            _deathHandler.OnHandleDeath();
        }
    }
}