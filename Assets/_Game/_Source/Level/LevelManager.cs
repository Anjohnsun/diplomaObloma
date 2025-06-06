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
    [SerializeField] private EnemySpawner _enemySpawner;

    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _mainCamera;

    private GameObject _currentLevel;
    private GameObject _nextLevel;
    private GameObject _previousLevel;

    private int _currentLevelIndex = 0;
    private Sequence _transitionSequence;

    private EnemyManager _enemyManager;

    public PlayerPawn PlayerPawn => _player.GetComponent<PlayerPawn>();
    public static LevelManager Instance { get; private set; }

    [Inject]
    private void Construct(EnemyManager enemyManager)
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_mainCamera == null) _mainCamera = Camera.main;

        _enemyManager = enemyManager;
    }

    public void InitializeFirstLevel()
    {
        _currentLevel = Instantiate(_levelPrefabs[0], Vector3.zero, Quaternion.identity);
        _currentLevel.GetComponent<LevelInfo>().GenerateLevel(0, _enemySpawner, () => _currentLevel.GetComponent<LevelInfo>().InitLevel(_player.GetComponent<APawn>()));

        _mainCamera.transform.position = new Vector3(0, -_levelSpacing, _mainCamera.transform.position.z
        );

        _mainCamera.transform.DOMoveY(_cameraOffsetY, _transitionDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                FieldTile startTile = GridManager.Instance.GetTileAtGridPosition(_currentLevel.GetComponent<LevelInfo>().PlayerPosition);
                _player.DOMove(startTile.transform.position, _playerTransitionDur).OnComplete(() =>
                {
                    Debug.Log("LevelManager: first level inited");

                    StateManager.Instance.ChangeState<UpgradeState>();
                });


            });
    }

    public void PrepareNextLevel()
    {
        if (_nextLevel != null) return;

        GameObject nextLevelPrefab = _levelPrefabs[Random.Range(0, _levelPrefabs.Count)];

        Vector3 spawnPos = _mainCamera.transform.position + Vector3.up * _levelSpacing;
        _nextLevel = Instantiate(nextLevelPrefab, spawnPos, Quaternion.identity);

        int levelNumber = _currentLevelIndex + 1;
        _nextLevel.GetComponent<LevelInfo>().GenerateLevel(levelNumber, _enemySpawner, () => Mathf.Round(1));
    }

    public void StartLevelTransition()
    {
        PrepareNextLevel();

        if (_nextLevel == null)
            throw new System.Exception("Next level not prepared!");


        StateManager.Instance.ChangeState<LevelTransitionState>();

        if (_transitionSequence != null && _transitionSequence.IsPlaying())
            _transitionSequence.Kill();

        _transitionSequence = DOTween.Sequence();
        _transitionSequence
            .Append(_mainCamera.transform.DOMoveY(_nextLevel.transform.position.y + _cameraOffsetY, _transitionDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(CompleteLevelTransition));

    }

    private void CompleteLevelTransition()
    {
        _previousLevel = _currentLevel;
        _currentLevel = _nextLevel;
        _nextLevel = null;

        Debug.Log("Init 1");
        _currentLevel.GetComponent<LevelInfo>().InitLevel(_enemyManager, _player.GetComponent<APawn>());
        Debug.Log("Init 2");
        FieldTile startTile = GridManager.Instance.GetTileAtGridPosition(_currentLevel.GetComponent<LevelInfo>().PlayerPosition);
        Debug.Log("Init 3");

        Debug.Log($"LevelManager: move pawn");
        _player.DOMove(startTile.transform.position, _playerTransitionDur).OnComplete(() => {
            StateManager.Instance.ChangeState<PlayerTurnState>();
            _player.GetComponent<APawn>().PawnStats.StartTurn();
        }); 


        _currentLevelIndex += 1;

        _previousLevel.GetComponent<LevelInfo>().DestroyLevel();
    }
}