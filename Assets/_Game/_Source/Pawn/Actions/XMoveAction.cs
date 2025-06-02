using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class XMoveAction : APawnAction
{
    private List<FieldTile> _possibleMoves;

    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/XMove");

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

    public override bool SelfPerform()
    {
        throw new NotImplementedException();
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
