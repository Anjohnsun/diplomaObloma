using System;
using UnityEngine;

public class TestEnemy : AEnemyPawn
{

    public override void PerformActions(Action handler)
    {
        base.PerformActions(handler);
        Debug.Log("Test enemy performing action");
        handler.Invoke();
    }


    public override string GetHintText()
    {
        return "��� �������� ����. ��� ��������� �������� �� ���-�� ����������";
    }

    public override void Construct(int currentLevel)
    {
        if (currentLevel < 1)
            Construct(0, 0, 0, 0);
        else if (currentLevel < 2)
            Construct(1, 0, 0, 0);
        else if (currentLevel < 3)
            Construct(2, 0, 0, 1);
    }
}
