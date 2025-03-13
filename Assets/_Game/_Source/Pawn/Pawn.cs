using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pawn : MonoBehaviour
{
    private PawnStats _pawnStats;
    private  GridManager _gridManager;

    private List<IPawnAction> _actions;

    public GridManager GridManager => _gridManager;

    [Inject]
    public void Construct(PawnStats pawnStats, GridManager gridManager)
    {
        _pawnStats = pawnStats;
        _gridManager = gridManager;
        Debug.Log($"Pawn created. MaxHP: {_pawnStats.MaxHealth}, MaxActions: {_pawnStats.MaxActionPoints}");
    }

    public void AddPossibleAction(IPawnAction newAction)
    {
        _actions.Add(newAction);
    }
}
