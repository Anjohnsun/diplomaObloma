using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FireballAction : IPawnAction
{
    private List<FieldTile> _possibleTargets;
    public Pawn Pawn { get; }
    public float Duration => 1.2f;
    private int _damage = 25;
    private int _splashDamage = 15;

    public FireballAction(Pawn pawn, int mainDamage, int splashDamage)
    {
        Pawn = pawn;
        _damage = mainDamage;
        _splashDamage = splashDamage;
    }

    public List<FieldTile> CalculateTargets()
    {
        _possibleTargets = new List<FieldTile>();

        // Получаем все клетки с противниками
        var allTiles = GridManager.Instance.GetAvailableTargets(GridManager.HasAnyPawn);
        foreach (var tile in allTiles)
        {
            if (tile.Pawn != null && tile.Pawn.PawnTeam != Pawn.PawnTeam)
            {
                _possibleTargets.Add(tile);
            }
        }

        return _possibleTargets;
    }

    public void Perform(Vector2 targetWorldPosition, Action handler)
    {
        var targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);

        if (targetTile != null && _possibleTargets.Contains(targetTile) && targetTile.Pawn != null)
        {
            Pawn.PawnStats.UseAP();

            var fireballPrefab = Resources.Load<GameObject>("Fireball");
            var fireball = GameObject.Instantiate(fireballPrefab, Pawn.transform.position, Quaternion.identity);

            fireball.transform.DOMove(targetTile.Pawn.transform.position, Duration * 0.7f)
                .OnComplete(() => {
                    targetTile.Pawn.PawnStats.TakeDamage(_damage);

                
                    Vector2Int centerPos = GridManager.Instance.WorldToGridPosition(targetWorldPosition);
                    List<FieldTile> splashTiles = new List<FieldTile>();

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;

                            var splashPos = new Vector2Int(centerPos.x + x, centerPos.y + y);
                            var splashTile = GridManager.Instance.GetTileAtGridPosition(splashPos);
                            if (splashTile != null && splashTile.Pawn != null && splashTile.Pawn.PawnTeam != Pawn.PawnTeam)
                            {
                                splashTiles.Add(splashTile);
                            }
                        }
                    }
                   
                    foreach (var tile in splashTiles)
                    {
                        tile.Pawn.PawnStats.TakeDamage(_splashDamage);
                    }

                    GameObject.Destroy(fireball);
                    handler();
                });
        }
        else
        {
            handler();
        }
    }

    public bool CanPerform(Vector2 targetWorldPosition)
    {
        FieldTile targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);
        return targetTile != null && _possibleTargets.Contains(targetTile);
    }

    public bool CanPerformAuto()
    {
        return CalculateTargets().Count > 0;
    }

    public void Cancel() { }
    public void SelfRealize(Action handler)
    {

    }
}
