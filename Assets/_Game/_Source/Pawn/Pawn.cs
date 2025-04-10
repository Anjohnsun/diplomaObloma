using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pawn : MonoBehaviour
{
    private IPawnStats _pawnStats;
    private GridManager _gridManager;

    private Dictionary<Type, IPawnAction> _actions;

    public GridManager GridManager => _gridManager;
    public IPawnStats PawnStats => _pawnStats;
    public Dictionary<Type, IPawnAction> Actions => _actions;

    //добавление спрайта в ui
    //public Action OnActionListChanged;

    public void Construct(PawnStatsSO pawnStats, GridManager gridManager)
    {
        _pawnStats = new PawnStats(pawnStats.MaxHealth, pawnStats.CurrentHealth, pawnStats.MaxActionPoints, pawnStats.ActionPointsLeft);
        _gridManager = gridManager;

        _actions = new Dictionary<Type, IPawnAction>();

        Debug.Log($"Pawn created. MaxHP: {_pawnStats.MaxHealth}, MaxActions: {_pawnStats.MaxActionPoints}");
    }

    public void AddAction(IPawnAction newAction)
    {
        _actions.Add(newAction.GetType(), newAction);
      //  OnActionListChanged.Invoke();
    }

    public void AddEffect(PawnStatsDecorator newEffect)
    {
        newEffect.Construct(_pawnStats);
        _pawnStats = newEffect;
    }

    public void HandleStartOfTurn()
    {
        _pawnStats.StartTurnUpdate();
    }
}
