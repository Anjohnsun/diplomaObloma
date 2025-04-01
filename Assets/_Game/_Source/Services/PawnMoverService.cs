using DG.Tweening;
using UnityEngine;

public class PawnMoverService : IPawnMoverService
{
    private AnimationCurve _gridMoveCurve;
    private AnimationCurve _appearMoveCurve;

    public float DURATION = 0.5f;

    public float GridDuration => DURATION;
    public float AppearDuration => DURATION;

    public PawnMoverService(MoverSettingsSO settings)
    {
        _gridMoveCurve = settings.GridMoveCurve;
        _appearMoveCurve = settings.GridMoveCurve;
        DURATION = settings.Duration;
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
