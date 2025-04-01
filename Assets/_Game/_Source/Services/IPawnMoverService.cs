using UnityEngine;

public interface IPawnMoverService
{
    float GridDuration { get; }
    float AppearDuration { get; }
    void GridMoveTo(Pawn pawn, Vector2 to);
    void AppearMoveTo(Pawn pawn, Vector2 to);
}
