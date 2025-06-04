using DG.Tweening;
using System;
using UnityEngine;

public abstract class AEnemyPawn : APawn
{
    [SerializeField] protected int _givesExp;
    public int GivesEXP => _givesExp;

    public override void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
    {
        base.Construct(hpLvl, apLvl, strLvl, armLvl);
        PawnStats.OnDamageTaken += HandleDamage;
        PawnStats.OnDeath += HandleDeath;
        PawnTeam = PawnTeam.Enemy;
    }

    public abstract void Construct(int currentLevel);

    public virtual void PerformActions(Action handler)
    {
        PawnStats.StartTurn();
    }

    protected virtual void HandleDamage(int hpLeft)
    {
        Debug.Log($"Pawn: damage handled, hp left {hpLeft}");
    }

    protected virtual void HandleDeath()
    {
        Debug.Log("Pawn died");
        transform.DOScale(Vector3.zero, 0.3f);
        GridManager.Instance.GetTileAtGridPosition(GridPosition).RemovePawn();
        Destroy(gameObject);
    }
}
