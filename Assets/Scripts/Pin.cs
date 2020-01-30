using System.Collections;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private float speed = 1f;
    private float height = 4.5f;
    private int currentPosition = 0;
    private int posCamera = 0;

    public Transform MainCamera;

    public void InitPin(Vector3 position)
    {
        transform.localPosition = position;
        transform.localEulerAngles = Vector3.zero;
        currentPosition = BoardManager.Instance.GetPinInitialPos();
    }

    public float GetHeight()
    {
        return height;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Block>() != null)
        {
            if (BoardManager.Instance != null)
                BoardManager.Instance.SetGameOver();
        }

        if (other.GetComponent<Objective>() != null)
        {
            if (BoardManager.Instance != null)
                BoardManager.Instance.SetWin(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Objective>() != null)
        {
            if (BoardManager.Instance != null)
                BoardManager.Instance.SetWin(false);
        }
    }

    public IEnumerator Move(Command.MoveCommand command)
    {

        bool stopMove = false;

        (currentPosition, posCamera) = BoardManager.Instance.Board.GetDirection(command, currentPosition, posCamera);

        (float x, float y, float z) = BoardManager.Instance.Board.GetXYZPosition(currentPosition, () =>
        {
            BoardManager.Instance.SetGameOver();
            stopMove = true;
        });

        if (stopMove)
            yield break;

        Vector3 currentPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

        Vector3 currentRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        Vector3 newCurrentRot = new Vector3(transform.localEulerAngles.x, posCamera, 0);

        float t = 0f;

        if (currentRot.y != newCurrentRot.y)
        {
            while (t < 1f)
            {
                t += 1 * Time.deltaTime;
                transform.localEulerAngles = Vector3.Lerp(currentRot, newCurrentRot, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }

        t = 0f;
        while (t < 1f)
        {
            t += speed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(currentPos, new Vector3(x, GetHeight(), z), Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
