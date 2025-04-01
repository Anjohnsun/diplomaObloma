using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform _uiObj;

    //заглушка, заменить
    [SerializeField] private PawnStatsSO _pawnStats;

    [Header("PlayerPawn")]
    [SerializeField] private Pawn _playerPawn;
    [SerializeField] private PlayerPawnHandler _pawnHandler;

    [Header("Grid Manager Settings")]
    [SerializeField] private GridSettingsSO _gridSettings;
    [SerializeField] private int _lineDestroyFrequency;
    [SerializeField] private int _linesUpToPlayer;
    [SerializeField] private int _linesDownToPlayer;
    [SerializeField] private int _width;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] Vector2Int _playerStartPosition;

    [Header("Mover service settings")]
    [SerializeField] private MoverSettingsSO _pawnMoverSettings;


    public override void InstallBindings()
    {

        //Зарегистрировать StateManager, GridManager, EnemyManager, PlayerPawn, UIManager
        Container.Bind<StateManager>().AsSingle();
        Container.Bind<GridManager>().AsSingle();

        //Бинды для grid manager
        Container.Bind<GridSettingsSO>().FromInstance(_gridSettings);
        Container.Bind<Pawn>().WithId("playerPawn").FromInstance(_playerPawn);

        Container.Bind<PlayerPawnHandler>().FromInstance(_pawnHandler);

        //Сервисы
        Container.Bind<IGameplayUIService>().FromComponentInHierarchy().AsSingle();
        IPawnMoverService mover = new PawnMoverService(_pawnMoverSettings);
        Container.Bind<IPawnMoverService>().To<PawnMoverService>().AsSingle().WithArguments(_pawnMoverSettings);

        //Зарегистрировать состояния (startAnimation, PlayerTurn, EnemyTurn, PlayerDeath)
        Container.Bind<StartAnimationState>().AsSingle();
        Container.Bind<PlayerTurnState>().AsSingle();
        Container.Bind<EnemyTurnState>().AsSingle();

        MonoBehaviour coroutines = new GameObject("Coroutines").AddComponent<Coroutines>();
        Container.Bind<MonoBehaviour>().FromInstance(coroutines).AsTransient();


        //REWRITE THIS FOR SMTH адекватное
        Container.Bind<PawnStatsSO>().FromInstance(_pawnStats);
        Container.Bind<BasePlayerMove>().AsSingle();
    }
}