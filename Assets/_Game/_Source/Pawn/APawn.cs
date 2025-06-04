using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class APawn : MonoBehaviour, IHintUser
{
    protected StatConfigSO _HP;
    protected StatConfigSO _AP;
    protected StatConfigSO _STR;
    protected StatConfigSO _ARM;

    protected List<APawnAction> _actions;

    public Vector2Int GridPosition { get; protected set; }
    public IPawnStats PawnStats { get; protected set; }
    public PawnTeam PawnTeam { get; protected set; }

    public virtual void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
    {
        _HP = Resources.Load<StatConfigSO>("Stats/HP");
        _AP = Resources.Load<StatConfigSO>("Stats/AP");
        _STR = Resources.Load<StatConfigSO>("Stats/STR");
        _ARM = Resources.Load<StatConfigSO>("Stats/ARM");

        Debug.Log($"Инициализация конфигов: {_HP != null} {_AP != null} {_STR != null} {_ARM != null}");

        PawnStats = new PawnStats(_HP, hpLvl, _AP, apLvl, _STR, strLvl, _ARM, armLvl);

        //initialize actions
        //set pawn team
    }

    public void UpdateGridPosition(Vector2Int newPosition)
    {
        GridManager.Instance.GetTileAtGridPosition(GridPosition)?.RemovePawn();
        GridPosition = newPosition;

        GridManager.Instance.GetTileAtGridPosition(newPosition).SetNewPawn(this);
    }

    public void SetGridPosition(Vector2Int newPosition)
    {
        GridPosition = newPosition;
    }

    public abstract string GetHintText();
}
