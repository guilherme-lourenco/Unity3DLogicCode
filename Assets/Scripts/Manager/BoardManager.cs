using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public Board Board;
    public Pin Pin;
    public Objective Objective;
    public Block Block;
    public Command Commands;

    private int initialPinPos = 10;
    private int initialObjectivePos = 24;

    [HideInInspector] public bool GameOver = false;
    [HideInInspector] public bool Win = false;


    Dictionary<Guid, IEnumerator> loopCommands;
    bool looping = false;
    int iteration = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitGame();
    }

    void InitGame()
    {
        Board.InitBoard();
        InitBlocks();
        StartPin();
        StartObjective();
    }

    void StartPin()
    {
        (float x, float y, float z) = Board.GetXYZPosition(initialPinPos);
        Pin.InitPin(new Vector3(x, Pin.GetHeight(), z));
    }

    void StartObjective()
    {
        (float x, float y, float z) = Board.GetXYZPosition(initialObjectivePos);
        Objective.InitObjective(new Vector3(x, Objective.GetHeight(), z));
    }

    void InitBlocks()
    {
        List<int> blocks = new List<int>();

        for (int i = 0; i < 20; i++)
        {
            int pos = UnityEngine.Random.Range(0, 100);

            if (!blocks.Contains(pos) && pos != initialPinPos && pos != initialObjectivePos)
            {
                Block block = Instantiate(Block, Board.GetTransform());
                (float x, float y, float z) = Board.GetXYZPosition(pos);
                block.SetPosition(new Vector3(x, block.GetHeight(), z));
                block.name = block.GetName();
                blocks.Add(pos);
            }
        }
    }

    public int GetPinInitialPos()
    {
        return initialPinPos;
    }

    public void AddCommand(Command.MoveCommand command, Action<Guid> callbackPos)
    {
        if (Utils.Enum<Command.MoveCommand>.IsDefined(command))
        {
            Guid guid = Guid.NewGuid();

            if (!looping)
            {
                Commands.AddCommand(guid, Pin.Move(command));
            }

            if (looping)
            {
                for (int i = 0; i < iteration; i++)
                {
                    guid = Guid.NewGuid();
                    loopCommands.Add(guid, Pin.Move(command));
                }
            }

            callbackPos(guid);
            return;
        }
    }

    public void AddLoopCommand(bool initLoop, int iteration)
    {
        looping = initLoop;
        if (initLoop)
        {
            loopCommands = new Dictionary<Guid, IEnumerator>();
            this.iteration = iteration;
            return;
        }

        Guid guid = Guid.NewGuid();

        Commands.AddCommand(guid, Commands.ExecuteLoopCommands(loopCommands));
    }

    public IEnumerator PlayGame(Action<bool> result)
    {
        yield return StartCoroutine(Commands.ExecuteCommands());
        result((Win) ? true : false);
    }

    public void SetGameOver()
    {
        GameOver = true;
    }

    public void SetWin(bool win)
    {
        this.Win = win;
    }

    public void ResetBoard()
    {
        GameOver = false;
        Commands.ClearCommands();
        StartPin();
    }
}
