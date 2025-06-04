using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class XMoveActionStayFar : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/XMove");
    public override MarkerType Marker => MarkerType.interact;

    public XMoveActionStayFar(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = "Перемещение в направлении от игрока";
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
        Debug.Log("Perform XMove (Stay Far)");
        if (tile != null && _possibleMoves.Contains(tile))
        {
            _owner.UpdateGridPosition(GridManager.Instance.GetTileCoordinates(tile));
            _owner.PawnStats.UseAP();

            _owner.transform.DOMove(tile.transform.position, _duration)
                .OnComplete(() => handler?.Invoke());
        }
        else
        {
            handler?.Invoke();
        }
    }

    public override bool SelfPerform(Action handler)
    {
        var possibleMoves = GetPossibleMoves(_owner.GridPosition);

        if (possibleMoves.Count == 0)
        {
            handler?.Invoke();
            return false;
        }

        Vector2Int playerPos = LevelManager.Instance.PlayerPawn.GridPosition;
        Vector2Int currentPos = _owner.GridPosition;

        FieldTile bestMove = null;
        float maxDistance = -1f;

        foreach (var move in possibleMoves)
        {
            Vector2Int movePos = GridManager.Instance.GetTileCoordinates(move);
            float distance = Vector2Int.Distance(movePos, playerPos);

            if (distance > maxDistance ||
                (distance == maxDistance && UnityEngine.Random.value > 0.5f))
            {
                maxDistance = distance;
                bestMove = move;
            }
        }

        if (bestMove != null)
        {
            _owner.UpdateGridPosition(GridManager.Instance.GetTileCoordinates(bestMove));
            _owner.PawnStats.UseAP();

            _owner.transform.DOMove(bestMove.transform.position, _duration)
                .OnComplete(() => handler?.Invoke());
            return true;
        }

        handler?.Invoke();
        return false;
    }

    public List<FieldTile> GetPossibleMoves(Vector2Int currentPosition)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>
        {
            new Vector2Int(currentPosition.x + 1, currentPosition.y),
            new Vector2Int(currentPosition.x - 1, currentPosition.y),
            new Vector2Int(currentPosition.x, currentPosition.y + 1),
            new Vector2Int(currentPosition.x, currentPosition.y - 1)
        };

        return GridManager.Instance.GetAvailableTargets(possibleMoves, GridManager.IsFreeTile);
    }
}