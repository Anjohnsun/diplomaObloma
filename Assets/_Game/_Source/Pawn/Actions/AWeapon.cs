using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon
{
    protected Pawn _owner;
    public int BaseDamage { get; private set; }

    public AWeapon(Pawn owner, int baseDamage)
    {
        _owner = owner;
        BaseDamage = baseDamage;
    }

    public abstract List<Vector2Int> GetAttackArea(Vector2Int position);
    public virtual int CalculateDamage() => Mathf.RoundToInt(BaseDamage * _owner.PawnStats.STR_VALUE);
}