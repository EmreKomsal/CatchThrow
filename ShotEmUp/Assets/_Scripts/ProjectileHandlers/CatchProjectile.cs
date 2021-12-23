using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchProjectile : MonoBehaviour
{
    [SerializeField] private LevelController controller;
    public Transform catchedObject = null;
    public PlayerControls playerControls;
    private TouchPhase touchPhase;
    public float throwSpeed;
    public LayerMask layerMask;

    public Transform DebugSphere;
    public RectTransform crosshairPos;
    public Image crosshairImage;
    public Transform crosshairWorld;

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
        else if(DebugSphere != null)
        {
            DebugSphere.position = Aim().point;
        }
    }

    private void LateUpdate()
    {
        crosshairPos.position = Camera.main.WorldToScreenPoint(crosshairWorld.position);
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
        crosshairImage.gameObject.SetActive(true);
        SoundManager.Instance.PlayPlayerSound(SoundManager.PlayerSoundTypes.PlayerArrowCatchSound);
    }

    private void Throw(Transform projectileTransform)
    {
        animator.SetTrigger(throwTrigger);
        projectileTransform.GetComponent<Collider>().enabled = true;
        Rigidbody prRigidbody = projectileTransform.GetComponent<Rigidbody>();
        prRigidbody.isKinematic = false;
        catchedObject.parent = null;
        RaycastHit hitInfo = Aim();
        if (hitInfo.transform == null)
        {
            hitInfo.point = Vector3.forward * 999;
        }
        //Make something for not found enemy situation
        controller.ThrowProjectile(transform.position, hitInfo.point ,catchedObject.gameObject, throwSpeed, false);
        catchedObject = null;
        crosshairImage.gameObject.SetActive(false);
        SoundManager.Instance.PlayPlayerSound(SoundManager.PlayerSoundTypes.PlayerArrowThrowingSound);
    }

    private RaycastHit Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(crosshairPos.position);
        Physics.Raycast(ray, out RaycastHit hitInfo, 999, layerMask);
        return hitInfo;
    }
}
