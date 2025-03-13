using UnityEngine;

public class MoveAction : IPawnAction
{
    private Pawn _pawn;
    private IMoveStrategy _moveStrategy;

    public MoveAction(Pawn pawn, IMoveStrategy moveStrategy)
    {
        _pawn = pawn;
        _moveStrategy = moveStrategy;
    }

    public void DoAction()
    {
        _moveStrategy.GetPossibleMoves(Vector3Int.CeilToInt(_pawn.transform.position));
    }
}
