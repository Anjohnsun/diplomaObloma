using DG.Tweening;
using UnityEngine;

public class GameplayUIService : MonoBehaviour, IGameplayUIService
{
    [SerializeField] private RectTransform _playerStats;
    [SerializeField] private Vector2 _hidePosition1;
    [SerializeField] private Vector2 _showPosition1;

    [SerializeField] private RectTransform _tileInfo;
    [SerializeField] private Vector2 _hidePosition2;
    [SerializeField] private Vector2 _showPosition2;

    [SerializeField] private RectTransform _menu;
    [SerializeField] private Vector2 _hidePosition3;
    [SerializeField] private Vector2 _showPosition3;

    [SerializeField] private RectTransform _actions;
    [SerializeField] private Vector2 _hidePosition4;
    [SerializeField] private Vector2 _showPosition4;


    [SerializeField] private float _showHideDuration;

    public void HideEscMenu()
    {
        throw new System.NotImplementedException();
    }

    public void ShowEscMenu()
    {
        
    }

    public void HideGameplayInterface()
    {
        _playerStats.DOAnchorPos(_hidePosition1, _showHideDuration);
        _tileInfo.DOAnchorPos(_hidePosition2, _showHideDuration);
        _menu.DOAnchorPos(_hidePosition3, _showHideDuration);
        _actions.DOAnchorPos(_hidePosition3, _showHideDuration);
    }

    public void ShowGameplayInterface()
    {
        _playerStats.DOAnchorPos(_showPosition1, _showHideDuration);
        _tileInfo.DOAnchorPos(_showPosition2, _showHideDuration);
        _menu.DOAnchorPos(_showPosition3, _showHideDuration);
        _actions.DOAnchorPos(_showPosition4, _showHideDuration);
    }

    public void UpdatePlayerStats(IPawnStats pawnStats)
    {
        throw new System.NotImplementedException();
    }
}
