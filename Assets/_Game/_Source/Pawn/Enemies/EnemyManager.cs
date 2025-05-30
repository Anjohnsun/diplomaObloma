using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<Pawn> _pawns = new List<Pawn>();
    private MonoBehaviour _coroutines;

    public int EnemyCount => _pawns.Count;

    public EnemyManager(MonoBehaviour coroutineRunner)
    {
        _coroutines = coroutineRunner;
    }
    public void InitPawns(List<Pawn> pawns)
    {
        _pawns = pawns;
        foreach (var pawn in _pawns)
        {
            pawn.PawnStats.OnDeath += () => RemovePawn(pawn);
        }
    }
    public void AddPawn(Pawn pawn) => _pawns.Add(pawn);
    public void RemovePawn(Pawn pawn) => _pawns.Remove(pawn);
    public void StartEnemyTurn() => _coroutines.StartCoroutine(EnemyTurnSequence());
    private IEnumerator EnemyTurnSequence()
    {
        Debug.Log($"-> EnemyTurn");
        if (EnemyCount <= 0)
        {
            StateManager.Instance.ChangeState<UpgradeState>();
            yield break;
        }

        Debug.Log($"EnemyManager: enemy count: {_pawns.Count}");
        yield return new WaitForSeconds(0.5f);

        if (_pawns.Count == 0)
        {
            EndEnemyTurn();
            yield break;
        }

        AEnemyPawn pawn;
        foreach (var enemy in _pawns)
        {
            if (enemy == null || enemy.PawnStats.CurrentAP <= 0) continue;

            pawn = (AEnemyPawn)enemy;
            pawn.PerformActions();
        }

        yield return new WaitForSeconds(0.5f);
        EndEnemyTurn();
    }


    private void EndEnemyTurn()
    {
        StateManager.Instance.ChangeState<PlayerTurnState>();
    }
}