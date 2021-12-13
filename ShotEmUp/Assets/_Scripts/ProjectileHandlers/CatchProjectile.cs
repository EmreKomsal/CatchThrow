using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchProjectile : MonoBehaviour
{
    [SerializeField] private LevelController controller;
    public Transform catchedObject = null;
    public PlayerControls playerControls;
    private TouchPhase touchPhase;
    public float throwSpeed;
    public LayerMask layerMask;

    //Player's hand animations
    [SerializeField] private Animator animator;
    [SerializeField] private string catchTrigger = "Catch";
    [SerializeField] private string throwTrigger = "Attack";

    private void Start()
    {
        playerControls = GetComponentInParent<PlayerControls>();
        controller = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        touchPhase = playerControls.touch.phase;
        if (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled)
        {
            if (catchedObject !=null)
            {
                Throw(catchedObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check conditions is object projectile , player still clicking to screen and already have catched object
        if (other.gameObject.CompareTag("Projectile") && (touchPhase == TouchPhase.Stationary || touchPhase == TouchPhase.Moved) && catchedObject == null)
        {
            catchedObject = other.transform;
            Catch(catchedObject);
        }
    }

    private void Catch(Transform projectileTransform)
    {
        animator.SetTrigger(catchTrigger);
        projectileTransform.GetComponent<ProjectileMisc>().isCaught = true;
        Rigidbody prRigidbody = projectileTransform.GetComponent<Rigidbody>();
        prRigidbody.velocity = Vector3.zero;
        prRigidbody.angularVelocity = Vector3.zero;
        prRigidbody.isKinematic = true;
        projectileTransform.GetComponent<Collider>().enabled = false;
        projectileTransform.parent = transform;
        projectileTransform.localPosition = Vector3.zero;
    }

    private void Throw(Transform projectileTransform)
    {
        animator.SetTrigger(throwTrigger);
        projectileTransform.GetComponent<Collider>().enabled = true;
        Rigidbody prRigidbody = projectileTransform.GetComponent<Rigidbody>();
        prRigidbody.isKinematic = false;
        catchedObject.parent = null;
        Ray ray = Camera.main.ScreenPointToRay(playerControls.touch.position);
        Physics.Raycast(ray, out RaycastHit hitInfo, 999, layerMask);
        if (hitInfo.transform == null)
        {
            hitInfo.point = Vector3.forward * 999;
        }
        //Make something for not found enemy situation
        controller.ThrowProjectile(transform.position, hitInfo.point ,catchedObject.gameObject, throwSpeed, false);
        catchedObject = null;
    }
}
