using System;
using UnityEngine;

public interface IPawnStats
{
    public int CurrentHP { get; }
    public int CurrentARM { get; }
    public int CurrentAP { get; }
    StatLevel HP { get; }
    StatLevel AP { get; }
    StatLevel STR { get; }
    StatLevel SPD { get; }
    StatLevel ARM { get; }

    int HP_VALUE { get; }
    int AP_VALUE { get; }
    int STR_VALUE { get; }
    int SPD_VALUE { get; }
    int ARM_VALUE { get; }

    void TakeDamage(int damage);
    void IncreaseHealth(int amount);
    void UseAction();
    void StartTurnUpdate();

    Action<int> OnGetDamage { get; set; }
}

public class StatLevel
{
    public StatConfigSO StatConfig;
    public int Level;

    public StatLevel(StatConfigSO statConfig, int level)
    {
        StatConfig = statConfig;
        Level = level;
    }
}