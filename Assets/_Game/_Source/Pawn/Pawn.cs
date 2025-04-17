using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class Pawn : MonoBehaviour
{
    [SerializeField] protected StatConfigSO _HP;
    [SerializeField] protected StatConfigSO _AP;
    [SerializeField] protected StatConfigSO _STR;
    [SerializeField] protected StatConfigSO _SPD;
    [SerializeField] protected StatConfigSO _ARM;
    [SerializeField] public readonly PawnTeam PawnTeam;

    protected IPawnStats _pawnStats;

    public IPawnAction _moveAction;
    public IPawnAction _attackAction;
    protected Dictionary<Type, IPawnAction> _bonusActions = new Dictionary<Type, IPawnAction>();

    public Action OnTurnBegin;
    public Action OnTurnOver;
    public Vector2Int GridPosition { get; protected set; }

    public IPawnStats PawnStats => _pawnStats;
    public IReadOnlyDictionary<Type, IPawnAction> BonusActions => _bonusActions;

    public virtual void Construct()
    {
        _pawnStats = new PawnStats(_HP, 0, _AP, 3, _STR, 0, _SPD, 0, _ARM, 0);

        OnTurnBegin += HandleStartOfTurn;
        OnTurnOver += HandleEndOfTurn;
    }

    public void SetMoveAction(IPawnAction moveAction)
    {
        _moveAction = moveAction;
    }

    public void SetAttackAction(IPawnAction attackAction)
    {
        _attackAction = attackAction;
    }

    public void AddBonusAction(IPawnAction action)
    {
        _bonusActions.Add(action.GetType(), action);
    }

    public bool TryGetBonusAction<T>(out T action) where T : IPawnAction
    {
        if (_bonusActions.TryGetValue(typeof(T), out var foundAction))
        {
            action = (T)foundAction;
            return true;
        }
        action = default;
        return false;
    }

    public void RemoveBonusAction<T>() where T : IPawnAction
    {
        _bonusActions.Remove(typeof(T));
    }

    public void ClearBonusActions()
    {
        _bonusActions.Clear();
    }

    public void HandleStartOfTurn()
    {
        _pawnStats.StartTurnUpdate();
    }

    public void HandleEndOfTurn()
    {
    }

    public void UpdatePosition(Vector2Int position)
    {
        GridPosition = position;
    }
}


public enum PawnTeam
{
    Player,
    Enemy
}
