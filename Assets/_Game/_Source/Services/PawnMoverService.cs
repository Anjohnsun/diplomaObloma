using DG.Tweening;
using UnityEngine;

public class PawnMoverService : IPawnMoverService
{
    private AnimationCurve _gridMoveCurve;
    private AnimationCurve _appearMoveCurve;

    public float DURATION = 0.5f;

    public PawnMoverService(AnimationCurve gridMoveCurve, AnimationCurve appearMoveCurve, float duration)
    {
        _gridMoveCurve = gridMoveCurve;
        _appearMoveCurve = appearMoveCurve;
        DURATION = duration;
    }

    public void AppearMoveTo(Pawn pawn, Vector2 to)
    {
        pawn.transform.DOMove(to, DURATION).SetEase(_appearMoveCurve);
    }

    public void GridMoveTo(Pawn pawn, Vector2 to)
    {
        pawn.transform.DOMove(to, DURATION).SetEase(_gridMoveCurve);
    }
}
