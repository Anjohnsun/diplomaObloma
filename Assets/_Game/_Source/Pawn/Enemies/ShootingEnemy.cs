using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : AEnemyPawn
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

        _actions.Add(new XMoveActionStayFar(this, 0.6f, -1));
        _actions.Add(new BaseShoot(this, 0.4f, -1, 6));
        _actions.Add(new SkipTurn(this, 0.4f, -1));
    }

    public override string GetHintText()
    {
        return "Стрелок. Будет стараться держать дистанцию и атаковать издалека.";
    }


    public override void PerformActions(Action handler)
    {
        base.PerformActions(handler);

        if (PawnStats.CurrentAP <= 0)
        {
            PawnStats.StartTurn();
        }

        int randomActionIndex = UnityEngine.Random.Range(0, _actions.Count);
        APawnAction selectedAction = _actions[randomActionIndex];

        selectedAction.SelfPerform(() =>
        {
            if (PawnStats.CurrentAP > 0)
            {
                PerformActions(handler);
            }
            else
            {
                handler?.Invoke();
            }
        });
    }
}
