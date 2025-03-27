using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ArmorDecorator : PawnStatsDecorator
{
    private int _armor;
    public ArmorDecorator(int lifetime, int armor) : base(lifetime)
    {
        _armor = armor;
    }



    public override void TakeDamage(int damage)
    {
        int reducedDamage = Mathf.Max(damage - _armor, 0);
        base.TakeDamage(reducedDamage);
    }
}
