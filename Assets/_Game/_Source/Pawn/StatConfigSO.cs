using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatConfig", menuName = "SO/StatConfig")]
public class StatConfigSO : ScriptableObject
{
    [SerializeField] private List<StatLevel> _levels;
    public List<StatLevel> Levels => _levels;
}

[Serializable]
public class StatLevel
{
    [SerializeField] private int _xpCost;
    [SerializeField] private int _value;

    public int XPCost => _xpCost;
    public int Value => _value;
}
