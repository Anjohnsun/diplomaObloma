using System;
using UnityEngine;

public abstract class AEffect : IPawnStats
{
    protected readonly IPawnStats _wrappedEntity;
    protected IPawnStats _nextEffect;
    protected int _duration;

    protected AEffect(IPawnStats wrappedEntity, int duration)
    {
        _wrappedEntity = wrappedEntity ?? throw new ArgumentNullException(nameof(wrappedEntity));
        _duration = duration;
        _nextEffect = null;
    }

    public void AddEffect(AEffect nextEffect)
    {
        if (_nextEffect == null)
            _nextEffect = nextEffect;
        else
            (_nextEffect as AEffect)?.AddEffect(nextEffect);
    }

    public virtual bool UpdateDuration()
    {
        _duration--;
        return _duration <= 0;
    }

    public virtual StatConfigSO HPConfig => _wrappedEntity.HPConfig;
    public virtual StatConfigSO APConfig => _wrappedEntity.APConfig;
    public virtual StatConfigSO STRConfig => _wrappedEntity.STRConfig;
    public virtual StatConfigSO ARMConfig => _wrappedEntity.ARMConfig;

    public virtual int EXP => _wrappedEntity.EXP;
    public virtual int CurrentHP => _wrappedEntity.CurrentHP;
    public virtual int CurrentAP => _wrappedEntity.CurrentAP;
    public virtual int MaxHP => _wrappedEntity.MaxHP;
    public virtual int MaxAP => _wrappedEntity.MaxAP;
    public virtual int STR => _wrappedEntity.STR;
    public virtual int ARM => _wrappedEntity.ARM;
    public virtual int HPLevel => _wrappedEntity.HPLevel;
    public virtual int APLevel => _wrappedEntity.APLevel;
    public virtual int STRLevel => _wrappedEntity.STRLevel;
    public virtual int ARMLevel => _wrappedEntity.ARMLevel;

    public event Action<int> OnDamageTaken
    {
        add => _wrappedEntity.OnDamageTaken += value;
        remove => _wrappedEntity.OnDamageTaken -= value;
    }

    public event Action OnDeath
    {
        add => _wrappedEntity.OnDeath += value;
        remove => _wrappedEntity.OnDeath -= value;
    }

    public event Action OnStatsChanged
    {
        add => _wrappedEntity.OnStatsChanged += value;
        remove => _wrappedEntity.OnStatsChanged -= value;
    }

    public virtual void GetEXP(int value) => _wrappedEntity.GetEXP(value);
    public virtual void Heal(int amount) => _wrappedEntity.Heal(amount);
    public virtual void ResetAP() => _wrappedEntity.ResetAP();
    public virtual void StartTurn()
    {
        UpdateDuration();
        _wrappedEntity.StartTurn();
    }

    public virtual void TakeDamage(int damage, bool isBonusDamage = false)
    {
        _wrappedEntity.TakeDamage(damage, isBonusDamage);
    }

    public virtual bool UpgradeStat(StatType statType) => _wrappedEntity.UpgradeStat(statType);
    public virtual void UseAP(int amount = 1) => _wrappedEntity.UseAP(amount);
}
