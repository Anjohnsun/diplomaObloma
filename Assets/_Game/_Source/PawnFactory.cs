using UnityEngine;
using Zenject;

public class PawnFactory : IFactory<Pawn>
{
    private PawnStatsSO _pawnStats;
    private GridManager _gridManager;
    private IMoveStrategy _moveStrategy;

    [Inject]
    public PawnFactory(PawnStatsSO pawnStats, GridManager gridManager, IMoveStrategy moveStrategy)
    {
        _pawnStats = pawnStats;
        _gridManager = gridManager;
        _moveStrategy = moveStrategy;
    }

    public Pawn Create()
    {
        Pawn pawn = GameObject.Instantiate(new GameObject()).AddComponent<Pawn>();
        pawn.Construct(_pawnStats, _gridManager);

        return pawn;
    }

    public Pawn Create(int i)
    {
        return null;
    }
}
