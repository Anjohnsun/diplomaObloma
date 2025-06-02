using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class StartAnimationState : IGameState
{
    private TextMeshProUGUI _startText;
    private MonoBehaviour _coroutines;
    public StartAnimationState(TextMeshProUGUI startText, MonoBehaviour coroutines)
    {
        _startText = startText;
        _coroutines = coroutines;
    }

    public void Enter()
    {
        _coroutines.StartCoroutine(StartAnimCor());
    }

    public void Exit()
    {
        _startText.enabled = false;
    }

    private IEnumerator StartAnimCor()
    {
        DOTween.To(() => _startText.color, (x) => _startText.color = x, new Color(_startText.color.r, _startText.color.g, _startText.color.b, 1), 0.7f);
        yield return new WaitForSeconds(2);
        DOTween.To(() => _startText.color, (x) => _startText.color = x, new Color(_startText.color.r, _startText.color.g, _startText.color.b, 0), 0.7f)
            .OnComplete(() => LevelManager.Instance.InitializeFirstLevel());

        yield return null;
    }
}
