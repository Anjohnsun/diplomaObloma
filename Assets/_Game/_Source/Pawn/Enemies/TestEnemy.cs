using System;
using UnityEngine;

public class TestEnemy : AEnemyPawn
{
    /*    public override void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
        {
            base.Construct(hpLvl, apLvl, strLvl, armLvl);
        }

        public override void PerformActions()
        {
            base.PerformActions();
            Debug.Log("Test enemy performing action");
        }

        public override string GetHintText()
        {
            return "������ ��� ����� ���� �� ����� ���� �����, ���� ���� ���������������!";
        }*/
    public override string GetHintText()
    {
        throw new NotImplementedException();
    }
}
