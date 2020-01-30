using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BoardUIManager : MonoBehaviour
{
    [Header("MainCamera")]
    public GameObject MainCamera;

    [Header("Buttons")]
    public Button BtnMoveUp;
    public Button BtnMoveDown;
    public Button BtnMoveRight;
    public Button BtnMoveLeft;
    public Button BtnLoop;
    public Button BtnPlay;

    [Header("Buttons PlayAgain")]
    public Button BtnPlayAgain;
    public Button BtnWin;

    [Header("Painels")]
    public GameObject PnlGameOver;
    public GameObject PnlWin;

    [Header("Commands Selected")]
    public CommandsSelected CommandsSelectedPrefab;
    public Transform PnlLoopPrefab;
    public Transform PnlCommandsSelected;
    public List<Color> ColorsCommands;

    [Header("Texts")]
    public Text TxtMsg;
    public Text TxtLoop;

    private Transform CurrentPnlLoop;
    bool looping = false;    

    private void Awake()
    {
        configureListener();
    }

    void configureListener()
    {
        BtnMoveUp.onClick.AddListener(() => btnAddMoveCommand(Command.MoveCommand.Up));
        BtnMoveDown.onClick.AddListener(() => btnAddMoveCommand(Command.MoveCommand.Down));
        BtnMoveRight.onClick.AddListener(() => btnAddMoveCommand(Command.MoveCommand.Right));
        BtnMoveLeft.onClick.AddListener(() => btnAddMoveCommand(Command.MoveCommand.Left));

        BtnLoop.onClick.AddListener(() => btnAddLoopCommand());

        BtnPlay.onClick.AddListener(() => btnPlay());
        BtnPlayAgain.onClick.AddListener(() => btnRestartGame());
        BtnWin.onClick.AddListener(() => btnWin());
    }

    private void btnAddMoveCommand(Command.MoveCommand direction)
    {
        BoardManager.Instance.AddCommand(direction, (guid) =>
        {
            InstantiateCommands(guid, direction);
            LayoutRebuilder.ForceRebuildLayoutImmediate(PnlCommandsSelected.GetComponent<RectTransform>());
        });
    }
    private void btnAddLoopCommand()
    {
        looping = !looping;

        if (looping)
        {
            CurrentPnlLoop = Instantiate(PnlLoopPrefab, PnlCommandsSelected);
            TxtLoop.text = "END LOOP";
            LayoutRebuilder.ForceRebuildLayoutImmediate(PnlCommandsSelected.GetComponent<RectTransform>());
            return;
        }

        TxtLoop.text = "LOOP";
        BoardManager.Instance.AddLoopCommand(looping, 3);
    }

    private void btnPlay()
    {
        StartCoroutine(play());
    }

    private IEnumerator play()
    {
        if (!looping)
        {
            MainCamera.SetActive(false);

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(BoardManager.Instance.PlayGame((result) =>
            {
                if (!result)
                    gameOver();
                else
                    win();
            }));

            yield break;
        }

        StartCoroutine(errorMsg("CLOSE THE LOOP!"));
    }

    private void gameOver()
    {
        PnlGameOver.SetActive(true);
    }

    private void btnRestartGame()
    {
        PnlCommandsSelected.GetComponentsInChildren<CommandsSelected>().ToList().ForEach(x => Destroy(x.gameObject));
        MainCamera.SetActive(true);
        BoardManager.Instance.ResetBoard();
        PnlGameOver.SetActive(false);
    }

    private void win()
    {
        PnlWin.SetActive(true);
    }

    private void btnWin()
    {
        btnRestartGame();
        PnlWin.SetActive(false);
    }

    private void InstantiateCommands(Guid guid, Command.MoveCommand direction)
    {
        CommandsSelected command = Instantiate(CommandsSelectedPrefab, (looping) ? CurrentPnlLoop : PnlCommandsSelected);
        command.SetInfo(direction.ToString(), ColorsCommands[(int) direction], guid);
    }

    IEnumerator errorMsg(string error)
    {
        TxtMsg.text = error;
        TxtMsg.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        TxtMsg.gameObject.SetActive(false);
    }
}
