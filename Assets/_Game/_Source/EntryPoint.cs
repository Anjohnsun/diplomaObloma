using System.Collections;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    private StateManager _stateManager;

    [Inject]
    private void Construct(StateManager stateManager)
    {
        _stateManager = stateManager;
    }

    IEnumerator Start()
    {
        _stateManager.ChangeState<StartAnimationState>();
        yield return null;
    }
}
