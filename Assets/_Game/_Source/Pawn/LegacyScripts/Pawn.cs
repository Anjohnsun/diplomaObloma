using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class Pawn : MonoBehaviour, IHintUser 
{
    [SerializeField] protected StatConfigSO _HP;
    [SerializeField] protected StatConfigSO _AP;
    [SerializeField] protected StatConfigSO _STR;
    [SerializeField] protected StatConfigSO _ARM;

    [SerializeField] public PawnTeam PawnTeam { get; protected set; }

    protected IPawnStats _pawnStats;

    public IPawnAction MoveAction;
    public IPawnAction AttackAction;
    protected Dictionary<Type, IPawnAction> _bonusActions;

    public Vector2Int GridPosition { get; set; }

    public IPawnStats PawnStats => _pawnStats;
    public IReadOnlyDictionary<Type, IPawnAction> BonusActions => _bonusActions;

    public virtual void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
    {
        _pawnStats = new PawnStats(_HP, hpLvl, _AP, apLvl, _STR, strLvl, _ARM, armLvl);
        _bonusActions = new Dictionary<Type, IPawnAction>();
        PawnTeam = PawnTeam.Player;
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
    public virtual string GetHintText()
    {
        return $"Будущее этой пешки ещё не определено";
    }
}
public enum PawnTeam
{
    Player,
    Enemy
}
