using DG.Tweening;
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

    private TileGenerator _tileGenerator;

    private IPawnMoverService _moverService;
    private EnemyManager _enemyManager;

    private Dictionary<Vector2Int, Tile> _tiles;
    private MonoBehaviour _coroutines;

    private Transform _camera;

    public GridManager(GridSettingsSO gridSettings,
        [Inject(Id = "playerPawn")] Pawn playerPawn,
        IPawnMoverService moverService,
        MonoBehaviour coroutines,
        EnemyManager enemyManager,
        [Inject(Id = "cameraTransform")] Transform camera)
    {
        _lineDestroyFrequency = gridSettings.LineDestroyFrequency;
        _linesUpToPlayer = gridSettings.LinesUpToPlayer;
        _linesDownToPlayer = gridSettings.LinesDownToPlayer;
        _width = gridSettings.Width;
        _playerStartPosition = gridSettings.PlayerStartPosition;
        _playerPawn = playerPawn;
        _coroutines = coroutines;

        _tileGenerator = new GameObject("TileGenerator").AddComponent<TileGenerator>();
        _tileGenerator.enabled = true;

        _tileGenerator.Construct(_width, gridSettings.TilePrefab, this);

        _moverService = moverService;

        _tiles = new Dictionary<Vector2Int, Tile>();
        _enemyManager = enemyManager;

        _camera = camera;
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
        Debug.Log(_heightReached + " - reached height");
        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).IsFree = true;
        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).Pawn = null;
        _moverService.GridMoveTo(pawn, newPosition);

        yield return new WaitForSeconds(_moverService.GridDuration);

        if (pawn.Equals(_playerPawn))
        {
            if (_camera.position.y < newPosition.y)
            {
                Debug.Log("MoveCamera");
                _camera.DOMoveY(newPosition.y, 0.3f);
            }
        }

        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).IsFree = false;
        GetTileAt(Vector2Int.CeilToInt(pawn.transform.position)).Pawn = pawn;
    }

    public void SetPlayerPawnToStartPosition()
    {
        _moverService.AppearMoveTo(_playerPawn, _playerStartPosition);
        _heightReached = Mathf.RoundToInt(_playerStartPosition.y);
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
        _turnsToDestroy--;
        if (_turnsToDestroy == 0)
        {
            DestroyLine();
            _turnsToDestroy = _lineDestroyFrequency;
        }
    }

    private void DestroyLine()
    {
        throw new NotImplementedException();
    }

    private void AddEnemy()
    {

    }
}