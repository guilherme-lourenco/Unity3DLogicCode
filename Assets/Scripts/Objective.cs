using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private float height = 3f;

    public void InitObjective(Vector3 position)
    {
        transform.localPosition = position;
    }

    public float GetHeight()
    {
        return height;
    }
}
