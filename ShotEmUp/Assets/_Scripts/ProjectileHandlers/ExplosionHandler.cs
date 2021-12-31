using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    public bool isExplosive;
    [Header("Tag List")] public List<string> TagList; //List of tags for explosive or traps activation
    
    public float radius = 5f; //Explosion Radius
    public float explosionForce = 1000f;

    public GameObject brokenObj;
    public GameObject particleObj;
    private bool isBroken = false;
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (TagList.Contains(other.gameObject.tag) && !isBroken)
        {
            if (isExplosive)
            {
                Explosion();
            }
            BarrelBroke();
            if (other.gameObject.CompareTag("Projectile"))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius); //Get Every collider in radius

        foreach (Collider nearbyEnemy in colliders)
        {
            if (nearbyEnemy.gameObject.CompareTag("Enemy"))
            {
                //Kill all enemies in explosion zone
                Debug.Log(nearbyEnemy.gameObject.name);
                EnemyProjectile enemyProjectile = nearbyEnemy.GetComponent<EnemyProjectile>();
                if(enemyProjectile != null)//call kill function
                {
                    enemyProjectile.Die();
                }
                 
            }
        }
    }

    private void BarrelBroke()
    {
        isBroken = true;
        var replacement = Instantiate(brokenObj, transform.position, transform.rotation);
        if (particleObj != null)
        {
            var bombEffect = Instantiate(particleObj, replacement.transform);
            bombEffect.GetComponent<ParticleSystem>().Play();
        }
        var rbs = replacement.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(explosionForce, transform.position, radius, 1f);
        }
        
        Destroy(gameObject);
    }
}
