using UnityEngine;

public interface IPawnStats
{
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int MaxActionPoints { get; }
    int ActionPointsLeft { get; }

    void TakeDamage(int damage);
    void IncreaseHealth(int amount);
    void UseAction();
    void StartTurnUpdate();
}
