using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrowAction : IPawnAction
{
    private List<FieldTile> _possibleTargets;
    public Pawn Pawn { get; }
    public float Duration => 1.5f;
    private int _damage = 15;

    public MagicArrowAction(Pawn pawn, int damage)
    {
        Pawn = pawn;
        _damage = damage;
    }

    public List<FieldTile> CalculateTargets()
    {
        _possibleTargets = new List<FieldTile>();

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

            var arrowPrefab = Resources.Load<GameObject>("MagicArrow");
            var arrow = GameObject.Instantiate(arrowPrefab, Pawn.transform.position, Quaternion.identity);

            var firstTarget = targetTile.Pawn;

            Pawn secondTarget = null;
            float minDistance = float.MaxValue;

            foreach (var tile in _possibleTargets)
            {
                if (tile.Pawn != firstTarget)
                {
                    float dist = Vector2.Distance(firstTarget.transform.position, tile.Pawn.transform.position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        secondTarget = tile.Pawn;
                    }
                }
            }

            Sequence arrowSequence = DOTween.Sequence();

            arrowSequence.Append(arrow.transform.DOMove(firstTarget.transform.position, Duration * 0.6f));
            arrowSequence.AppendCallback(() => {
                firstTarget.PawnStats.TakeDamage(_damage);
                if (secondTarget == null)
                {
                    GameObject.Destroy(arrow);
                    handler();
                }
            });

            if (secondTarget != null)
            {
                arrowSequence.Append(arrow.transform.DOMove(secondTarget.transform.position, Duration * 0.6f));
                arrowSequence.AppendCallback(() => {
                    secondTarget.PawnStats.TakeDamage(_damage);
                    GameObject.Destroy(arrow);
                    handler();
                });
            }

            arrowSequence.Play();
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