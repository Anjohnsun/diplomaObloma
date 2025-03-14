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
    private Vector2Int _playerStartPosition;

    private GameObject _tilePrefab;
    private TileGenerator _tileGenerator;

    private IPawnMoverService _moverService;

    private Dictionary<Vector2Int, Tile> _tiles;

    public GridManager([Inject(Id = "lineDestroyFrequency")] int lineDestroyFrequency,
        [Inject(Id = "linesUpToPlayer")] int linesUpToPlayer,
        [Inject(Id = "linesDownToPlayer")] int linesDownToPlayer,
        [Inject(Id = "width")] int width,
        [Inject(Id = "playerPawn")] Pawn playerPawn,
        [Inject(Id = "playerStartPosition")] Vector2Int playerStartPosition,
        [Inject(Id = "tilePrefab")] GameObject tilePrefab)
    {
        _lineDestroyFrequency = lineDestroyFrequency;
        _linesUpToPlayer = linesUpToPlayer;
        _linesDownToPlayer = linesDownToPlayer;
        _width = width;
        _playerStartPosition = playerStartPosition;
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

    public Tile GetTileAt(Vector2Int point)
    {
        if (!_tiles.TryGetValue(point, out Tile tile))
        {
            throw new Exception("Try to get tile out of bounds");
        }
        return tile;
    }

    public void MovePawnTo(Pawn pawn, Vector2Int newPosition)
    {
        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).IsFree = true;
        _moverService.GridMoveTo(pawn, newPosition);
        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).IsFree = false;
    }

    void OnAvailableTileClicked()
    {
        throw new NotImplementedException();
    }

    public bool IsWithinBounds(Vector2Int point)
    {
        return _tiles.TryGetValue(point, out Tile tile);
    }

    public bool IsTileFree(Vector2Int point)
    {
        if (!_tiles.TryGetValue(point, out Tile tile))
        {
            throw new Exception("Try to get tile out of bounds");
        }
        return tile.IsFree;
    }

    public void HighlightTileGroup(List<Vector2Int> points)
    {
        foreach (Vector2Int point in points)
        {
            _tiles[point].Highlight(true);
        }
    }

    public void DehighlightAllTiles()
    {
        foreach (var tile in _tiles.Values)
        {
            tile.Highlight(false);
        }
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