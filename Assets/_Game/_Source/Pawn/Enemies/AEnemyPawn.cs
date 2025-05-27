using System;
using UnityEngine;

public abstract class AEnemyPawn : Pawn
{
    [SerializeField] protected int _givesExp;

    public Action _OnPawnDie;

    public override void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
    {
        base.Construct(hpLvl, apLvl, strLvl, armLvl);
        _pawnStats.OnDamageTaken += HandleDamage;
        _pawnStats.OnDeath += HandleDeath;
        PawnTeam = PawnTeam.Enemy;
    }

    public virtual void PerformActions()
    {
        _pawnStats.StartNewTurn();
    }

    protected virtual void HandleDamage(int hpLeft)
    {
        Debug.Log($"Pawn: damage handled, hp left {hpLeft}");
    }

    protected virtual void HandleDeath()
    {
        Debug.Log("Pawn died");
        Destroy(gameObject);
    }
}
