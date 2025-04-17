using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager
{
    private StateManager _stateManager;
    public TurnManager(StateManager stateManager)
    {
        _stateManager = stateManager;
    }

    private readonly List<Pawn> _pawns = new();
    private int _currentPawnIndex = -1;

    public void Initialize(List<Pawn> initialPawns)
    {
        _pawns.Clear();
        foreach (var pawn in initialPawns)
        {
            _pawns.Add(pawn);
            pawn.OnTurnOver += HandlePawnTurnOver;
        }

        ReorderQueue();

        _currentPawnIndex = -1;
        StartNextTurn();
    }


    public void RemovePawn(Pawn pawn)
    {
        if (!_pawns.Contains(pawn)) return;

        if (_pawns[_currentPawnIndex] == pawn) return;

        int removedIndex = _pawns.IndexOf(pawn);
        if (_pawns.Remove(pawn))
        {
            pawn.OnTurnOver -= HandlePawnTurnOver;

            if (removedIndex < _currentPawnIndex)
            {
                _currentPawnIndex--;
            }

            ReorderQueue();

            if (_pawns.Count == 1 && _pawns[0].PawnTeam == PawnTeam.Player)
            {
                _stateManager.ChangeState<UpgradeState>();
            }
        }
    }

    private void ReorderQueue()
    {
        _pawns.Sort((a, b) =>
        {
            int spdComparison = b.PawnStats.SPD_VALUE.CompareTo(a.PawnStats.SPD_VALUE);
            return spdComparison != 0 ? spdComparison : UnityEngine.Random.Range(-1, 2);
        });

        _currentPawnIndex = Mathf.Clamp(_currentPawnIndex, -1, Mathf.Max(0, _pawns.Count - 1));
    }

    private void StartNextTurn()
    {
        if (_pawns.Count == 0) return;

        _currentPawnIndex = (_currentPawnIndex + 1) % _pawns.Count;

        Debug.Log("Starting new turn. Index : " + _currentPawnIndex);
        var currentPawn = _pawns[_currentPawnIndex];
        currentPawn.OnTurnBegin?.Invoke();
    }

    private void HandlePawnTurnOver()
    {
        if (_pawns.Count == 1 && _pawns[0].PawnTeam == PawnTeam.Player)
        {
            StartNextTurn();
            _stateManager.ChangeState<UpgradeState>();
        }
        else
        {
            StartNextTurn();
        }
    }
}