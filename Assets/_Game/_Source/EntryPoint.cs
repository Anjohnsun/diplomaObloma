using System.Collections;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [Inject]
    private void Construct()
    {
    }

    IEnumerator Start()
    {
        StateManager.Instance.ChangeState<StartAnimationState>();
        yield return null;
    }
}
