using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<Pawn> _pawns = new List<Pawn>();
    private MonoBehaviour _coroutines;


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
        Debug.Log($"EnemyManager: enemy count: {_pawns.Count}");
        yield return new WaitForSeconds(0.5f);

        if (_pawns.Count == 0)
        {
            EndEnemyTurn();
            yield break;
        }

        foreach (var enemy in _pawns.ToArray())
        {
            if (enemy == null || enemy.PawnStats.CurrentAP <= 0) continue;

            IPawnAction action = ChooseAction(enemy);
            if (action != null)
            {
                yield return PerformAction(enemy, action);
            }
        }

        EndEnemyTurn();
    }

    private IPawnAction ChooseAction(Pawn enemy)
    {
        // 1. Проверяем основную атаку
        if (enemy.AttackAction != null && HasValidTargets(enemy.AttackAction))
        {
            return enemy.AttackAction;
        }

        /*        foreach (var action in enemy.Actions)
                {
                    if (action is AttackAction && HasValidTargets(action))
                    {
                        return action;
                    }
                }*/

        // 3. Если атаковать не можем - выбираем движение
        return enemy.MoveAction;
    }

    private bool HasValidTargets(IPawnAction action)
    {
        return action.CalculateTargets().Count > 0;
    }

    private IEnumerator PerformAction(Pawn enemy, IPawnAction action)
    {
        /* Debug.Log($"{enemy.name} выполняет {action.GetType().Name}");

         bool actionCompleted = false;
         action.Perform(() => actionCompleted = true);

         while (!actionCompleted)
             yield return null;*/

        yield return new WaitForSeconds(0.3f); // Пауза между действиями
    }

    private void EndEnemyTurn()
    {
        StateManager.Instance.ChangeState<PlayerTurnState>();
    }
}