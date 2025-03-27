using System.Collections;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    private StateManager _stateManager;
    private StartAnimationState _startAnimationState;

    [Inject]
    private void Construct(StateManager stateManager, StartAnimationState startAnimationState)
    {
        _stateManager = stateManager;
        _startAnimationState = startAnimationState;
    }

    IEnumerator Start()
    {
        //������ ����� ���� ��� �������� ����������

        //������ ��������� ��������
        _stateManager.ChangeState(_startAnimationState);
        Debug.Log("EntryPoint: Start animation");

        return null;
    }



}
