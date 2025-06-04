using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class InstantHealing : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/InstantHealing");

    public override MarkerType Marker => MarkerType.interact;

    public InstantHealing(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = $"ћоментальное востановление здоровь€";
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
        Debug.Log("Perform InstantHealing");
        if (tile != null && _possibleMoves.Contains(tile))
        {
            tile.Pawn.PawnStats.Heal(3);
            _owner.PawnStats.UseAP();

            _owner.transform.DOScale(new Vector2(1.3f, 1.3f), _duration)
                .OnComplete(() =>
                _owner.transform.DOScale(new Vector2(1f, 1f), _duration)
                .OnComplete(() =>
                {
                    handler();
                    return;
                }));
        }
        else
        {
            handler();
        }
    }

    public override bool SelfPerform(Action handler)
    {
        throw new NotImplementedException();
    }

    public List<FieldTile> GetPossibleMoves(Vector2Int currentPosition)
    {
        List<FieldTile> moves = new List<FieldTile>();
        moves.Add(GridManager.Instance.GetTileAtGridPosition(currentPosition));

        return moves;
    }
}
