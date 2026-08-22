using Character.Enemy;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private GameObject _enemyPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(_enemyPrefab);

        Container.Bind<IFactory<EnemyTypes, Vector3, Quaternion, EnemyContextHandler>>()
        .To<CustomEnemyFactory>()
        .AsSingle();

        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PlayerDiedSignal>();
        Container.DeclareSignal<PlayerFoundSignal>();
    }
}