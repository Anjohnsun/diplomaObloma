using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PerimeterTP : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/PerimeterTP");
    public override MarkerType Marker => MarkerType.interact;

    public PerimeterTP(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = "Телепортация на свободную крайнюю клетку";
    }

    public override List<FieldTile> CalculateTargets()
    {
        _possibleMoves = GetPossibleTargets(_owner.GridPosition);
        return _possibleMoves;
    }

    public override void Perform(FieldTile tile, Action handler)
    {
        base.Perform(tile, handler);

        if (tile != null && _possibleMoves.Contains(tile) && tile.Pawn == null)
        {
            _owner.PawnStats.UseAP();

            Vector3 targetWorldPos = tile.transform.position;
            Vector2Int targetGridPos = GridManager.Instance.GetTileCoordinates(tile);

            Sequence teleportSequence = DOTween.Sequence();
            teleportSequence.Append(_owner.transform.DOScale(0f, 0.3f).SetEase(Ease.OutQuad));
            teleportSequence.AppendCallback(() =>
            {
                _owner.transform.position = new Vector3(
                    targetWorldPos.x,
                    targetWorldPos.y,
                    targetWorldPos.z
                );
            });
            teleportSequence.Append(_owner.transform.DOScale(1f, 0.3f).SetEase(Ease.InQuad));
            teleportSequence.OnComplete(() =>
            {
                GridManager.Instance.GetTileAtGridPosition(_owner.GridPosition).RemovePawn();
                _owner.UpdateGridPosition(targetGridPos);
                handler.Invoke();
            });
        }
        else
        {
            handler?.Invoke();
        }
    }

    public override void CanPerform(Vector2Int tile)
    {
        throw new System.NotImplementedException();
    }

    public override bool SelfPerform(Action handler)
    {
        throw new System.NotImplementedException();
    }

    public List<FieldTile> GetPossibleTargets(Vector2Int currentPosition)
    {
        List<Vector2Int> perimeterTiles = new List<Vector2Int>();
        int gridSize = GridManager.Instance.VerticalSize;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                bool isBorderCell = x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1;
                bool isNotCurrentPos = x != currentPosition.x || y != currentPosition.y;

                if (isBorderCell && isNotCurrentPos)
                {
                    perimeterTiles.Add(new Vector2Int(x, y));
                }
            }
        }

        return GridManager.Instance.GetAvailableTargets(perimeterTiles, GridManager.IsFreeTile);
    }
}