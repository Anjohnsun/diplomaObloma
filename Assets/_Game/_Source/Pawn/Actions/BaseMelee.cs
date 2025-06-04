using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseMelee : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/BaseMelee");
    public override MarkerType Marker => MarkerType.attack;

    public BaseMelee(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = "јтака противника, сто€щего на соседней клетке";
    }

    public override List<FieldTile> CalculateTargets()
    {
        _possibleMoves = GetPossibleTargets(_owner.GridPosition);
        return _possibleMoves;
    }

    public override void CanPerform(Vector2Int tile)
    {
        throw new NotImplementedException();
    }

    public override bool SelfPerform(Action handler)
    {
        Vector2Int playerPos = LevelManager.Instance.PlayerPawn.GridPosition;
        Vector2Int currentPos = _owner.GridPosition;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int checkPos = currentPos + new Vector2Int(x, y);
                if (checkPos == playerPos)
                {
                    FieldTile targetTile = GridManager.Instance.GetTileAtGridPosition(checkPos);
                    if (targetTile != null && targetTile.Pawn == LevelManager.Instance.PlayerPawn)
                    {
                        _owner.PawnStats.UseAP();

                        Sequence attackSequence = DOTween.Sequence();
                        attackSequence.Append(
                            targetTile.Pawn.transform.DOMoveY(targetTile.Pawn.transform.position.y + 0.3f, _duration / 2)
                        );
                        attackSequence.Append(
                            targetTile.Pawn.transform.DOMoveY(targetTile.Pawn.transform.position.y, _duration / 2)
                        );
                        attackSequence.OnComplete(() =>
                        {
                            targetTile.Pawn.PawnStats.TakeDamage(_owner.PawnStats.STR);
                            handler?.Invoke();
                        });

                        return true;
                    }
                }
            }
        }

        handler?.Invoke();
        return false;
    }

    public override void Perform(FieldTile tile, Action handler)
    {
        if (tile != null && _possibleMoves.Contains(tile))
        {
            Debug.Log("Perform BaseMelee");
            base.Perform(tile, handler);

            _owner.PawnStats.UseAP();

            tile.Pawn.transform.DOMoveY(tile.Pawn.transform.position.y + 0.3f, _duration / 2)
                .OnComplete(() => tile.Pawn.transform.DOMoveY(tile.Pawn.transform.position.y - 0.3f, _duration / 2).OnComplete(() =>
                {
                    tile.Pawn.PawnStats.TakeDamage(_owner.PawnStats.STR*2);
                    handler.Invoke();
                }));
        }
        else
        {
            handler();
        }
    }

    public List<FieldTile> GetPossibleTargets(Vector2Int currentPosition)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                possibleMoves.Add(new Vector2Int(currentPosition.x + x, currentPosition.y + y));
            }
        }

        return GridManager.Instance.GetAvailableTargets(possibleMoves, GridManager.HasAnyPawn);
    }
}