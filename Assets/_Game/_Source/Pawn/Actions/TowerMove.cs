using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerMove : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/TowerMove");
    public override MarkerType Marker => MarkerType.interact;

    private AEnemyPawn _targetPawn;
    private Vector2Int _targetDirection;

    public TowerMove(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = "Толкнуть врага в линии, нанося урон";
    }

    public override List<FieldTile> CalculateTargets()
    {
        _possibleMoves = GetPossibleTargets(_owner.GridPosition);
        return _possibleMoves;
    }

    public override void Perform(FieldTile tile, Action handler)
    {
        base.Perform(tile, handler);

        if (tile == null || !_possibleMoves.Contains(tile) || tile.Pawn == null)
        {
            handler?.Invoke();
            return;
        }

        _owner.PawnStats.UseAP();
        _targetPawn = tile.Pawn as AEnemyPawn;
        _targetDirection = GetPushDirection(_owner.GridPosition, _targetPawn.GridPosition);

        Vector2Int intermediatePos = _targetPawn.GridPosition - _targetDirection;
        FieldTile intermediateTile = GridManager.Instance.GetTileAtGridPosition(intermediatePos);

        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(_owner.transform.DOMove(intermediateTile.transform.position, 0.3f).SetEase(Ease.OutQuad));

        Vector2Int pushTargetPos = _targetPawn.GridPosition + _targetDirection;
        FieldTile pushTargetTile = GridManager.Instance.GetTileAtGridPosition(pushTargetPos);

        if (pushTargetTile != null && pushTargetTile.Pawn == null)
        {
            moveSequence.Join(_targetPawn.transform.DOMove(pushTargetTile.transform.position, 0.3f).SetEase(Ease.OutBack));
            moveSequence.AppendCallback(() =>
            {
                UpdatePawnPosition(_owner, intermediateTile);
                UpdatePawnPosition(_targetPawn, pushTargetTile);
            });
        }
        else
        {
            moveSequence.AppendCallback(() =>
            {
                UpdatePawnPosition(_owner, intermediateTile);
            });
            moveSequence.Join(_targetPawn.transform.DOJump(_targetPawn.transform.position, 0.2f, 1, 0.3f));
        }

        moveSequence.AppendCallback(() =>
        {
            _targetPawn.PawnStats.TakeDamage(_owner.PawnStats.STR * 2);
            handler?.Invoke();
        });
    }

    private void UpdatePawnPosition(APawn pawn, FieldTile targetTile)
    {
        GridManager.Instance.GetTileAtGridPosition(pawn.GridPosition).RemovePawn();
        pawn.UpdateGridPosition(GridManager.Instance.GetTileCoordinates(targetTile));
        targetTile.SetNewPawn(pawn);
    }

    private Vector2Int GetPushDirection(Vector2Int from, Vector2Int to)
    {
        if (from.x == to.x)
            return new Vector2Int(0, from.y < to.y ? 1 : -1);
        else
            return new Vector2Int(from.x < to.x ? 1 : -1, 0);
    }

    public override void CanPerform(Vector2Int tile)
    {
        throw new NotImplementedException();
    }

    public override bool SelfPerform(Action handler)
    {
        throw new NotImplementedException();
    }

    public List<FieldTile> GetPossibleTargets(Vector2Int currentPosition)
    {
        List<FieldTile> possibleTargets = new List<FieldTile>();
        int gridSize = GridManager.Instance.VerticalSize;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        foreach (var dir in directions)
        {
            Vector2Int checkPos = currentPosition + dir;
            while (checkPos.x >= 0 && checkPos.x < gridSize && checkPos.y >= 0 && checkPos.y < gridSize)
            {
                FieldTile tile = GridManager.Instance.GetTileAtGridPosition(checkPos);
                if (tile != null && tile.Pawn != null)
                {
                    possibleTargets.Add(tile);
                    break;
                }
                checkPos += dir;
            }
        }

        return possibleTargets;
    }
}