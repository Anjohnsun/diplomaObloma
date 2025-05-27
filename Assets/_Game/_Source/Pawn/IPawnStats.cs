using System;
using UnityEngine;

public interface IPawnStats
{
    int EXP { get; }

    int CurrentHP { get; set; }
    int CurrentAP { get; set; }

    int MaxHP { get; }
    int MaxAP { get; }
    int STR { get; }
    int ARM { get; }

    StatConfigSO HPConfig { get; }
    StatConfigSO APConfig { get; }
    StatConfigSO STRConfig { get; }
    StatConfigSO ARMConfig { get; }

    int HPLevel { get; }
    int APLevel { get; }
    int STRLevel { get; }
    int ARMLevel { get; }

    event Action<int> OnDamageTaken;
    event Action OnDeath;
    event Action<int, int, int, int> OnStatsChanged;

    void TakeDamage(int damage);
    void Heal(int amount);
    void UseAP(int amount = 1);
    void ResetAP();
    void AddEXP (int value);
    void StartNewTurn();
    bool LevelUpStat(StatType statType);
}

public enum StatType
{
    AP,
    HP,
    ARM,
    STR
}