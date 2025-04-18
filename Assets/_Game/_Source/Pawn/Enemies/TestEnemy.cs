using UnityEngine;

public class TestEnemy : AEnemyPawn
{
    public override void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
    {
        base.Construct(hpLvl, apLvl, strLvl, armLvl);
        MoveAction = new MoveAction(this, new PlusMove());
        AttackAction = new AttackAction(this, new LittleSword(this));
    }

    protected override void ChooseAndPerformAction()
    {
        base.ChooseAndPerformAction();
        Debug.Log("Test enemy performing action");
    }
}
