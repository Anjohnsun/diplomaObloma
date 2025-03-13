using UnityEngine;

public interface IPawnMoverService
{
    void GridMoveTo(Pawn pawn, Vector2 to);
    void AppearMoveTo(Pawn pawn, Vector2 to);
}
