using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
using Zenject;

public class PawnFactory : IFactory<Pawn>
{
    private List<PawnStatsSO> _pawnStats;
    private GridManager _gridManager;



    [Inject]
    public PawnFactory(GridManager gridManager, List<PawnStatsSO> pawnStatsSOs)
    {
        _gridManager = gridManager;
        _pawnStats = pawnStatsSOs;
    }

    public Pawn Create(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.movingPawn:
                {
                    break;
                }
            case EnemyType.simpleAttackPawn:
                {
                    break;
                }
        }
        return null;
    }

    public Pawn Create()
    {
        throw new System.NotImplementedException();
    }
}

public enum EnemyType
{
    movingPawn,
    simpleAttackPawn
}
