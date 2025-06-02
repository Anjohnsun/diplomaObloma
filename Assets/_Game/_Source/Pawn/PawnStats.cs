using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PawnStats : IPawnStats
{
    public StatConfigSO HPConfig { get; private set; }
    public StatConfigSO APConfig { get; private set; }
    public StatConfigSO STRConfig { get; private set; }
    public StatConfigSO ARMConfig { get; private set; }

    public int EXP { get; private set; }

    public int CurrentHP { get; private set; }
    public int CurrentAP { get; private set; }

    public int MaxHP => HPConfig.Levels[HPLevel].Value;
    public int MaxAP => APConfig.Levels[APLevel].Value;
    public int STR => STRConfig.Levels[STRLevel].Value;
    public int ARM => ARMConfig.Levels[ARMLevel].Value;

    public int HPLevel { get; private set; }
    public int APLevel { get; private set; }
    public int STRLevel { get; private set; }
    public int ARMLevel { get; private set; }

    public event Action<int> OnDamageTaken;
    public event Action OnDeath;
    public event Action OnStatsChanged;

    public PawnStats(
        StatConfigSO hpConfig, int hpLevel,
        StatConfigSO apConfig, int apLevel,
        StatConfigSO strConfig, int strLevel,
        StatConfigSO armConfig, int armLevel)
    {
        HPConfig = hpConfig;
        APConfig = apConfig;
        STRConfig = strConfig;
        ARMConfig = armConfig;

        HPLevel = hpLevel;
        APLevel = apLevel;
        STRLevel = strLevel;
        ARMLevel = armLevel;

        ResetStats();
        OnStatsChanged += () => Debug.Log("Stats Updated");
        OnStatsChanged.Invoke();
    }

    private void ResetStats()
    {
        CurrentHP = MaxHP;
        CurrentAP = MaxAP;
    }

    public void TakeDamage(int damage, bool isBonusDamage)
    {
        int remainingDamage = damage - ARM;

        if (remainingDamage > 0)
        {
            CurrentHP = Mathf.Max(CurrentHP - remainingDamage, 0);
            OnDamageTaken?.Invoke(CurrentHP);

            if (CurrentHP <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        OnStatsChanged.Invoke();
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public void UseAP(int amount = 1)
    {
        CurrentAP = Mathf.Max(CurrentAP - amount, 0);
        OnStatsChanged.Invoke();
    }

    public void ResetAP()
    {
        CurrentAP = MaxAP;
        OnStatsChanged.Invoke();
    }

    public void GetEXP(int value)
    {
        EXP += value;
        OnStatsChanged.Invoke();
    }

    public void StartTurn()
    {
        CurrentAP = MaxAP;
        OnStatsChanged.Invoke();
    }

    public bool UpgradeStat(StatType statType)
    {
        StatConfigSO config = null;
        int currentLevel = 0;
        int xpCost = 0;

        Debug.Log($"{statType.ToString()}, currentLevel: {APLevel}, maxLevel: {APConfig.Levels.Count}");

        switch (statType)
        {
            case StatType.HP:
                config = HPConfig;
                currentLevel = HPLevel;
                break;
            case StatType.AP:
                config = APConfig;
                currentLevel = APLevel;
                break;
            case StatType.STR:
                config = STRConfig;
                currentLevel = STRLevel;
                break;
            case StatType.ARM:
                config = ARMConfig;
                currentLevel = ARMLevel;
                break;
        }

        if (currentLevel >= config.Levels.Count - 1)
            return false;

        xpCost = config.Levels[currentLevel + 1].XPCost;

        if (EXP < xpCost)
            return false;

        EXP -= xpCost;

        switch (statType)
        {
            case StatType.HP:
                HPLevel++;
                CurrentHP = MaxHP;
                break;
            case StatType.AP:
                APLevel++;
                CurrentAP = MaxAP;
                break;
            case StatType.STR:
                STRLevel++;
                break;
            case StatType.ARM:
                ARMLevel++;
                break;
        }

        OnStatsChanged.Invoke();
        return true;
    }
}
