using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMisc : MonoBehaviour
{
    //Just holds is projectile caught by player
    public bool isCaught = false;

    public LayerMask layerMask;
 
    private void OnCollisionEnter(Collision collision)
    {
        if(layerMask == (layerMask | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
