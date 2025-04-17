using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject.Asteroids;

public class StateManager
{
    private readonly Dictionary<Type, IGameState> _states;
    private IGameState _currentState;

    public StateManager(
        StartAnimationState startAnimationState,
        BattleState battleState,
        UpgradeState upgradeState)
    {
        _states = new Dictionary<Type, IGameState>
        {
            { typeof(StartAnimationState), startAnimationState },
            { typeof(BattleState), battleState },
            { typeof(UpgradeState), upgradeState}
        };

    }

    public void ChangeState<T>() where T : IGameState
    {
        var stateType = typeof(T);

        if (!_states.TryGetValue(stateType, out var newState))
            Debug.LogError($"State {stateType.Name} is not registered");


        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
