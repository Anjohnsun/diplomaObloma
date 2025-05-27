using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PawnStats : IPawnStats
{
    private readonly StatConfigSO _hpConfig;
    private readonly StatConfigSO _apConfig;
    private readonly StatConfigSO _strConfig;
    private readonly StatConfigSO _armConfig;

    public int EXP { get; private set; }

    public int HPLevel { get; private set; }
    public int APLevel { get; private set; }
    public int STRLevel { get; private set; }
    public int ARMLevel { get; private set; }

    public int CurrentHP { get; set; }
    public int CurrentAP { get; set; }

    public int MaxHP => _hpConfig.Levels[HPLevel].Value;
    public int MaxAP => _apConfig.Levels[APLevel].Value;
    public int STR => _strConfig.Levels[STRLevel].Value;
    public int ARM => _armConfig.Levels[ARMLevel].Value;

    public StatConfigSO HPConfig => _hpConfig;
    public StatConfigSO APConfig => _apConfig;
    public StatConfigSO STRConfig => _strConfig;
    public StatConfigSO ARMConfig => _armConfig;

    public event Action<int> OnDamageTaken;
    public event Action OnDeath;
    public event Action<int, int, int, int> OnStatsChanged;

    public PawnStats(
        StatConfigSO hpConfig, int hpLevel,
        StatConfigSO apConfig, int apLevel,
        StatConfigSO strConfig, int strLevel,
        StatConfigSO armConfig, int armLevel)
    {
        _hpConfig = hpConfig;
        _apConfig = apConfig;
        _strConfig = strConfig;
        _armConfig = armConfig;

        HPLevel = hpLevel;
        APLevel = apLevel;
        STRLevel = strLevel;
        ARMLevel = armLevel;

        ResetStats();
        OnStatsChanged += (int a, int f, int s, int r) => Mathf.Abs(0.4f);
        OnStatsChanged.Invoke(CurrentHP, CurrentAP, STR, ARM);
    }

    private void ResetStats()
    {
        CurrentHP = MaxHP;
        CurrentAP = MaxAP;
    }

    public void TakeDamage(int damage)
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

        OnStatsChanged.Invoke(CurrentHP, CurrentAP, STR, ARM);
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public void UseAP(int amount = 1)
    {
        CurrentAP = Mathf.Max(CurrentAP - amount, 0);
        OnStatsChanged.Invoke(CurrentHP, CurrentAP, STR, ARM);
    }

    public void StartNewTurn()
    {
        CurrentAP = MaxAP;
        OnStatsChanged.Invoke(CurrentHP, CurrentAP, STR, ARM);
    }

    public bool LevelUpStat(StatType statType)
    {
        StatConfigSO config = null;
        int currentLevel = 0;
        int xpCost = 0;

        Debug.Log($"{statType.ToString()}, currentLevel: {APLevel}, maxLevel: {APConfig.Levels.Count}");

        switch (statType)
        {
            case StatType.HP:
                config = _hpConfig;
                currentLevel = HPLevel;
                break;
            case StatType.AP:
                config = _apConfig;
                currentLevel = APLevel;
                break;
            case StatType.STR:
                config = _strConfig;
                currentLevel = STRLevel;
                break;
            case StatType.ARM:
                config = _armConfig;
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

        OnStatsChanged.Invoke(CurrentHP, CurrentAP, STR, ARM);
        return true;
    }

    public void AddEXP(int value)
    {
        EXP += value;
    }

    public void ResetAP()
    {
        CurrentAP = MaxAP;
        OnStatsChanged.Invoke(CurrentHP, CurrentAP, STR, ARM);
    }
}
