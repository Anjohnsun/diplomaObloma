using UnityEngine;

public class PawnStatsDecorator : IPawnStats
{
    protected IPawnStats _wrappedStats;
    protected int _lifeTime;

    public virtual int MaxHealth => _wrappedStats.MaxHealth;
    public virtual int CurrentHealth => _wrappedStats.CurrentHealth;
    public virtual int MaxActionPoints => _wrappedStats.MaxActionPoints;
    public virtual int ActionPointsLeft => _wrappedStats.ActionPointsLeft;
    public int LifeTime => _lifeTime;
    
    public PawnStatsDecorator(int lifeTime)
    {
        _lifeTime = lifeTime;
    }

    public void Construct(IPawnStats wrappedStats)
    {
        _wrappedStats = wrappedStats;
    }

    public virtual void IncreaseHealth(int amount)
    {
        _wrappedStats.IncreaseHealth(amount);
    }

    public virtual void TakeDamage(int damage)
    {
        _wrappedStats.TakeDamage(damage);
    }

    public virtual void UseAction()
    {
        _wrappedStats.UseAction();
    }

    public void StartTurnUpdate()
    {
        _lifeTime--;
        _wrappedStats?.StartTurnUpdate();
    }
}
