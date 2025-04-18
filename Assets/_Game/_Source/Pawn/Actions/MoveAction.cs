using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MoveAction : IPawnAction
{
    private IMoveStrategy _moveStrategy;

    private List<FieldTile> _possibleMoves;

    public Pawn Pawn { get; private set; }
    public float Duration => 0.6f;

    public MoveAction(Pawn pawn, IMoveStrategy moveStrategy)
    {
        Pawn = pawn;
        _moveStrategy = moveStrategy;
    }

    public List<FieldTile> CalculateTargets()
    {
        _possibleMoves = _moveStrategy.GetPossibleMoves(Pawn.GridPosition);
        return _possibleMoves;
    }


    public void Perform(Vector2 targetWorldPosition, Action handler)
    {
        FieldTile targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);

        if (targetTile != null && _possibleMoves.Contains(targetTile))
        {
            GridManager.Instance.MovePawn(Pawn, targetTile);
            Pawn.PawnStats.UseAP();

            Pawn.transform.DOMove(targetTile.transform.position, Duration)
                .OnComplete(() =>
                {

                    handler();
                    return;
                });

            //HANDLE LAYER_ORDER UPDATE
        }
        else
        {
            handler();
        }
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }

    public void SelfRealize()
    {
        throw new NotImplementedException();
    }

    public bool CanPerform(Vector2 targetWorldPosition)
    {
        FieldTile targetTile = GridManager.Instance.WorldPositionToTile(targetWorldPosition);
        return targetTile != null && _possibleMoves.Contains(targetTile);
    }
}
