using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;

public class StateManager
{
    private AGameState currentState;

    //Dictionary<Type, AGameState> _states;

    /*public StateManager (StartAnimationState a, PlayerTurnState b, EnemyTurnState c)
    {
        _states.Add(a.GetType(), a);
        _states.Add(b.GetType(), b);
        _states.Add(c.GetType(), c);
    }*/

    public void ChangeState(AGameState newState) 
    {
        currentState?.Exit(); // Выход из текущего состояния
        currentState = newState;
        currentState.Enter(); // Вход в новое состояние
    }
}
