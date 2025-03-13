using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAction : APlayerPawnAction
{
    private const PlayerPawnActions _actionType = PlayerPawnActions.move;

    private Pawn _pawn;
    private IMoveStrategy _moveStrategy;
    private PlayerPawnHandler _playerHandler;
    private GridManager _gridManager;

    List<Vector2Int> _possibleMoves;
    public PlayerPawnActions ActionType => _actionType;

    public PlayerMoveAction(Pawn pawn, IMoveStrategy moveStrategy, PlayerPawnHandler playerHandler, GridManager gridManager)
    {
        _pawn = pawn;
        _moveStrategy = moveStrategy;
        _playerHandler = playerHandler;

        _playerHandler.PerformOnPlayerTurnStart(HighlightPossibleMoves);
    }

    private void HighlightPossibleMoves()
    {
        _possibleMoves = _moveStrategy.GetPossibleMoves(Vector3Int.CeilToInt(_pawn.transform.position));

        //highlight tiles
        foreach (Vector2Int point in _possibleMoves)
        {
            _gridManager.GetTileAt(point).HighlightAsFree(true);
        }
    }

    public override void DoAction()
    {
        _gridManager.MovePawnTo(_pawn, _playerHandler.ClickPoint);

        //dehighlight tiles
        foreach (Vector2Int point in _possibleMoves)
        {
            _gridManager.GetTileAt(point).HighlightAsFree(true);
        }
    }
}
