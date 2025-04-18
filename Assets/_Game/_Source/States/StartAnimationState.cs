using System.Collections;
using UnityEngine;
using Zenject;

public class StartAnimationState : IGameState
{
    public StartAnimationState()
    {
    }

    public void Enter()
    {
        LevelManager.Instance.InitializeFirstLevel();
    }

    public void Exit()
    {
    }
}
