using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
        {
            transform.Rotate(Vector3.up, 2f);
        }
    }
}
