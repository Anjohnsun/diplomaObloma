using DG.Tweening.Core.Easing;
using UnityEngine;
using Zenject.Asteroids;

public class StateManager
{
    private AGameState currentState;

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(AGameState newState)
    {
        currentState?.Exit(); // Выход из текущего состояния
        currentState = newState;
        currentState.Enter(); // Вход в новое состояние
    }
}
