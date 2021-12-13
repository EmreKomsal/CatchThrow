using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageProjectile : MonoBehaviour
{
    public LevelController controller;

    private void Start()
    {
        controller = FindObjectOfType<LevelController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            //Make game over or watch ad for continue
            Debug.Log("You killed hostage. Game Over");
            controller.GameOverHandler();
        }
    }
}
