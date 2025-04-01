using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSettingsSO", menuName = "Scriptable Objects/GridSettingsSO")]
public class GridSettingsSO : ScriptableObject
{
    [SerializeField] private int _lineDestroyFrequency;

    [SerializeField] private int _linesUpToPlayer;
    [SerializeField] private int _linesDownToPlayer;

    [SerializeField] private int _width;

    [SerializeField] private Vector2Int _playerStartPosition;

    [SerializeField] private GameObject _tilePrefab;

    public int LineDestroyFrequency => _lineDestroyFrequency;
    public int LinesUpToPlayer => _linesUpToPlayer;
    public int LinesDownToPlayer => _linesDownToPlayer;
    public int Width => _width;
    public Vector2Int PlayerStartPosition => _playerStartPosition;
    public GameObject TilePrefab => _tilePrefab;
}
