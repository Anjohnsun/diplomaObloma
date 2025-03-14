using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Transform _uiObj;

    //��������, ��������
    [SerializeField] private PawnStatsSO _pawnStats;

    [Header("PlayerPawn")]
    [SerializeField] private Pawn _playerPawn;

    [Header("Grid Manager Settings")]
    [SerializeField] private int _lineDestroyFrequency;
    [SerializeField] private int _linesUpToPlayer;
    [SerializeField] private int _linesDownToPlayer;
    [SerializeField] private int _width;
    [SerializeField] private GameObject _tilePrefab;


    public override void InstallBindings()
    {

        //���������������� StateManager, GridManager, EnemyManager, PlayerPawn, UIManager
        Container.Bind<StateManager>().AsSingle();
        Container.Bind<GridManager>().AsSingle();

        //����� ��� grid manager
        Container.Bind<Pawn>().WithId("playerPawn").FromInstance(_playerPawn);
        Container.Bind<int>().WithId("lineDestroyFrequency").FromInstance(_lineDestroyFrequency);
        Container.Bind<int>().WithId("linesUpToPlayer").FromInstance(_linesUpToPlayer);
        Container.Bind<int>().WithId("linesDownToPlayer").FromInstance(_linesDownToPlayer);
        Container.Bind<int>().WithId("width").FromInstance(_width);
        Container.Bind<GameObject>().WithId("tilePrefab").FromInstance(_tilePrefab);


        //�������
        Container.Bind<IGameplayUIService>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IPawnMoverService>().To<PawnMoverService>().AsSingle();

        //���������������� ��������� (startAnimation, PlayerTurn, EnemyTurn, PlayerDeath)
        Container.Bind<StartAnimationState>().AsSingle();
        Container.Bind<PlayerTurnState>().AsSingle();

        MonoBehaviour coroutines = new GameObject("Coroutines").AddComponent<Coroutines>();
        Container.Bind<MonoBehaviour>().FromInstance(coroutines).AsTransient();


        //REWRITE THIS FOR SMTH ����������
        Container.Bind<PawnStatsSO>().FromInstance(_pawnStats);
        Container.Bind<BasePlayerMove>().AsSingle();
    }
}