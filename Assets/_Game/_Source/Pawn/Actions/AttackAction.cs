using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : IPawnAction
{
    private AWeapon _weapon;
    private List<FieldTile> _possibleTargets;

    public Pawn Pawn { get; }
    public float Duration => 0.8f;

    public AttackAction(Pawn pawn, AWeapon weapon)
    {
        Pawn = pawn;
        _weapon = weapon;
    }

    public List<FieldTile> CalculateTargets()
    {
        _possibleTargets = new List<FieldTile>();
        var attackArea = _weapon.GetAttackArea(Pawn.GridPosition);

        foreach (var pos in attackArea)
        {
            var tile = GridManager.Instance.GetTileAtGridPosition(pos);
            if (tile?.Pawn != null && tile.Pawn.PawnTeam != Pawn.PawnTeam)
            {
                _possibleTargets.Add(tile);
            }
        }

        return _possibleTargets;
    }

    public void Perform(Vector2 targetWorldPosition, Action handler)
    {
        var targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);

        if (targetTile != null && _possibleTargets.Contains(targetTile))
        {
            Debug.Log("ATTACK");
            int damage = _weapon.CalculateDamage();
            targetTile.Pawn.PawnStats.TakeDamage(damage);

            Pawn.PawnStats.UseAP();

            Pawn.transform.DOShakePosition(Duration, 0.3f)
                .OnComplete(() => handler());
        }
        else
        {
            handler();
        }
    }

    public void Cancel() { /*...*/ }
    public void SelfRealize(Action handler) { /*...*/ }

    public bool CanPerform(Vector2 targetWorldPosition)
    {
        FieldTile targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);
        return targetTile != null && _possibleTargets.Contains(targetTile);
    }

    public bool CanPerformAuto()
    {
        if (CalculateTargets().Count > 0)
            return true;
        return false;
    }
}
