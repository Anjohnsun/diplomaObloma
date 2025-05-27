using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : IPawnAction
{
    public Pawn Pawn { get; }
    public float Duration => 0.5f;
    private int _healAmount = 20;

    public HealAction(Pawn pawn, int healAmount)
    {
        Pawn = pawn;
        _healAmount = healAmount;
    }

    public List<FieldTile> CalculateTargets()
    {
        var currentTile = GridManager.Instance.GetTileAtGridPosition(Pawn.GridPosition);
        return new List<FieldTile> { currentTile };
    }

    public void Perform(Vector2 targetWorldPosition, Action handler)
    {
        var targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);
        var currentTile = GridManager.Instance.GetTileAtGridPosition(Pawn.GridPosition);

        if (targetTile == currentTile)
        {
            Pawn.PawnStats.Heal(_healAmount);
            Pawn.PawnStats.UseAP();

            Pawn.transform.DOPunchScale(Vector3.one * 0.2f, Duration)
                .OnComplete(() => handler());
        }
        else
        {
            handler();
        }
    }

    public bool CanPerform(Vector2 targetWorldPosition)
    {
        var targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);
        var currentTile = GridManager.Instance.GetTileAtGridPosition(Pawn.GridPosition);
        return targetTile == currentTile;
    }

    public bool CanPerformAuto()
    {
        return Pawn.PawnStats.CurrentHP < Pawn.PawnStats.MaxHP;
    }

    public void Cancel() { }
    public void SelfRealize(Action handler)
    {
        Perform(Pawn.transform.position, handler);
    }
}