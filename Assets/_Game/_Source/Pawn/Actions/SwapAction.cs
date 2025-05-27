using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SwapAction : IPawnAction
{
    private List<FieldTile> _possibleTargets;
    public Pawn Pawn { get; }
    public float Duration => 1f;

    public SwapAction(Pawn pawn)
    {
        Pawn = pawn;
    }

    public List<FieldTile> CalculateTargets()
    {
        _possibleTargets = new List<FieldTile>();

        var allTiles = GridManager.Instance.GetAvailableTargets(GridManager.HasAnyPawn);
        foreach (var tile in allTiles)
        {
            if (tile.Pawn != null && tile.Pawn.PawnTeam != Pawn.PawnTeam)
            {
                _possibleTargets.Add(tile);
            }
        }

        return _possibleTargets;
    }

    public void Perform(Vector2 targetWorldPosition, Action handler)
    {
        var targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);

        if (targetTile != null && _possibleTargets.Contains(targetTile) && targetTile.Pawn != null)
        {
            // Меняемся местами с противником
            var enemyPawn = targetTile.Pawn;
            var currentTile = GridManager.Instance.GetTileAtGridPosition(Pawn.GridPosition);

            GridManager.Instance.MovePawn(Pawn, targetTile);
            GridManager.Instance.MovePawn(enemyPawn, currentTile);

            Pawn.PawnStats.UseAP();

            // Анимация перемещения
            Sequence swapSequence = DOTween.Sequence();
            swapSequence.Join(Pawn.transform.DOMove(targetTile.transform.position, Duration));
            swapSequence.Join(enemyPawn.transform.DOMove(currentTile.transform.position, Duration));
            swapSequence.OnComplete(() => handler());
        }
        else
        {
            handler();
        }
    }

    public bool CanPerform(Vector2 targetWorldPosition)
    {
        FieldTile targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);
        return targetTile != null && _possibleTargets.Contains(targetTile);
    }

    public bool CanPerformAuto()
    {
        return CalculateTargets().Count > 0;
    }

    public void Cancel() { }
    public void SelfRealize(Action handler)
    {
    }
}