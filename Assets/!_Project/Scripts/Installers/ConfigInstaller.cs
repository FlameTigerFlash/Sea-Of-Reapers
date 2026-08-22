using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField] private EnemyPrefabsConfig _enemyPrefabsConfig;
    [SerializeField] private FlyweightPrefabsConfig _flyweightPrefabsConfig;
    [SerializeField] private ProjectilePrefabsConfig _projectilePrefabsConfig;

    public override void InstallBindings()
    {
        Container.BindInstance<EnemyPrefabsConfig>(_enemyPrefabsConfig).AsSingle().NonLazy();
        Container.BindInstance<FlyweightPrefabsConfig>(_flyweightPrefabsConfig).AsSingle().NonLazy();
        Container.BindInstance<ProjectilePrefabsConfig>(_projectilePrefabsConfig).AsSingle().NonLazy();
    }
}