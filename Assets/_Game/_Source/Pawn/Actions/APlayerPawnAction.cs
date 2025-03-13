using UnityEngine;

public abstract class APlayerPawnAction : IPawnAction
{
    public const PlayerPawnActions ActionType = PlayerPawnActions.move;
    public abstract void DoAction();
}
