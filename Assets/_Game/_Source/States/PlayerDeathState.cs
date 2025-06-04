using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PlayerDeathState : IGameState
{
    private MonoBehaviour _coroutines;
    private Transform _camera;
    private TextMeshProUGUI _endText;

    public PlayerDeathState(MonoBehaviour coroutines, [Inject(Id = "Camera")] Transform camera, [Inject(Id = "EndText")] TextMeshProUGUI text)
    {
        _coroutines = coroutines;
        _camera = camera;
        _endText = text;
    }
    public void Enter()
    {
        _coroutines.StartCoroutine(FinalAnimationCoroutine());
    }

    private IEnumerator FinalAnimationCoroutine()
    {
        yield return new WaitForSeconds(1);

        _camera.DOMoveY(_camera.position.y + 20, 2.5f).SetEase(Ease.InSine).OnComplete(() =>
        DOTween.To(() => _endText.color, (x) => _endText.color = x, new Color(_endText.color.r, _endText.color.g, _endText.color.b, 1), 1f));
        yield return new WaitForSeconds(4);
        DOTween.To(() => _endText.color, (x) => _endText.color = x, new Color(_endText.color.r, _endText.color.g, _endText.color.b, 0), 1f);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
    }
}
