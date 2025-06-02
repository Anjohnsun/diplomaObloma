using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnConfig> _enemyConfigs;

    [SerializeField][Range(3, 5)] private int _enemyCount = 3;

    private FieldTile[,] _tiles;

    public List<APawn> SpawnEnemies(FieldTile[,] tiles, int level)
    {
        _tiles = tiles;
        List<APawn> enemies = new List<APawn>();

        var enemiesToSpawn = GetEnemiesForLevel(level);

        for (int i = 0; i < _enemyCount; i++)
        {

            var enemyPrefab = enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)];

            var tile = GetValidTile();
            var enemy = Instantiate(enemyPrefab, tile.transform.position, Quaternion.identity);
            tile.SetNewPawn(enemy);

            enemy.Construct(0,0,0,0);

            enemies.Add(enemy);
        }

        return enemies;
    }

    private List<APawn> GetEnemiesForLevel(int levelNumber)
    {
        return _enemyConfigs
            .Where(c => c.MinLevel <= levelNumber)
            .Select(c => c.Prefab)
            .ToList();
    }

    private FieldTile GetValidTile()
    {
        int width = _tiles.GetLength(0);
        int height = _tiles.GetLength(1);
        var validCoords = new List<FieldTile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                if (_tiles[x, y].Pawn == null)
                {
                    validCoords.Add(_tiles[x, y]);
                }
            }
        }

        if (validCoords.Count == 0)
        {
            Debug.LogError("No valid tiles");
            return null;
        }

        return validCoords[UnityEngine.Random.Range(0, validCoords.Count)];
    }
}

[Serializable]
public class EnemySpawnConfig
{
    public APawn Prefab;
    public int MinLevel;
}