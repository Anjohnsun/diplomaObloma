using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayUI : MonoBehaviour, IGameplayUIService
{
    [SerializeField] private RectTransform _statsTransform;
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

    [Header("Techical info")]
    [SerializeField] private IPawnStats _playerStats;
    [SerializeField] private TextMeshProUGUI _statsTextField;

    [SerializeField] private List<Button> _upgradeButtons;
    [SerializeField] private List<TextMeshProUGUI> _upgradeTextFields;

    [SerializeField] private float _animDuration;
    public List<Button> UpgradeButtons => _upgradeButtons;

    [Inject]
    public void Construct(PlayerPawn player)
    {
        _playerStats = player.PawnStats;
    }

    public void HideEscMenu()
    {
        throw new System.NotImplementedException();
    }

    public void ShowEscMenu()
    {

    }

    public void UnlockUpgrade(bool value)
    {
        if (_upgradeButtons.Count != _upgradeTextFields.Count)
            throw new System.Exception("Upgrade button/text field counts not equal");

        if (value)
        {
            Debug.Log("This should UNLOCK upgrade buttons");
            //Debug.Log($"Сравнение. APLevel: {_playerStats.APLevel}, APLevelCount: {_playerStats.APConfig.Levels.Count}");
            if (_playerStats.APLevel < _playerStats.APConfig.Levels.Count - 1)
            {
                _upgradeTextFields[0].text = _playerStats.APConfig.Levels[_playerStats.APLevel + 1].XPCost.ToString();
                _upgradeButtons[0].interactable = true;
            }
            if (_playerStats.HPLevel < _playerStats.HPConfig.Levels.Count - 1)
            {
                _upgradeTextFields[1].text = _playerStats.HPConfig.Levels[_playerStats.HPLevel + 1].XPCost.ToString();
                _upgradeButtons[1].interactable = true;
            }
            if (_playerStats.ARMLevel < _playerStats.ARMConfig.Levels.Count - 1)
            {
                _upgradeTextFields[2].text = _playerStats.ARMConfig.Levels[_playerStats.ARMLevel + 1].XPCost.ToString();
                _upgradeButtons[2].interactable = true;
            }
            if (_playerStats.STRLevel < _playerStats.STRConfig.Levels.Count - 1)
            {
                _upgradeTextFields[3].text = _playerStats.STRConfig.Levels[_playerStats.STRLevel + 1].XPCost.ToString();
                _upgradeButtons[3].interactable = true;
            }
        }
        else
        {
            _upgradeTextFields[0].text = "";
            _upgradeButtons[0].interactable = false;

            _upgradeTextFields[1].text = "";
            _upgradeButtons[1].interactable = false;

            _upgradeTextFields[2].text = "";
            _upgradeButtons[2].interactable = false;

            _upgradeTextFields[3].text = "";
            _upgradeButtons[3].interactable = false;
        }
    }

    public void HideGameplayInterface()
    {
        _statsTransform.DOKill();
        _tileInfo.DOKill();
        _menu.DOKill();
        _actions.DOKill();

        _statsTransform.DOAnchorPos(_hidePosition1, _animDuration);
        _tileInfo.DOAnchorPos(_hidePosition2, _animDuration);
        _menu.DOAnchorPos(_hidePosition3, _animDuration);
        _actions.DOAnchorPos(_hidePosition4, _animDuration);
    }

    public void ShowGameplayInterface()
    {
        _statsTransform.DOKill();
        _tileInfo.DOKill();
        _menu.DOKill();
        _actions.DOKill();

        _statsTransform.DOAnchorPos(_showPosition1, _animDuration);
        _tileInfo.DOAnchorPos(_showPosition2, _animDuration);
        _menu.DOAnchorPos(_showPosition3, _animDuration);
        _actions.DOAnchorPos(_showPosition4, _animDuration);
    }

    public void UpdatePlayerStats(IPawnStats pawnStats)
    {
        _statsTextField.text = $"AP: {pawnStats.CurrentAP}\nHP: {pawnStats.CurrentHP}\nARM: {pawnStats.ARM}\nSTR: {pawnStats.STR}";
    }
}
