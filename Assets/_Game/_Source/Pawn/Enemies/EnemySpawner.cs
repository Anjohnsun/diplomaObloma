using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnConfig> _enemyConfigs;

    [SerializeField][Range(3, 5)] private int _enemyCount = 3;

    private FieldTile[,] _tiles;

    private List<APawn> GetEnemiesForLevel(int levelNumber)
    {
        return _enemyConfigs
            .Where(c => c.MinLevel <= levelNumber)
            .Select(c => c.Prefab)
            .ToList();
    }

    public List<AEnemyPawn> SpawnEnemies(FieldTile[,] tiles, int level)
    {
        _tiles = tiles;
        List<AEnemyPawn> enemies = new List<AEnemyPawn>();
        var enemiesToSpawn = GetEnemiesForLevel(level);

        for (int i = 0; i < _enemyCount; i++)
        {
            var enemyPrefab = enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)];

            Vector2Int gridCoords = GetValidTile();

            AEnemyPawn enemy = Instantiate(
                enemyPrefab,
                _tiles[gridCoords.x, gridCoords.y].transform.position,
                Quaternion.identity
            ).GetComponent<AEnemyPawn>();

            tiles[gridCoords.x, gridCoords.y].SetNewPawn(enemy);
            enemy.SetGridPosition(gridCoords);

            enemy.Construct(level);

            enemies.Add(enemy);
        }

        return enemies;
    }

    private Vector2Int GetValidTile()
    {
        int width = _tiles.GetLength(0);
        var validTiles = new List<FieldTile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < width; y++) 
            {
                if (_tiles[x, y].Pawn == null)
                {
                    validTiles.Add(_tiles[x, y]);
                }
            }
        }

        if (validTiles.Count == 0)
            throw new Exception("No valid tiles");

        return GetTileMatrixCoordinates(validTiles[UnityEngine.Random.Range(0, validTiles.Count)]);
    }
    private Vector2Int GetTileMatrixCoordinates(FieldTile tile)
    {
        for (int x = 0; x < _tiles.GetLength(0); x++)
        {
            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                if (_tiles[x, y] == tile)
                    return new Vector2Int(x, y);
            }
        }
        throw new Exception("Tile not found in matrix");
    }
}

[Serializable]
public class EnemySpawnConfig
{
    public APawn Prefab;
    public int MinLevel;
}