using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<AEnemyPawn> _pawns = new List<AEnemyPawn>();
    private MonoBehaviour _coroutines;
    private PlayerPawn _player;
    private Queue<AEnemyPawn> _activeEnemies;

    public int EnemyCount => _pawns.Count;

    public EnemyManager(MonoBehaviour coroutineRunner, PlayerPawn player)
    {
        _coroutines = coroutineRunner;
        _player = player;
    }

    public void InitPawns(List<AEnemyPawn> pawns)
    {
        ClearSubscriptions();
        _pawns = pawns;

        foreach (AEnemyPawn pawn in _pawns)
        {
            if (pawn != null && pawn.PawnStats != null)
            {
                AEnemyPawn currentPawn = pawn;
                currentPawn.PawnStats.OnDeath += () => RemovePawn(currentPawn);
            }
        }
    }

    public void ClearSubscriptions()
    {
        foreach (AEnemyPawn pawn in _pawns)
        {
            if (pawn != null && pawn.PawnStats != null)
            {
                pawn.PawnStats.OnDeath -= () => RemovePawn(pawn);
            }
        }
        _pawns.Clear();
    }

    public void AddPawn(AEnemyPawn pawn)
    {
        if (pawn != null)
        {
            _pawns.Add(pawn);
            AEnemyPawn currentPawn = pawn;
            currentPawn.PawnStats.OnDeath += () => RemovePawn(currentPawn);
        }
    }

    public void RemovePawn(AEnemyPawn pawn)
    {
        if (pawn == null) return;

        if (_pawns.Remove(pawn))
        {
            _player.PawnStats.GetEXP(pawn.GivesEXP);

            if (EnemyCount <= 0)
            {
                StateManager.Instance.ChangeState<UpgradeState>();
            }
        }
    }

    public void StartEnemyTurn()
    {
        _coroutines.StopAllCoroutines();
        _coroutines.StartCoroutine(EnemyTurnCor());
    }

    private IEnumerator EnemyTurnCor()
    {
        if (EnemyCount <= 0)
        {
            StateManager.Instance.ChangeState<UpgradeState>();
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        _activeEnemies = new Queue<AEnemyPawn>(_pawns);

        if (_activeEnemies.Count == 0)
        {
            EndEnemyTurn();
            yield break;
        }

        PerformNextEnemy();
    }

    private void PerformNextEnemy()
    {
        if (_activeEnemies.Count == 0)
        {
            _coroutines.StartCoroutine(FinishEnemyTurn());
            return;
        }

        var currentEnemy = _activeEnemies.Dequeue();
        currentEnemy.PerformActions(OnEnemyActionComplete);
    }

    private void OnEnemyActionComplete()
    {
        Debug.Log("Конец хода вражеской пешки");
        _coroutines.StartCoroutine(ContinueEnemyTurn());
    }

    private IEnumerator ContinueEnemyTurn()
    {
        yield return new WaitForSeconds(0.3f);
        PerformNextEnemy();
    }

    private IEnumerator FinishEnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);
        EndEnemyTurn();
    }

    private void EndEnemyTurn()
    {
        StateManager.Instance.ChangeState<PlayerTurnState>();
    }
}