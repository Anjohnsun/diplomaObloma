using UnityEngine;

public class LevelTransitionState : IGameState
{
    public void Enter()
    {
        Debug.Log("-> Level Transition State");
    }

    public void Exit()
    {
    }
}
