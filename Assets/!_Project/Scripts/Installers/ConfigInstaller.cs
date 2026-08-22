using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField] private EnemyPrefabsConfig _enemyPrefabsConfig;

    public override void InstallBindings()
    {
        Container.BindInstance<EnemyPrefabsConfig>(_enemyPrefabsConfig).AsSingle().NonLazy();
    }
}