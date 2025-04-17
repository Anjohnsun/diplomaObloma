using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private PlayerPawnHandler _playerHandler;

    public override void InstallBindings()
    {
        GridManager gridManager = new GridManager();
        //states
        Container.Bind<StartAnimationState>().AsSingle();
        Container.Bind<BattleState>().AsSingle();
        Container.Bind<UpgradeState>().AsSingle();
        Container.Bind<StateManager>().AsSingle();

        Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<PlayerPawnHandler>().FromInstance(_playerHandler).AsSingle();
        Container.Bind<IGameplayUIService>().To<GameplayUI>().FromComponentInHierarchy().AsSingle();

    }
}