using System.Collections;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [Inject]
    private void Construct(PlayerPawn player)
    {
        player.Construct(0, 0, 0, 0);
    }

    IEnumerator Start()
    {
        StateManager.Instance.ChangeState<StartAnimationState>();
        yield return null;
    }
}
