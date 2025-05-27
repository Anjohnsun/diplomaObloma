using UnityEngine;

public class ActionButtonHinter : MonoBehaviour, IHintUser
{
    [SerializeField] private string _hint;
    public string GetHintText()
    {
        return _hint;
    }
}
