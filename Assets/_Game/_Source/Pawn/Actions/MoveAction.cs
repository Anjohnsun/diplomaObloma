using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : IPawnAction
{
    private Pawn _pawn;
    private IMoveStrategy _moveStrategy;
    private GridManager _gridManager;

    private List<Vector2Int> _possibleMoves;

    public Pawn pawn => _pawn;
    public float duration => 0.6f;

    public MoveAction(Pawn pawn, IMoveStrategy moveStrategy, GridManager gridManager)
    {
        _pawn = pawn;
        _moveStrategy = moveStrategy;
        _gridManager = gridManager;
    }

    public List<Vector2Int> CalculateTargets()
    {
        _possibleMoves =  _moveStrategy.GetPossibleMoves(Vector3Int.CeilToInt(_pawn.transform.position));
        return _possibleMoves;
    }

    public void Perform(Vector2Int point)
    {
        if (_possibleMoves.Contains(point))
        {
            _gridManager.MovePawnTo(_pawn, point);
            _pawn.PawnStats.UseAction();
        } else
        {
            throw new Exception("Tried to get point out of PossibleMoves");
        }
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }

    public void SelfRealize()
    {
        throw new NotImplementedException();
    }
}
