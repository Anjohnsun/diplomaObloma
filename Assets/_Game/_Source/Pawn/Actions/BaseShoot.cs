using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseShoot : APawnAction
{
    private int _distance;
    private GameObject _bulletPrefab;
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/BaseShoot");

    public override MarkerType Marker => MarkerType.attack;

    public BaseShoot(APawn owner, float duration, int usesNumber, int distance) : base(owner, duration, usesNumber)
    {
        _distance = distance;
        _hint = $"Дальняя атака с ограниченной дистанцией";

        _bulletPrefab = Resources.Load<GameObject>("Bullet");
    }

    public override List<FieldTile> CalculateTargets()
    {
        _possibleMoves = GetPossibleTargets(_owner.GridPosition, _distance);
        return _possibleMoves;
    }

    public override void CanPerform(Vector2Int tile)
    {
        throw new System.NotImplementedException();
    }

    public override bool SelfPerform(Action handler)
    {
        Vector2Int playerPos = LevelManager.Instance.PlayerPawn.GridPosition;
        Vector2Int currentPos = _owner.GridPosition;

        float distanceToPlayer = Vector2Int.Distance(currentPos, playerPos);
        if (distanceToPlayer > _distance)
        {
            handler?.Invoke();
            return false;
        }

        FieldTile playerTile = GridManager.Instance.GetTileAtGridPosition(playerPos);
        if (playerTile != null && playerTile.Pawn == LevelManager.Instance.PlayerPawn)
        {
            CalculateTargets();
            Perform(playerTile, handler);
            return true;
        }

        handler?.Invoke();
        return false;
    }

    public override void Perform(FieldTile tile, Action handler)
    {
        if (tile != null && _possibleMoves.Contains(tile))
        {
            Debug.Log("Perform BaseShoot");
            base.Perform(tile, handler);

            _owner.PawnStats.UseAP();

            GameObject bullet = GameObject.Instantiate(
                _bulletPrefab,
                _owner.transform.position,
                Quaternion.identity
            );

            bullet.transform
                .DOMove(tile.transform.position, _duration / 2).SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                    GameObject.Destroy(bullet);

                    tile.Pawn.transform.DOMoveY(tile.Pawn.transform.position.y + 0.3f, _duration / 4)
                        .OnComplete(() =>
                            tile.Pawn.transform.DOMoveY(tile.Pawn.transform.position.y - 0.3f, _duration / 4)
                                .OnComplete(() =>
                                {
                                    tile.Pawn.PawnStats.TakeDamage(_owner.PawnStats.STR);
                                    handler.Invoke();
                                })
                        );
                });
        }
        else
        {
            handler?.Invoke();
        }
    }

    public List<FieldTile> GetPossibleTargets(Vector2Int currentPosition, int distanceX)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        for (int x = -distanceX; x <= distanceX; x++)
        {
            for (int y = -distanceX; y <= distanceX; y++)
            {
                if (x * x + y * y > distanceX * distanceX)
                    continue;

                if (x == 0 && y == 0)
                    continue;

                possibleMoves.Add(new Vector2Int(currentPosition.x + x, currentPosition.y + y));
            }
        }

        return GridManager.Instance.GetAvailableTargets(possibleMoves, GridManager.HasAnyPawn);
    }
}



