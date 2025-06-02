using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerPawn : APawn
{
    private PlayerInput _input;

    private int _chosenActionIndex = -1;
    List<FieldTile> _tiles;

    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private Transform _actionPanel;
    [SerializeField] private GameObject _actionIconPrefab;
    private List<Button> _actionButtons;

    public override void Construct(int hpLvl, int apLvl, int strLvl, int armLvl)
    {
        base.Construct(hpLvl, apLvl, strLvl, armLvl);

        //доступные действия
        _actions = new List<APawnAction>();
        _actionButtons = new List<Button>();

        _actions.Add(new XMoveAction(this, 0.6f, -1));

        //создание иконок действий
        for (int i = 0; i < _actions.Count; i++)
        {
            Transform icon = Instantiate(_actionIconPrefab, _actionPanel).transform;
            icon.GetChild(0).GetComponent<Image>().sprite = _actions[i].ActionIcon;
            icon.GetComponent<ActionButtonHinter>().SetHint(_actions[i].Hint);
            int index = i;
            icon.GetComponent<Button>().onClick.AddListener(() => ChooseAction(index));
            _actionButtons.Add(icon.GetComponent<Button>());
        }

        PawnStats.OnStatsChanged += UpdateUI;
        UpdateUI();


        _input = new PlayerInput();
        _input.GameplayInput.DoAction.performed += context => DoAction();

        PawnTeam = PawnTeam.Player;
    }

    private void EnableActionButtons(bool value)
    {
        foreach (Button button in _actionButtons)
        {
            button.interactable = value;
        }
    }

    public void EnableInput(bool value)
    {
        if (value)
        {
            if (PawnStats.CurrentAP <= 0)
            {
                PawnStats.StartTurn();
            }

            _input.Enable();
            EnableActionButtons(value);
            ChooseAction(0);
        }
        else
        {
            _input.Disable();
        }
    }

    private void UpdateUI()
    {
        _gameplayUI.UpdatePlayerStats(PawnStats);
    }

    private void ChooseAction(int actionIndex)
    {
        GridManager.Instance.DemarkTiles();
        _chosenActionIndex = actionIndex;

        _tiles = _actions[actionIndex].CalculateTargets();
        GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);
    }

    public override string GetHintText()
    {
        return "Ваш персонаж. Его судьба - в ваших руках.";
    }

    private void DoAction()
    {
        EnableInput(false);
        GridManager.Instance.DemarkTiles();

        FieldTile tile = GridManager.Instance.WorldPositionToTile(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        _actions[_chosenActionIndex].Perform(tile, () => HandleActionEnding());

        if (StateManager.Instance.CurrentState == typeof(UpgradeState))
        {
            PawnStats.ResetAP();
        }
    }

    private void HandleActionEnding()
    {
        Debug.Log($"PawnYPos: {GridPosition.y}, LevelYSize: {GridManager.Instance.VerticalSize - 1}");

        if (GridPosition.y == GridManager.Instance.VerticalSize - 1)
        {
            Debug.Log("Start level transition");
            LevelManager.Instance.StartLevelTransition();
        }


        if (PawnStats.CurrentAP <= 0)
        {
            GridManager.Instance.DemarkTiles();
            StateManager.Instance.ChangeState<EnemyTurnState>();
        }
        else
        {
            EnableInput(true);
        }
    }

    public void UnlockUpgrade(bool value)
    {
        //
    }
}
