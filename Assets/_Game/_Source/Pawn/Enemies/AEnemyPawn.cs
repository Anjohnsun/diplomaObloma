using System;
using UnityEngine;

public abstract class AEnemyPawn : Pawn
{
    [SerializeField] protected int _givesExp;

    public Action _OnPawnDie;

    public override void Construct()
    {
        base.Construct();
        OnTurnBegin += ChooseAndPerformAction;
        _pawnStats.OnGetDamage += HandleDamage;
    }

    protected virtual void ChooseAndPerformAction()
    {

    }

    protected virtual void HandleDamage(int hp)
    {
        Debug.Log("Pawn: damage handled");
    }
}
