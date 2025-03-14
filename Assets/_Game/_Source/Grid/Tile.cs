using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    //заменить на хранение объекта(пешки, препятствия(?)), занимающего клетку
    public bool IsFree;

    [SerializeField] private SpriteRenderer _highlightedTile;
    [SerializeField] private float _highlightAnimDuration;

    private void Start()
    {
        IsFree = true;
        _highlightedTile.color = new Color(_highlightedTile.color.r, _highlightedTile.color.g, _highlightedTile.color.b, 0);
    }

    public void Highlight(bool v)
    {
        if (v)
        {
            _highlightedTile.DOColor(new Color(_highlightedTile.color.r, _highlightedTile.color.g, _highlightedTile.color.b, 1), 
                _highlightAnimDuration);
        } else
        {
            _highlightedTile.DOColor(new Color(_highlightedTile.color.r, _highlightedTile.color.g, _highlightedTile.color.b, 0),
                _highlightAnimDuration);
        }
    }
}
