using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _cameraTransitionDuration = 1.5f;
    [SerializeField] private float _playerTransitionDuration = 0.7f;
    [SerializeField] private float _cameraOffsetY = 3f;
    [SerializeField] private List<GameObject> _levelPrefabs;

    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private EnemySpawner _enemySpawner;

    private GameObject _currentLevel;
    private int _currentLevelIndex = -1;
    private Sequence _transitionSequence;

    public PlayerPawn PlayerPawn => _player.GetComponent<PlayerPawn>();
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNextLevel()
    {
        if (_transitionSequence != null && _transitionSequence.IsPlaying())
            _transitionSequence.Kill();

        _transitionSequence = DOTween.Sequence();

        _transitionSequence.Append(_mainCamera.transform.DOMoveY(_cameraOffsetY, _cameraTransitionDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (_currentLevelIndex + 1 >= _levelPrefabs.Count)
                {
                    StateManager.Instance.ChangeState<WinState>();
                    return;
                }

                if (_currentLevel != null)
                {
                    Destroy(_currentLevel);
                }

                _currentLevelIndex++;
                _currentLevel = Instantiate(_levelPrefabs[_currentLevelIndex], Vector3.zero, Quaternion.identity);

                var levelInfo = _currentLevel.GetComponent<LevelInfo>();
                levelInfo.GenerateLevel(_currentLevelIndex, _enemySpawner, () =>
                {
                    var startTile = GridManager.Instance.GetTileAtGridPosition(_currentLevel.GetComponent<LevelInfo>().PlayerPosition);
                    _player.DOMove(startTile.transform.position, _playerTransitionDuration)
                        .OnComplete(() =>
                        {
                            StateManager.Instance.ChangeState<PlayerTurnState>();
                        });
                });
            }));
    }
}