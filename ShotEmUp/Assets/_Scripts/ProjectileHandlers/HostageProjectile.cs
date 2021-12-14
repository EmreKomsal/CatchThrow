using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageProjectile : MonoBehaviour
{
    public LevelController controller;
    public Animator animator;

    private void Start()
    {
        controller = FindObjectOfType<LevelController>();
        animator = GetComponent<Animator>();
    }

    //Animation Event For Hostage Death
    public void HostageKilled()
    {
        Debug.Log("You killed hostage. Game Over");
        controller.GameOverHandler();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            animator.SetTrigger("Death");
            //Make game over or watch ad for continue
            Destroy(other.gameObject);
        }
    }
}
