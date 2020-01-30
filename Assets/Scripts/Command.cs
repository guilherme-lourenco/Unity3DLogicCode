using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{
    public enum MoveCommand
    {
        Up = 0,
        Down = 1,
        Right = 2,
        Left = 3
    }

    [HideInInspector]
    Dictionary<Guid, IEnumerator> CommandsToExecute;

    private void Awake()
    {
        CommandsToExecute = new Dictionary<Guid, IEnumerator>();
    }

    public void AddCommand(Guid guid, IEnumerator command)
    {
        RemoveCommand(guid);
        CommandsToExecute.Add(guid, command);
    }

    public void RemoveCommand(Guid guid)
    {
        if (CommandsToExecute.ContainsKey(guid))
            CommandsToExecute.Remove(guid);
    }

    public void ClearCommands()
    {
        CommandsToExecute.Clear();
    }

    public IEnumerator ExecuteCommands()
    {
        foreach (KeyValuePair<Guid, IEnumerator> command in CommandsToExecute)
        {
            yield return StartCoroutine(command.Value);
            if (BoardManager.Instance.GameOver)
                yield break;
        }
    }

    public IEnumerator ExecuteLoopCommands(Dictionary<Guid, IEnumerator> commands)
    {
        foreach (KeyValuePair<Guid, IEnumerator> command in commands)
        {
            yield return StartCoroutine(command.Value);

            if (BoardManager.Instance.GameOver)
                yield break;
        }
    }
}
