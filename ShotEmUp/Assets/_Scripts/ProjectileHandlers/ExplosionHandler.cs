using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [Header("Tag List")] public List<string> TagList; //List of tags for explosive or traps activation
    
    public float radius = 5f; //Explosion Radius
    public float explosionForce = 1000f;

    public GameObject brokenObj;
    private bool isBroken = false;
    private void OnCollisionEnter(Collision other)
    {
        if (TagList.Contains(other.gameObject.tag) && !isBroken)
        {
            Explosion();
            BarrelBroke();
            Destroy(other.gameObject);
        }
    }

    private void Explosion()
    {
        //Add effects here

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius); //Get Every collider in radius

        foreach (Collider nearbyEnemy in colliders)
        {
            if (nearbyEnemy.gameObject.CompareTag("Enemy"))
            {
                //Kill all enemies in explosion zone
                Debug.Log(nearbyEnemy.gameObject.name);
                EnemyProjectile enemyProjectile = nearbyEnemy.GetComponent<EnemyProjectile>();
                enemyProjectile.Die(); //call kill function
            }
        }
    }

    private void BarrelBroke()
    {
        isBroken = true;
        var replacement = Instantiate(brokenObj, transform.position, transform.rotation);
        //------Add instance particle in here------//
        var rbs = replacement.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(explosionForce, transform.position, radius, 1f);
        }
        
        Destroy(gameObject);
    }
}
