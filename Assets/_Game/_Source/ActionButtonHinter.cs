using UnityEngine;

public class ActionButtonHinter : MonoBehaviour, IHintUser
{
    [SerializeField] private string _hint;

    public void SetHint(string text)
    {
        _hint = text;
    }

    public string GetHintText()
    {
        return _hint;
    }
}
