using System;
using UnityEngine;

public interface IPawnStats
{
    StatConfigSO HPConfig { get; }
    StatConfigSO APConfig { get; }
    StatConfigSO STRConfig { get; }
    StatConfigSO ARMConfig { get; }

    int EXP { get; }

    int CurrentHP { get;}
    int CurrentAP { get;}

    int MaxHP { get; }
    int MaxAP { get; }
    int STR { get; }
    int ARM { get; }


    int HPLevel { get; }
    int APLevel { get; }
    int STRLevel { get; }
    int ARMLevel { get; }

    event Action<int> OnDamageTaken;
    event Action OnDeath;
    event Action OnStatsChanged;

    void TakeDamage(int damage, bool isBonusDamage = false);
    void Heal(int amount);
    void UseAP(int amount = 1);
    void ResetAP();
    void GetEXP (int value);
    void StartTurn();
    bool UpgradeStat(StatType statType);
}

public enum StatType
{
    AP,
    HP,
    ARM,
    STR
}