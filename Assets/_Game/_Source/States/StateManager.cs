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
        currentState?.Exit(); // ����� �� �������� ���������
        currentState = newState;
        currentState.Enter(); // ���� � ����� ���������
    }
}
