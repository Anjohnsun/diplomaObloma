using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurn : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/SkipTurn");

    public override MarkerType Marker => MarkerType.interact;

    public SkipTurn(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = $"Пропуск хода";
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
        if (tile == null || !_possibleMoves.Contains(tile))
        {
            handler?.Invoke();
            return;
        }

        _owner.PawnStats.UseAP(100);

        DOTween.Sequence()
            .Append(_owner.transform.DORotate(new Vector3(0, 0, -15), _duration))
            .Append(_owner.transform.DORotate(new Vector3(0, 0, 15), _duration))
            .Append(_owner.transform.DORotate(new Vector3(0, 0, -0), _duration))
            .OnComplete(() => handler?.Invoke())
            .Play();
    }

    public override bool SelfPerform(Action handler)
    {

        _owner.PawnStats.UseAP(100);

        DOTween.Sequence()
            .Append(_owner.transform.DORotate(new Vector3(0, 0, -15), _duration))
            .Append(_owner.transform.DORotate(new Vector3(0, 0, 15), _duration))
            .Append(_owner.transform.DORotate(new Vector3(0, 0, -0), _duration))
            .OnComplete(() => handler?.Invoke())
            .Play();

        return true;
    }

    public List<FieldTile> GetPossibleMoves(Vector2Int currentPosition)
    {
        List<FieldTile> moves = new List<FieldTile>();
        moves.Add(GridManager.Instance.GetTileAtGridPosition(currentPosition));

        return moves;
    }
}
