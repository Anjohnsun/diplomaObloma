using System.Collections;
using UnityEngine;
using Zenject;

public class StartAnimationState : IGameState
{
    [Inject]
    public StartAnimationState()
    {
    }

    public void Enter()
    {
        Debug.Log("Animation");

        LevelManager.Instance.InitializeFirstLevel();
    }

    public void Exit()
    {
    }
}
