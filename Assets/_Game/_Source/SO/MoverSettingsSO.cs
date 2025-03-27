using UnityEngine;

[CreateAssetMenu(fileName = "MoverSettingsSO", menuName = "Scriptable Objects/newMoverSettings")]
public class MoverSettingsSO : ScriptableObject
{
    [SerializeField] private AnimationCurve _gridMoveCurve;
    [SerializeField] private AnimationCurve _appearMoveCurve;

    [SerializeField] private float _duration;

    public AnimationCurve GridMoveCurve  => _gridMoveCurve; 
    public AnimationCurve AppearMoveCurve  => _appearMoveCurve; 
    public float Duration  => _duration;
}
