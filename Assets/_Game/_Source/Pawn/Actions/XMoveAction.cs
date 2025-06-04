using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class XMoveAction : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/XMove");

    public override MarkerType Marker => MarkerType.interact;

    public XMoveAction(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = $"Простое перемещение в четырёх направлениях.";
    }

    public override List<FieldTile> CalculateTargets()
    {
        _possibleMoves = GetPossibleMoves(_owner.GridPosition);
        return _possibleMoves;
    }

    public override void CanPerform(Vector2Int tile)
    {
        throw new NotImplementedException();
    }

    public override void Perform(FieldTile tile, Action handler)
    {
        Debug.Log("Perform XMove");
        if (tile != null && _possibleMoves.Contains(tile))
        {
            _owner.UpdateGridPosition(GridManager.Instance.GetTileCoordinates(tile));
            _owner.PawnStats.UseAP();

            _owner.transform.DOMove(tile.transform.position, _duration)
                .OnComplete(() =>
                {
                    handler();
                    return;
                });
        }
        else
        {
            handler();
        }
    }

    public override bool SelfPerform(Action handler)
    {
        var possibleMoves = GetPossibleMoves(_owner.GridPosition);

        if (possibleMoves.Count == 0)
            return false;

        Vector2Int playerPos = LevelManager.Instance.PlayerPawn.GridPosition;
        Vector2Int currentPos = _owner.GridPosition;

        FieldTile bestMove = null;
        float minDistance = float.MaxValue;

        foreach (var move in possibleMoves)
        {
            float distance = Vector2Int.Distance(GridManager.Instance.GetTileCoordinates(move), playerPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                bestMove = move;
            }
        }

        if (bestMove != null)
        {
            _owner.UpdateGridPosition(GridManager.Instance.GetTileCoordinates(bestMove));
            _owner.PawnStats.UseAP();

            _owner.transform.DOMove(bestMove.transform.position, _duration);
            return true;
        }

        return false;
    }

    public List<FieldTile> GetPossibleMoves(Vector2Int currentPosition)
    {
        List<FieldTile> moves = new List<FieldTile>();

        Vector2Int[] possibleMoves = new Vector2Int[]
        {
            new Vector2Int(currentPosition.x + 1, currentPosition.y),
            new Vector2Int(currentPosition.x - 1, currentPosition.y),
            new Vector2Int(currentPosition.x, currentPosition.y + 1),
            new Vector2Int(currentPosition.x, currentPosition.y - 1),
        };

        moves = GridManager.Instance.GetAvailableTargets(new List<Vector2Int>(possibleMoves),
            GridManager.IsFreeTile);

        return moves;
    }
}
