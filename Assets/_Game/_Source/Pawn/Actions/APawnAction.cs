using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class APawnAction
{
    protected APawn _owner;
    protected float _duration;

    protected int _usesNumber;
    protected List<FieldTile> _possibleMoves;
    public abstract Sprite ActionIcon { get; }
    public abstract MarkerType Marker { get; }

    protected string _hint;
    public int UsesNumber => _usesNumber;
    public string Hint => _hint;

    public APawnAction(APawn owner, float duration, int usesNumber)
    {
        _owner = owner;
        _duration = duration;
        _usesNumber = usesNumber;
    }

    public abstract List<FieldTile> CalculateTargets();
    public virtual void Perform(FieldTile tile, Action handler)
    {
        _usesNumber--;
    }
    public abstract void CanPerform(Vector2Int tile);
    public abstract bool SelfPerform(Action handler);
}
