using System.Collections;
using System.Collections.Generic;
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

    [Header("Commands")]
    public Text TxtCommands;

    int currentPosition = 0;
    int numberOrder = 1;
    bool looping = false;

    private void Awake()
    {
        configureListener();
    }

    private void Start()
    {
        currentPosition = BoardManager.Instance.GetPinInitialPos();
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
        BoardManager.Instance.AddCommand(numberOrder, direction, currentPosition, (currentPos) =>
        {
            currentPosition = currentPos;

            if (looping)
                TxtCommands.text += $"\n\t{direction.ToString()}";
            if (!looping)
            {
                TxtCommands.text += $"\n{direction.ToString()}";
                numberOrder++;
            }
        });
    }
    private void btnAddLoopCommand()
    {
        looping = !looping;

        BoardManager.Instance.AddLoopCommand(looping, numberOrder, 3);

        if (looping)
            TxtCommands.text += $"\nInitial Loop";

        if (!looping)
        {
            TxtCommands.text += $"\nEnd Loop";
            numberOrder++;
        }
    }

    private void btnPlay()
    {
        StartCoroutine(play());
    }

    private IEnumerator play()
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
    }

    private void gameOver()
    {
        PnlGameOver.SetActive(true);
    }

    private void btnRestartGame()
    {
        MainCamera.SetActive(true);
        TxtCommands.text = string.Empty;
        currentPosition = BoardManager.Instance.GetPinInitialPos();
        numberOrder = 1;
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
}
