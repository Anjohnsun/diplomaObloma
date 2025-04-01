using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridManager
{
    private int _heightReached;
    private int _lineDestroyFrequency;
    private int _turnsToDestroy;

    private int _linesUpToPlayer;
    private int _linesDownToPlayer;

    private int _width;

    private Pawn _playerPawn;
    private Vector2Int _playerStartPosition;

    private GameObject _tilePrefab;
    private TileGenerator _tileGenerator;

    private IPawnMoverService _moverService;

    private Dictionary<Vector2Int, Tile> _tiles;
    private MonoBehaviour _coroutines;

    public GridManager(GridSettingsSO gridSettings,
        [Inject(Id = "playerPawn")] Pawn playerPawn,
        IPawnMoverService moverService,
        MonoBehaviour coroutines)
    {
        _lineDestroyFrequency = gridSettings.LineDestroyFrequency;
        _linesUpToPlayer = gridSettings.LinesUpToPlayer;
        _linesDownToPlayer = gridSettings.LinesDownToPlayer;
        _width = gridSettings.Width;
        _playerStartPosition = gridSettings.PlayerStartPosition;
        _tilePrefab = gridSettings.TilePrefab;
        _playerPawn = playerPawn;
        _coroutines = coroutines;

        _tileGenerator = new GameObject("TileGenerator").AddComponent<TileGenerator>();
        _tileGenerator.enabled = true;

        _tileGenerator.Construct(_width, _tilePrefab, this);

        _moverService = moverService;

        _tiles = new Dictionary<Vector2Int, Tile>();
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
        _coroutines.StartCoroutine(MovePawnToCor(pawn, newPosition));

        if (pawn.Equals(_playerPawn))
        {
            /*if(newPosition.y >= _heightReached - _linesUpToPlayer)
            {
                while (newPosition.y >= _heightReached - _linesUpToPlayer)
                {
                    _tileGenerator.CreateLine(_heightReached + 1);
                }
            }*/
            Debug.Log($"PLAYER HEIGHT: {_heightReached}");
            if (newPosition.y > _heightReached)
            {
                for (int i = 0; i < newPosition.y - _heightReached; i++)
                {
                    _tileGenerator.CreateLine(_heightReached + _linesUpToPlayer + 1);
                }
                _heightReached = newPosition.y;
            }

        }
    }

    private IEnumerator MovePawnToCor(Pawn pawn, Vector2Int newPosition)
    {
        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).IsFree = true;
        _moverService.GridMoveTo(pawn, newPosition);

        yield return new WaitForSeconds(_moverService.GridDuration);

        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).IsFree = false;
    }

    public void SetPlayerPawnToStartPosition()
    {
        _moverService.AppearMoveTo(_playerPawn, _playerStartPosition);
        _heightReached = Mathf.RoundToInt(_playerStartPosition.y);
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
            _tiles[point].Highlight();
        }
    }

    public void DehighlightAllTiles()
    {
        foreach (var tile in _tiles.Values)
        {
            tile.Dehighlight();
        }
    }

    public void AddNewTiles(List<Tile> tiles)
    {
        foreach (Tile t in tiles)
        {
            _tiles.Add(Vector2Int.CeilToInt(new Vector2(t.transform.position.x, t.transform.position.y)), t);
        }
    }

    public void UpdateTimer()
    {

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