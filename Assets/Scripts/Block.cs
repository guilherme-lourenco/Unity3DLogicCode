using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float height = 2.5f;
    private string instanceName = "block";

    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public float GetHeight()
    {
        return height;
    }

    public string GetName()
    {
        return instanceName;
    }
}
