using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridManager
{
    private int _heightReached;
    private int _lineDestroyFrequency;

    private int _linesUpToPlayer;
    private int _linesDownToPlayer;

    private int _width;

    private Pawn _playerPawn;
    private GameObject _tilePrefab;

    private TileGenerator _tileGenerator;

    public GridManager( [Inject(Id = "lineDestroyFrequency")] int lineDestroyFrequency,
        [Inject(Id = "linesUpToPlayer")] int linesUpToPlayer,
        [Inject(Id = "linesDownToPlayer")] int linesDownToPlayer,
        [Inject(Id = "width")] int width, 
        Pawn playerPawn,
        [Inject(Id = "tilePrefab")] GameObject tilePrefab)
    {
        _lineDestroyFrequency = lineDestroyFrequency;
        _linesUpToPlayer = linesUpToPlayer;
        _linesDownToPlayer = linesDownToPlayer;
        _width = width;
        _playerPawn = playerPawn;
        _tilePrefab = tilePrefab;

        _tileGenerator = new GameObject("TileGenerator").AddComponent<TileGenerator>();
        _tileGenerator.enabled = true;

        _tileGenerator.Construct(_width, _tilePrefab);
    }

    public void CreateStartLines()
    {
        _tileGenerator.CreateStartLines(_linesUpToPlayer + _linesDownToPlayer + 1);
    }

    void CreateNewLines()
    {

    }

    void MovePawn(Transform pawn, Vector3Int to, bool isPlayer)
    {
        //Change to pawnMoverService();
        pawn.localPosition = to;
    }

    void OnAvailableTileClicked()
    {
        throw new NotImplementedException();
    }

    bool IsBetweenBound()
    {
        throw new NotImplementedException();
    }

    void IsTileFree()
    {
        throw new NotImplementedException();
    }
}


/*    private Dictionary<Vector2, Tile> _tiles;

    private int _currentHeight;
    private int _width;

    private TileGenerator _generator;
    private GameObject _tilePrefab;
    
    public GridManager(int width, GameObject tilePrefab)
    {
        _width = width;
        _tilePrefab = tilePrefab;

        _generator = GameObject.Instantiate(new GameObject()).AddComponent<TileGenerator>();
        _generator.Init(_width, _tilePrefab);
    }

    public void OnAvailableTileClicked()
    {

    }

    public void CreateField(Action OnCreated)
    {
        //where to set height????
        _generator.GenerateStartField(8, OnCreated);
    }


    internal bool IsWithinBounds(Vector2Int position)
    {
        Debug.Log($"Проверка позиции {position}, нынешняя высота {_currentHeight}, ширина {_width}");
        return position.x >= -_width / 2 && position.x <= _width / 2 &&
            position.y >= _currentHeight - _width / 2 && position.y <= _currentHeight + _width / 2;
    }*/