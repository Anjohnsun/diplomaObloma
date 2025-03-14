using System.Collections.Generic;
using UnityEngine;

public class MoveAction : IPawnAction
{
    private Pawn _pawn;
    private IMoveStrategy _moveStrategy;
    private GridManager _gridManager;

    public MoveAction(Pawn pawn, IMoveStrategy moveStrategy, GridManager gridManager)
    {
        _pawn = pawn;
        _moveStrategy = moveStrategy;
        _gridManager = gridManager;
    }

    public List<Vector2Int> CalculateTargets()
    {
        return _moveStrategy.GetPossibleMoves(Vector3Int.CeilToInt(_pawn.transform.position));
    }

    public void Perform(Vector2Int point)
    {
        _gridManager.MovePawnTo(_pawn, point);
    }
}
