using System;
using UnityEngine;
using System.Collections;
using UnityEditor.U2D.Aseprite;
using System.Collections.Generic;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _playerPosition;

    private FieldTile[,] _tiles;

    public Vector2Int PlayerPosition => _playerPosition;

    private TurnManager _turnManager;
    private List<Pawn> _pawns;

    public void GenerateLevel(int number)
    {
        if (_size.x < 7 || _size.x % 2 == 0)
            throw new Exception("Incorrect level size");

        _tiles = new FieldTile[_size.x, _size.y];

        switch (number)
        {
            case 0:
                StartCoroutine(GenerateFirstLevelCor());
                break;
            default:
                StartCoroutine(GenerateLevelCor());
                break;
        }
    }

    private IEnumerator GenerateFirstLevelCor()
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
        yield return null;
    }

    private IEnumerator GenerateLevelCor()
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
        yield return null;
    }

    public void StartLevelPassing(Pawn playerPawn, TurnManager turnManager)
    {

        _pawns = new List<Pawn>();
        playerPawn.UpdatePosition(_playerPosition);
        _pawns.Add(playerPawn);
        //create enemies

        turnManager.Initialize(_pawns);
    }


    public void InitLevel()
    {
        GridManager.Instance.InitializeGrid(_tiles, transform);
    }
}