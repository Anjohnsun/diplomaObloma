using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager
{
    private List<Pawn> _pawns;

    private PlayerTurnState _playerTurnState;
    private MonoBehaviour _coroutines;
    private StateManager _stateManager;

    public EnemyManager(PlayerTurnState playerTurnState, MonoBehaviour coroutines, StateManager stateManager)
    {
        _playerTurnState = playerTurnState;
        _coroutines = coroutines;
        _stateManager = stateManager;
    }

    public void AddPawn(Pawn pawn)
    {
        _pawns.Add(pawn);
    }
    public void HandleEnemyTurn()
    {
        _coroutines.StartCoroutine(HandleEnemyTurnCor());
    }

    private IEnumerator HandleEnemyTurnCor()
    {
        if (_pawns.Count == 0)
        {
            yield return new WaitForSeconds(0.7f);
            _stateManager.ChangeState(_playerTurnState);
        }
        else
        {
            foreach (Pawn pawn in _pawns)
            {
                IPawnAction action = pawn.Actions.ToList()[Random.Range(0, pawn.Actions.Count)].Value;

                Debug.Log($"Pawn {pawn.ToString()} is performing action {action.GetType()}");
                action.SelfRealize();
            }

            yield return new WaitForSeconds(2f);
            _stateManager.ChangeState(_playerTurnState);
        }
    }

    public void RemovePawn(Pawn pawn)
    {
        _pawns.Remove(pawn);
    }
}
