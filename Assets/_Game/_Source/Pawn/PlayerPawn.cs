using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerPawn : APawn
{
    private PlayerInput _input;
    private bool _inputEnabled;

    private int _chosenActionIndex;
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

    public void EnableInput(bool v)
    {
        if (v)
        {
            if (PawnStats.CurrentAP <= 0)
            {
                PawnStats.StartTurn();
            }

            _inputEnabled = true;
            _input.Enable();
            //ChooseDefaultActions();
        }
        else
        {
            _inputEnabled = false;
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
    }

    private void HandleActionEnding()
    {
/*        if (_endlessActions)
        {
            _pawn.PawnStats.ResetAP();
        }*/

        if (PawnStats.CurrentAP <= 0)
        {
            if (GridPosition.y == GridManager.Instance.VerticalSize - 1)
            {
                LevelManager.Instance.StartLevelTransition();
            }
            GridManager.Instance.DemarkTiles();
            StateManager.Instance.ChangeState<EnemyTurnState>();
        }
        else
        {
            if (GridPosition.y == GridManager.Instance.VerticalSize - 1)
            {
                LevelManager.Instance.StartLevelTransition();
            }
            EnableInput(true);
        }
    }

    /*    private void ChooseDefaultActions()
        {
            GridManager.Instance.DemarkTiles();

            _tiles = _pawn.MoveAction.CalculateTargets();
            GridManager.Instance.MarkTiles(_tiles, MarkerType.interact);

            _tiles = _pawn.AttackAction.CalculateTargets();
            GridManager.Instance.MarkTiles(_tiles, MarkerType.attack);
        }*/
}
