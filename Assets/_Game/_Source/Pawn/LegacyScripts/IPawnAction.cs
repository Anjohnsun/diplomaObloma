using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPawnAction
{
    Pawn Pawn { get; }
    float Duration { get; }
/*    int UsesNumber { get; }
    int ReloadDuration { get; }*/
    List<FieldTile> CalculateTargets();
    void Perform(Vector2 point, Action handler);
    void Cancel();
    void SelfRealize(Action handler);
    bool CanPerform(Vector2 point);
    bool CanPerformAuto();
}
