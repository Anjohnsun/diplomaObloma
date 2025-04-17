using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PawnStats : IPawnStats
{
    private int _currentHP;
    private int _currentARM;
    private int _currentAP;

    public int CurrentHP => _currentHP;
    public int CurrentARM => _currentARM;
    public int CurrentAP => _currentAP;

    public StatLevel HP { get; private set; }
    public StatLevel AP { get; private set; }
    public StatLevel STR { get; private set; }
    public StatLevel SPD { get; private set; }
    public StatLevel ARM { get; private set; }

    public int HP_VALUE => HP.StatConfig.Levels[HP.Level].Value;
    public int AP_VALUE => AP.StatConfig.Levels[AP.Level].Value;
    public int STR_VALUE => STR.StatConfig.Levels[STR.Level].Value;
    public int SPD_VALUE => SPD.StatConfig.Levels[SPD.Level].Value;
    public int ARM_VALUE => ARM.StatConfig.Levels[ARM.Level].Value;

    public Action<int> OnGetDamage { get; set; }

    public PawnStats(StatConfigSO HP, int hpLevel, StatConfigSO AP, int apLevel, StatConfigSO STR, int strLevel,
        StatConfigSO SPD, int spdLevel, StatConfigSO ARM, int armLevel)
    {
        this.HP = new StatLevel(HP, hpLevel);
        this.AP = new StatLevel(AP, apLevel);
        this.STR = new StatLevel(STR, strLevel);
        this.SPD = new StatLevel(SPD, spdLevel);
        this.ARM = new StatLevel(ARM, armLevel);

        _currentHP = HP.Levels[hpLevel].Value;
        _currentARM = ARM.Levels[armLevel].Value;
        _currentAP = AP.Levels[apLevel].Value;
    }


    public void UseAction()
    {
        if (_currentAP <= 0)
            throw new Exception("AP == ZERO");
        _currentAP--;
    }

    public void IncreaseHealth(int amount)
    {

    }

    public void TakeDamage(int damage)
    {
        if (damage > _currentARM)
        {
            _currentHP -= damage - _currentARM;
            _currentARM = 0;

            if (_currentHP <= 0)
            {

            }
        }
    }

    public void StartTurnUpdate()
    {
        _currentAP = AP_VALUE;

        if (_currentARM < ARM_VALUE)
        {
            _currentARM++;
        }
    }
}
