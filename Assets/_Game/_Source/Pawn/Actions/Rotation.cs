using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : APawnAction
{
    public override Sprite ActionIcon => Resources.Load<Sprite>("ActionSprites/Rotation");

    public override MarkerType Marker => MarkerType.interact;

    public Rotation(APawn owner, float duration, int usesNumber) : base(owner, duration, usesNumber)
    {
        _hint = $"Поменяйтесь с противником местами";
    }

    public override List<FieldTile> CalculateTargets()
    {
        _possibleMoves = GetPossibleTargets(_owner.GridPosition);
        return _possibleMoves;
    }

    public override void Perform(FieldTile tile, Action handler)
    {
        base.Perform(tile, handler);

        if (tile != null && _possibleMoves.Contains(tile) && tile.Pawn != null)
        {
            Debug.Log("Perform Position Swap");
            base.Perform(tile, handler);

            _owner.PawnStats.UseAP();

            Vector3 ownerWorldPos = _owner.transform.position;
            Vector3 targetWorldPos = tile.Pawn.transform.position;

            Vector2Int ownerGridPos = _owner.GridPosition;
            Vector2Int targetGridPos = tile.Pawn.GridPosition;

                Debug.Log($"ROTATE TARGET POSITION: {_owner.GridPosition.ToString()}");

            APawn targetPawn = tile.Pawn;

            Sequence swapSequence = DOTween.Sequence();

            swapSequence.Join(_owner.transform.DOMoveY(ownerWorldPos.y + 0.5f, 0.3f)).SetEase(Ease.OutQuad);
            swapSequence.Join(targetPawn.transform.DOMoveY(targetWorldPos.y + 0.5f, 0.3f)).SetEase(Ease.OutQuad);

            swapSequence.Append(_owner.transform.DOMove(new Vector3(
                targetWorldPos.x,
                ownerWorldPos.y + 0.5f, 
                targetWorldPos.z
            ), 0.5f).SetEase(Ease.InOutQuad));

            swapSequence.Join(targetPawn.transform.DOMove(new Vector3(
                ownerWorldPos.x,
                targetWorldPos.y + 0.5f, 
                ownerWorldPos.z
            ), 0.5f).SetEase(Ease.InOutQuad));

            swapSequence.Append(_owner.transform.DOMoveY(targetWorldPos.y, 0.3f).SetEase(Ease.InQuad));
            swapSequence.Join(targetPawn.transform.DOMoveY(ownerWorldPos.y, 0.3f).SetEase(Ease.InQuad));

            swapSequence.OnComplete(() =>
            {
                GridManager.Instance.GetTileAtGridPosition(ownerGridPos).RemovePawn();
                GridManager.Instance.GetTileAtGridPosition(targetGridPos).RemovePawn();

                _owner.UpdateGridPosition(targetGridPos);
                targetPawn.UpdateGridPosition(ownerGridPos);

                Debug.Log($"ROTATE NEW POSITION: {_owner.GridPosition.ToString()}");

                handler?.Invoke();
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
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        int a = GridManager.Instance.VerticalSize;

        for (int x = 0; x < a; x++)
        {
            for (int y = 0; y < a; y++)
            {
                if (x == currentPosition.x && y == currentPosition.y)
                    continue;

                possibleMoves.Add(new Vector2Int(x, y));
            }
        }

        return GridManager.Instance.GetAvailableTargets(possibleMoves, GridManager.HasAnyPawn);
    }
}
