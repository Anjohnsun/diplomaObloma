using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    //заменить на хранение объекта(пешки, препятствия(?)), занимающего клетку
    public bool IsFree;

    [SerializeField] private SpriteRenderer _tile;
    [SerializeField] private SpriteRenderer _moveHighlight;
    [SerializeField] private SpriteRenderer _attackHighlight;
    private float _animDuration;
    private float _targetAlfa;

    private void Start()
    {
        IsFree = true;
        _moveHighlight.color = new Color(_moveHighlight.color.r, _moveHighlight.color.g, _moveHighlight.color.b, 0);
        _attackHighlight.color = new Color(_attackHighlight.color.r, _attackHighlight.color.g, _attackHighlight.color.b, 0);
    }

    public void Construct(GridSpritesSO sprites)
    {
        _tile.sprite = sprites.TileSprite;
        _moveHighlight.sprite = sprites.MoveHighlighted;
        _attackHighlight.sprite = sprites.AttackHighlighted;

        _animDuration = sprites.AnimDuration;
    }

    public void Highlight(TileHighlightType t)
    {
        DOTween.Kill(transform);
        if (t == TileHighlightType.move)
        {
            _moveHighlight.DOColor(new Color(_moveHighlight.color.r, _moveHighlight.color.g, _moveHighlight.color.b, _targetAlfa),
                 _animDuration);
            _attackHighlight.DOColor(new Color(_attackHighlight.color.r, _attackHighlight.color.g, _attackHighlight.color.b, 0),
                 _animDuration);
        }
        else if(t == TileHighlightType.attack)
        {
            _attackHighlight.DOColor(new Color(_attackHighlight.color.r, _attackHighlight.color.g, _attackHighlight.color.b, _targetAlfa),
                 _animDuration);
            _moveHighlight.DOColor(new Color(_moveHighlight.color.r, _moveHighlight.color.g, _moveHighlight.color.b, 0),
                 _animDuration);
        }
    }

    public void Dehighlight()
    {
        _attackHighlight.DOColor(new Color(_attackHighlight.color.r, _attackHighlight.color.g, _attackHighlight.color.b, 0),
                 _animDuration);
        _moveHighlight.DOColor(new Color(_moveHighlight.color.r, _moveHighlight.color.g, _moveHighlight.color.b, 0),
                 _animDuration);
    }
}
