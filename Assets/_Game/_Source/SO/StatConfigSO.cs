using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatConfig", menuName = "SO/StatConfig")]
public class StatConfigSO : ScriptableObject
{
    [SerializeField] private List<StatLevelConf> _levels;

    public List<StatLevelConf> Levels => _levels;

}

[Serializable]
public class StatLevelConf
{
    [SerializeField] private int _XPCost;
    [SerializeField] private int _value;

    public int XPCost => _XPCost;
    public int Value => _value;
}
