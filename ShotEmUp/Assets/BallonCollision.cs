using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallonCollision : MonoBehaviour
{
    public BallonSimpleAI ballonAI;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            ballonAI.PopBallon(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
