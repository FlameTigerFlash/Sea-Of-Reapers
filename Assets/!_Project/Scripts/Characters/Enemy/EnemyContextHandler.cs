using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Zenject.SpaceFighter;

public class EnemyContextHandler : MonoBehaviour
{
    [SerializeField, NotNull] private UtilityBrain _brain;
    [SerializeField, NotNull] private Engine _mainEngine;
    [SerializeField, NotNull] private Rotor _mainEngineRotor;
    [SerializeField, NotNull] private Radar _radar;
    [SerializeField, NotNull] private Cannon _cannon;

    [SerializeField, NotNull] private CharacterHP _health;

    [SerializeField, NotNull] private DeathHandler _deathHandler;

    [SerializeField] private bool _startActive = true;

    private MapLocator _mapLocator;

    public EnemyContext Context => _context;

    public bool IsActive { get; private set;}

    private EnemyContext _context;

    private readonly List<IUpdateControls> _controllers = new();

    private EngineController _engineController;

    private void Start()
    {
        IsActive = _startActive;

        _context = new(gameObject);
        SetupFields();
        SetupComponents();
    }

    private void Update()
    {
        if (IsActive)
        {
            ProcessControls();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_context == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_context.WaypointPosition.Value, 3f);

        if (_context.TargetObject.Value != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_context.TargetObject.Value.transform.position, 3f);
        }
    }

    [Inject]
    public void Construct(MapLocator mapLocator)
    {
        _mapLocator = mapLocator;
    }

    public void SetActive()
    {
        IsActive = true;
    }

    public void SetInactive()
    {
        if (!IsActive)
        {
            return;
        }
        IsActive = false;

        foreach (var controller in _controllers)
        {
            controller.HaltControls();
        }
    }

    private void ProcessControls()
    {
        _brain.Process(_context);

        foreach (var controller in _controllers)
        {
            controller.UpdateControls();
        }
    }

    private void SetupFields()
    {
        _context.Health = _health.HPListener;

        if (_mapLocator != null)
        {
            _context.TargetObject = _mapLocator.PlayerListener;
        }
    }

    private void SetupComponents()
    {
        _engineController = new EngineController(_mainEngine, _mainEngineRotor, Context.ThrustMultiplier, Context.ThrustDirection);
        _controllers.Add(_engineController);

        _context.Brain = _brain;
        _context.Radar = _radar;
        _context.Cannon = _cannon;

        _context.Health.ValueChangedEvent += (float newHp) =>
        {
            if (newHp <= 0)
            {
                HandleDeath();
            }
        };
        _brain.Initialize(_context);
    }

    private void HandleDeath()
    {
        _deathHandler.OnHandleDeath();
    }
}
