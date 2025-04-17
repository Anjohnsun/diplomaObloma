using System.Collections.Generic;
using UnityEngine;

public class LittleSword : AWeapon
{
    public LittleSword(Pawn owner) : base(owner, 5) { }

    public override List<Vector2Int> GetAttackArea(Vector2Int position)
    {
        List<Vector2Int> area = new List<Vector2Int>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                area.Add(new Vector2Int(position.x + dx, position.y + dy));
            }
        }

        return area;
    }
}
