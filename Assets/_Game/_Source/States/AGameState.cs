using UnityEngine;

public abstract class AGameState
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
}
