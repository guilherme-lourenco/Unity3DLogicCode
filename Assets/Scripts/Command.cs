using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public enum MoveCommand
    {
        Up,
        Down,
        Right,
        Left
    }

    [HideInInspector]
    Dictionary<int, IEnumerator> CommandsToExecute;

    private void Awake()
    {
        CommandsToExecute = new Dictionary<int, IEnumerator>();
    }

    public void AddCommand(int order, IEnumerator command)
    {
        RemoveCommand(order);
        CommandsToExecute.Add(order, command);
    }

    public void RemoveCommand(int order)
    {
        if (CommandsToExecute.ContainsKey(order))
            CommandsToExecute.Remove(order);
    }

    public void ClearCommands()
    {
        CommandsToExecute.Clear();
    }

    public IEnumerator ExecuteCommands()
    {
        for (int i = 1; i <= CommandsToExecute.Count; i++)
        {
            yield return StartCoroutine(CommandsToExecute[i]);

            if (BoardManager.Instance.GameOver)
                yield break;
        }
    }

    public IEnumerator ExecuteLoopCommands(Dictionary<int, IEnumerator> commands)
    {

        for (int j = 1; j <= commands.Count; j++)
        {
            yield return StartCoroutine(commands[j]);

            if (BoardManager.Instance.GameOver)
                yield break;
        }
    }
}
