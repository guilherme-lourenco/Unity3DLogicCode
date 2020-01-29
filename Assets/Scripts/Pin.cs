using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private float speed = 1f;
    private float height = 4.5f;

    public Transform MainCamera;

    public void InitPin(Vector3 position)
    {
        transform.localPosition = position;
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

    public IEnumerator Move(Vector3 to, int rotation)
    {
        Vector3 currentPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

        Vector3 currentRot = new Vector3(transform.rotation.x, transform.rotation.y, 0);
        Vector3 newCurrentRot = new Vector3(transform.rotation.x, rotation, 0);

        float t = 0f;
       
        if (currentRot.y != newCurrentRot.y)
        {
            while (t < 1f)
            {
                t += 2 * Time.deltaTime;
                transform.localEulerAngles = Vector3.Lerp(currentRot, newCurrentRot, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
        }


        t = 0f;
        while (t < 1f)
        {
            t += speed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(currentPos, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
