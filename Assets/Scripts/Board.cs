using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Transform BoardObj;
    public Transform CellObj1;
    public Transform CellObj2;
    private int matrix = 10;

    private Dictionary<int, Transform> boardPositions;

    private void Awake()
    {
        InitVariables();
    }

    void InitVariables()
    {
        boardPositions = new Dictionary<int, Transform>();
    }

    public void InitBoard()
    {
        int currentX = 0;
        int currentZ = 0;

        for (int column = 0; column < matrix; column++)
        {
            currentX = 0;

            bool useCell1 = (column % 2 == 0);

            for (int line = 0; line < matrix; line++)
            {
                int objName = (line * matrix) + column;
                Transform plane = null;

                if (useCell1)
                    plane = Instantiate((line % 2 == 0) ? CellObj1 : CellObj2, BoardObj);
                else
                    plane = Instantiate((line % 2 == 0) ? CellObj2 : CellObj1, BoardObj);

                plane.localPosition = new Vector3(currentX, 0, currentZ);
                plane.gameObject.name = objName.ToString();
                boardPositions.Add(objName, plane);

                currentX += matrix;
            }
            currentZ += matrix;
        }
    }

    public (float, float, float) GetXYZPosition(int cellPos)
    {
        Vector3 v3 = boardPositions[cellPos].localPosition;

        return (v3.x, v3.y, v3.z);
    }

    public (float, float, float) GetXYZPosition(int cellPos, Action gameOver)
    {
        if (!boardPositions.ContainsKey(cellPos))
        {
            gameOver();
            return (0,0,0);
        }

        Vector3 v3 = boardPositions[cellPos].localPosition;

        return (v3.x, v3.y, v3.z);
    }

    public Transform GetTransform()
    {
        return BoardObj.transform;
    }

    public (int, int) GetDirection(Command.MoveCommand direction, int currentPosition, int currentPosCamera)
    {
        if (direction == Command.MoveCommand.Right)
        {
            currentPosition += 10;
            currentPosCamera = 90;
        }

        if (direction == Command.MoveCommand.Left)
        {
            currentPosition -= 10;
            currentPosCamera = -90;
        }

        if (direction == Command.MoveCommand.Up)
        {
            currentPosition += 1;
            currentPosCamera = 0;
        }

        if (direction == Command.MoveCommand.Down)
        {
            currentPosition -= 1;
            currentPosCamera = 180;
        }

        return (currentPosition, currentPosCamera);
    }
}
