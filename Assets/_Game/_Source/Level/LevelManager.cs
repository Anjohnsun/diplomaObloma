using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _transitionDuration = 1.5f;
    [SerializeField] private float _playerTransitionDur = 0.7f;
    [SerializeField] private float _cameraOffsetY = 3f;
    [SerializeField] private int _levelSpacing = 20;
    [SerializeField] private List<GameObject> _levelPrefabs;

    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _mainCamera;

    private StateManager _stateManager;

    private GameObject _currentLevel;
    private GameObject _nextLevel;
    private int _currentLevelIndex = 0;
    private Sequence _transitionSequence;

    private TurnManager _turnManager;

    public static LevelManager Instance { get; private set; }

    [Inject]
    private void Construct(StateManager stateManager)
    {
        _stateManager = stateManager;

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_mainCamera == null) _mainCamera = Camera.main;

        _turnManager = new TurnManager(_stateManager);
    }

    public void InitializeFirstLevel()
    {
        _currentLevel = Instantiate(_levelPrefabs[0], Vector3.zero, Quaternion.identity);
        _currentLevel.GetComponent<LevelInfo>().GenerateLevel(0);

        _mainCamera.transform.position = new Vector3(0, -_levelSpacing, _mainCamera.transform.position.z
        );

        _mainCamera.transform.DOMoveY(_cameraOffsetY, _transitionDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _currentLevel.GetComponent<LevelInfo>().InitLevel();

                FieldTile startTile = GridManager.Instance.GetTileAtGridPosition(_currentLevel.GetComponent<LevelInfo>().PlayerPosition);
                _player.DOMove(startTile.transform.position, _playerTransitionDur).OnComplete(() =>
                    _currentLevel.GetComponent<LevelInfo>().StartLevelPassing(_player.GetComponent<Pawn>(), _turnManager));



                PrepareNextLevel();
            });
    }

    public void PrepareNextLevel()
    {
        if (_nextLevel != null) return;

        GameObject nextLevelPrefab = _levelPrefabs[Random.Range(0, _levelPrefabs.Count)];

        Vector3 spawnPos = _currentLevel.transform.position + Vector3.up * _levelSpacing;
        _nextLevel = Instantiate(nextLevelPrefab, spawnPos, Quaternion.identity);

        int levelNumber = _currentLevelIndex + 1;
        _nextLevel.GetComponent<LevelInfo>().GenerateLevel(levelNumber);
    }

    public void StartLevelTransition()
    {
        if (_nextLevel == null)
            throw new System.Exception("Next level not prepared!");

        _stateManager.ChangeState<LevelTransitionState>();

        if (_transitionSequence != null && _transitionSequence.IsPlaying())
            _transitionSequence.Kill();

        _transitionSequence = DOTween.Sequence();
        _transitionSequence
            .Append(_mainCamera.transform.DOMoveY(_nextLevel.transform.position.y - _cameraOffsetY, _transitionDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(CompleteLevelTransition));
    }

    private void CompleteLevelTransition()
    {

        _currentLevel = _nextLevel;
        _nextLevel = null;

        _currentLevel.GetComponent<LevelInfo>().InitLevel();
        FieldTile startTile = GridManager.Instance.GetTileAtGridPosition(_nextLevel.GetComponent<LevelInfo>().PlayerPosition);
        _player.DOMove(startTile.transform.position, _playerTransitionDur).OnComplete(() =>
            _currentLevel.GetComponent<LevelInfo>().StartLevelPassing(_player.GetComponent<Pawn>(), _turnManager));


        _currentLevelIndex += 1;
        PrepareNextLevel();
    }
}