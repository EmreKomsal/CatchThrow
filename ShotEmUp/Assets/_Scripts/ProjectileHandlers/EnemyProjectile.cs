using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyProjectile : MonoBehaviour
{
    public Transform gunTip;

    public GameObject projectilePrefab;

    public int enemyEncounterNumber;

    public bool canShoot = true;

    [SerializeField] private LevelController controller;
    
    [Header("AnimControls")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string deathTrigger = "Die";

    public float speed;

    private IEnumerator _deathCorutine;

    private IEnumerator _attackCorutine;
    // Start is called before the first frame update
    void Start()
    {
        //Find controller used in every object
        controller = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //Make enemy death if projectile hits iy
        if (other.gameObject.CompareTag("Projectile"))
        {
            GameObject projectileObject = other.gameObject;
            if (projectileObject.GetComponent<ProjectileMisc>().isCaught)
            {
                Destroy(projectileObject);
                Die(1f);
            }
        }
    }

    public void AttackToPlayer(Vector3 playerPos)
    {
        animator.SetTrigger(attackTrigger);
        transform.LookAt(playerPos);
        _attackCorutine = AttackCoroutine(0.2f, playerPos);
        StartCoroutine(_attackCorutine);
    }
    
    IEnumerator AttackCoroutine(float waitTime, Vector3 playerPos)
    {
        yield return new WaitForSeconds(waitTime);
        controller.ThrowProjectile(gunTip.position, playerPos, projectilePrefab, speed);
    }

    public void Die(float time)
    {
        animator.SetTrigger(deathTrigger);
        canShoot = false;
        _deathCorutine = Killself(time);
        StartCoroutine(_deathCorutine);
    }

    IEnumerator Killself(float waitTime)
    {
        while (true)
        {
            Debug.Log("Im gonna die");
            yield return new WaitForSeconds(waitTime);
            Debug.Log("I died");
            controller.RemoveFromLists(this);
            Destroy(gameObject);
        }
    }
}
