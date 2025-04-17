using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private FieldTile[,] _grid;
    private int _horizontalSize;
    private int _verticalSize;
    private Transform _levelTransform;
    public static GridManager Instance { get; private set; }

    public static Predicate<FieldTile> IsFreeTile = tile => tile.Pawn == null;
    public static Predicate<FieldTile> HasAnyPawn = tile => tile.Pawn != null;

    public GridManager()
    {
        if (Instance == null) Instance = this;
    }

    public void InitializeGrid(FieldTile[,] grid, Transform levelTransform)
    {
        _grid = grid;
        _horizontalSize = grid.GetLength(0);
        _verticalSize = grid.GetLength(1);
        _levelTransform = levelTransform;
    }

    public FieldTile WorldPositionToTile(Vector2 worldPosition)
    {
        return GetTileAtGridPosition(WorldToGridPosition(worldPosition));
    }

    private Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        Vector2 localPos = _levelTransform.InverseTransformPoint(worldPosition);
        return new Vector2Int(
            Mathf.RoundToInt(localPos.x + _grid.GetLength(0)/2),
            Mathf.RoundToInt(localPos.y)
        );
    }

    public FieldTile GetTileAtGridPosition(Vector2Int gridPosition)
    {
        return IsPositionValid(gridPosition) ? _grid[gridPosition.x, gridPosition.y] : null;
    }

    public List<FieldTile> GetAvailableTargets(List<Vector2Int> targetPositions, Predicate<FieldTile> condition)
    {
        List<FieldTile> availableTargets = new List<FieldTile>();
        
        foreach (var pos in targetPositions)
        {
            if (IsPositionValid(pos) && condition.Invoke(_grid[pos.x, pos.y]))
            {
                availableTargets.Add(_grid[pos.x, pos.y]);
            }
        }
        return availableTargets;
    }

    private bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < _horizontalSize && 
               position.y >= 0 && position.y < _verticalSize;
    }

    public void MovePawn(Pawn pawn, FieldTile tile)
    {
        Vector2Int newPos = GetTileCoordinates(tile);
        if (newPos.x < 0 || newPos.y < 0) return;

        _grid[pawn.GridPosition.x, pawn.GridPosition.y].RemovePawn();
        tile.SetNewPawn(pawn);
        pawn.UpdatePosition(newPos);
    }

    public Vector2Int GetTileCoordinates(FieldTile tile)
    {
        if (tile == null) return new Vector2Int(-1, -1);

        for (int x = 0; x < _horizontalSize; x++)
        {
            for (int y = 0; y < _verticalSize; y++)
            {
                if (_grid[x, y] == tile)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    public void MarkTiles(List<FieldTile> tiles, MarkerType markerType)
    {
        foreach (var tile in tiles)
        {
            if (tile != null) tile.Mark(markerType);
        }
    }

    public void DemarkTiles()
    {
        foreach (var tile in _grid)
        {
            if (tile != null) tile.Demark();
        }
    }
}