using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private PlayerPawn _playerPawn;
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private TextMeshProUGUI _endText;
    [SerializeField] private Transform _camera;

    public override void InstallBindings()
    {
        GridManager gridManager = new GridManager();
        //states
        Container.Bind<StartAnimationState>().AsSingle();
        Container.Bind<PlayerTurnState>().AsSingle();
        Container.Bind<EnemyTurnState>().AsSingle();
        Container.Bind<LevelTransitionState>().AsSingle();
        Container.Bind<UpgradeState>().AsSingle();
        Container.Bind<PlayerDeathState>().AsSingle();
        Container.Bind<StateManager>().AsSingle().NonLazy();

        MonoBehaviour coroutines = new GameObject("Coroutines").AddComponent<Coroutines>();
        Container.Bind<MonoBehaviour>().FromInstance(coroutines).AsTransient();

        Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
        Container.Bind<PlayerPawn>().FromInstance(_playerPawn).AsSingle();
        Container.Bind<GameplayUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyManager>().AsSingle();

        Container.Bind<TextMeshProUGUI>().WithId("StartText").FromInstance(_startText);
        Container.Bind<TextMeshProUGUI>().WithId("EndText").FromInstance(_endText);
        Container.Bind<Transform>().WithId("Camera").FromInstance(_camera);

    }
}