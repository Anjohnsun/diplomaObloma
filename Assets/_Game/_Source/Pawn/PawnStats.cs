using UnityEngine;

public class PawnStats : IPawnStats
{
    private int _maxHealth;
    private int _currentHealth;
    private int _maxActionPoints;
    private int _actionPointsLeft;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int MaxActionPoints => MaxActionPoints;
    public int ActionPointsLeft => _actionPointsLeft;

    public PawnStats(int maxHealth, int currentHealth, int maxActionPoints, int actionPointsLeft)
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _maxActionPoints = maxActionPoints;
        _actionPointsLeft = actionPointsLeft;
    }
    

    public void UseAction()
    {
        _actionPointsLeft--;
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth += amount;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            throw new System.Exception("HANDLE ZERO HP");
        }
    }
}
