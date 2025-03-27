using UnityEngine;

[CreateAssetMenu(fileName = "GridSpritesSO", menuName = "Scriptable Objects/newGridSprites")]
public class GridSpritesSO : ScriptableObject
{
    [SerializeField] private Sprite _tileSprite;
    [SerializeField] private Sprite _moveHighlighted;
    [SerializeField] private Sprite _attackHighlighted;

    [SerializeField] private float _animDuration;
    [SerializeField] private float _targetAlfa;

    public Sprite TileSprite => _tileSprite;
    public Sprite MoveHighlighted => _moveHighlighted;
    public Sprite AttackHighlighted => _attackHighlighted;
    public float AnimDuration => _animDuration;
    public float TargetAlfa => _targetAlfa;
}
