using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _playerPosition;

    private FieldTile[,] _tiles;

    public Vector2Int PlayerPosition => _playerPosition;

    private List<APawn> _pawns;
    private EnemySpawner _enemySpawner;
    private int _levelIndex;

    public void GenerateLevel(int number, EnemySpawner spawner, Action handler)
    {
        _enemySpawner = spawner;
        _levelIndex = number;

        if (_size.x < 7 || _size.x % 2 == 0)
            throw new Exception("Incorrect level size");

        _tiles = new FieldTile[_size.x, _size.y];


        switch (number)
        {
            case 0:
                StartCoroutine(GenerateFirstLevelCor(handler));
                break;
            default:
                StartCoroutine(GenerateLevelCor(handler));
                break;
        }
    }
    private IEnumerator GenerateFirstLevelCor(Action handler)
    {
        for (int visualRow = 0; visualRow < _size.y; visualRow++)
        {
            for (int column = 0; column < _size.x; column++)
            {
                Vector2 spawnPos = new Vector2(
                    transform.position.x - _size.x / 2 + column,
                    transform.position.y + visualRow
                );

                int matrixRow = visualRow;
                _tiles[column, matrixRow] = Instantiate(_tilePrefab, spawnPos, Quaternion.identity, transform).GetComponent<FieldTile>();
                _tiles[column, matrixRow].Construct();
            }
        }

        _pawns = new List<APawn>();
        handler.Invoke();
        yield return null;
    }
    private IEnumerator GenerateLevelCor(Action handler)
    {
        for (int visualRow = 0; visualRow < _size.y; visualRow++)
        {
            for (int column = 0; column < _size.x; column++)
            {
                Vector2 spawnPos = new Vector2(
                    transform.position.x - _size.x / 2 + column,
                    transform.position.y + visualRow
                );

                int matrixRow = visualRow;
                _tiles[column, matrixRow] = Instantiate(_tilePrefab, spawnPos, Quaternion.identity, transform).GetComponent<FieldTile>();
                _tiles[column, matrixRow].Construct();
            }
        }

        //_pawns = _enemySpawner.SpawnEnemies(_tiles, _levelIndex);

        //заглушка
        _pawns = new List<APawn>();

        handler.Invoke();
        yield return null;
    }
    public void InitLevel(EnemyManager enemyManager, APawn playerPawn)
    {
        GridManager.Instance.InitializeGrid(_tiles, transform);

        enemyManager.InitPawns(_pawns);

        _pawns = new List<APawn>();
        playerPawn.UpdateGridPosition(_playerPosition);
        Debug.Log($"Player pos: {_playerPosition.ToString()}");
    }
    public void InitLevel(APawn playerPawn)
    {
        GridManager.Instance.InitializeGrid(_tiles, transform);

        _pawns = new List<APawn>();
        playerPawn.UpdateGridPosition(_playerPosition);
        Debug.Log($"Player pos: {_playerPosition.ToString()}");
    }

    public void DestroyLevel()
    {
        StartCoroutine(DestroyLevelCor());
    }
    private IEnumerator DestroyLevelCor()
    {
        return null;
    }
}