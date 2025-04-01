using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    //заменить на хранение объекта(пешки, препятствия(?)), занимающего клетку
    public bool IsFree;

    [SerializeField] private SpriteRenderer _YELLOWtile;

    private float _animDuration;
    private float _targetAlfa = 0.7f;

    private void Start()
    {
        IsFree = true;
        _YELLOWtile.color = new Color(_YELLOWtile.color.r, _YELLOWtile.color.g, _YELLOWtile.color.b, 0);
    }

    public void Construct()
    {

    }

    public void Highlight()
    {
        DOTween.Kill(transform);

        _YELLOWtile.DOColor(new Color(_YELLOWtile.color.r, _YELLOWtile.color.g, _YELLOWtile.color.b, _targetAlfa),
             _animDuration);

    }

    public void Dehighlight()
    {
        DOTween.Kill(transform);

        _YELLOWtile.DOColor(new Color(_YELLOWtile.color.r, _YELLOWtile.color.g, _YELLOWtile.color.b, 0),
                 _animDuration);
    }
}
