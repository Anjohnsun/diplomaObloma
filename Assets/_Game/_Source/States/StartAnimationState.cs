using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class StartAnimationState : IGameState
{
    private TextMeshProUGUI _startText;
    private MonoBehaviour _coroutines;
    private GameplayUI _UI;

    public StartAnimationState([Inject(Id = "StartText")]TextMeshProUGUI startText, MonoBehaviour coroutines, GameplayUI ui)
    {
        _startText = startText;
        _coroutines = coroutines;
        _UI = ui;
    }

    public void Enter()
    {
        _UI.HideGameplayInterface();
        _coroutines.StartCoroutine(StartAnimCor());
    }

    public void Exit()
    {
        _startText.enabled = false;
    }

    private IEnumerator StartAnimCor()
    {
        DOTween.To(() => _startText.color, (x) => _startText.color = x, new Color(_startText.color.r, _startText.color.g, _startText.color.b, 1), 3f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(4);
        DOTween.To(() => _startText.color, (x) => _startText.color = x, new Color(_startText.color.r, _startText.color.g, _startText.color.b, 0), 1f).SetEase(Ease.InSine)
            .OnComplete(() => LevelManager.Instance.InitializeFirstLevel());

        yield return null;
    }
}
