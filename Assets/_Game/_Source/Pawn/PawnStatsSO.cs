using UnityEngine;

[CreateAssetMenu(fileName = "PawnStatsSO", menuName = "Scriptable Objects/newPawnStats")]
public class PawnStatsSO : ScriptableObject
{
    [SerializeField] private PawnType _pawnType;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxActionPoints;
    [SerializeField] private int _actionPointsLeft;

    public PawnType PawnType => _pawnType;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int MaxActionPoints => _maxActionPoints;
    public int ActionPointsLeft => _actionPointsLeft;

}
