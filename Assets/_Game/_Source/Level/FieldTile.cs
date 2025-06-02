using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FieldTile : MonoBehaviour
{
    //заменить на хранение объекта(пешки, препятствия(?)), занимающего клетку

    [SerializeField] private SpriteRenderer _attackMarker;
    [SerializeField] private SpriteRenderer _interactMarker;

    [SerializeField] private float _animDuration = 0.3f;
    [SerializeField] private float _targetAlfa = 0.7f;
    public APawn Pawn { get; private set; }

    public void SetNewPawn(APawn pawn)
    {
        if (Pawn != null)
            throw new System.Exception("Pawn substitution attempt");
        else
            Pawn = pawn;
    }

    public APawn RemovePawn()
    {
        APawn pawn = Pawn;
        Pawn = null;
        return pawn;
    }

    public void Construct()
    {
        _interactMarker.color = new Color(_interactMarker.color.r, _interactMarker.color.g, _interactMarker.color.b, 0);
        _attackMarker.color = new Color(_attackMarker.color.r, _attackMarker.color.g, _attackMarker.color.b, 0);
    }

    public void Mark(MarkerType markerType)
    {
        DOTween.Kill(transform);

        switch (markerType)
        {
            case (MarkerType.interact):
                {
                    _interactMarker.DOColor(new Color(_interactMarker.color.r, _interactMarker.color.g, _interactMarker.color.b, _targetAlfa),
             _animDuration);
                    _attackMarker.color = new Color(_attackMarker.color.r, _attackMarker.color.g, _attackMarker.color.b, 0);
                    break;
                }

            case (MarkerType.attack):
                {
                    _attackMarker.DOColor(new Color(_attackMarker.color.r, _attackMarker.color.g, _attackMarker.color.b, _targetAlfa),
             _animDuration);
                    _interactMarker.color = new Color(_interactMarker.color.r, _interactMarker.color.g, _interactMarker.color.b, 0);
                    break;
                }
        }

    }

    public void Demark()
    {
        DOTween.Kill(transform);

        _interactMarker.DOColor(new Color(_interactMarker.color.r, _interactMarker.color.g, _interactMarker.color.b, 0), _animDuration);
        _attackMarker.DOColor(new Color(_attackMarker.color.r, _attackMarker.color.g, _attackMarker.color.b, 0), _animDuration);
    }
}

public enum MarkerType
{
    interact,
    attack
}
