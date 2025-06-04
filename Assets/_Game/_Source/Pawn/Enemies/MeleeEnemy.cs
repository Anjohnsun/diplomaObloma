using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : AEnemyPawn
{
    public override void Construct(int currentLevel)
    {
        if (currentLevel < 2)
        {
            Construct(0, 0, 0, 0);
        }
        else if (currentLevel < 3)
        {
            Construct(1, 0, 0, 0);
        }
        else if (currentLevel < 4)
        {
            Construct(1, 1, 0, 0);
        }
        else
        {
            Construct(1, 1, 0, 1);
        }
        _actions = new List<APawnAction>();

        _actions.Add(new XMoveAction(this, 0.6f, -1));
        _actions.Add(new BaseMelee(this, 0.4f, -1));
    }

    public override string GetHintText()
    {
        return "Самый простой противник. Преследует и атакует с соседних клеток";
    }


    public override void PerformActions(Action handler)
    {
        base.PerformActions(handler);

        if (PawnStats.CurrentAP <= 0)
        {
            PawnStats.StartTurn();
        }

        if (PawnStats.CurrentAP > 0)
        {
            PawnStats.UseAP();
            if (_actions[1].SelfPerform(() =>
            {
                if (PawnStats.CurrentAP > 0)
                    PerformActions(handler);
                else
                    handler.Invoke();
            }))
            {
                Debug.Log("performed");
            }
            else
            {
                if (_actions[0].SelfPerform(() =>
                {
                    if (PawnStats.CurrentAP > 0)
                        PerformActions(handler);
                    else
                        handler.Invoke();
                }))
                    Debug.Log("performed");
                else
                {
                    PawnStats.UseAP(100);
                    handler.Invoke();
                }
            }

        }
    }
}
