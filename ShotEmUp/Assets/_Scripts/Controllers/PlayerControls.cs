using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControls : MonoBehaviour
{
    [Header("General Properties")] 
    [SerializeField] private LevelController controller;
    public float speed = 0.0001f; //Hand Position Lerp Speed
    public bool doLerp = true;
    public Touch touch;//Hand Position Lerp Speed
    [SerializeField] private int health = 3;
    [SerializeField] private NavMeshAgent agent;
    private bool isMoving = false;
    public bool agentOnPoint = false;
    [Header("Hand Properties")]
    public string handName; //Player's hand name
    [SerializeField] private Transform handTransform; //Player hand transform

    // Start is called before the first frame update
    void Start()
    {
        //Get necessary references
        controller = GameObject.FindWithTag("GameController").GetComponent<LevelController>();
        handTransform = transform.Find(handName);
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveHand(doLerp, speed);
        if (!agent.hasPath && isMoving)//if agent has path do walking to there
        {
            isMoving = false;
            controller.readyForEncounter = true;
        }
    }

    //In this function hand position controlled
    void MoveHand(bool _doLerp , float _speed)
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0); //Get first touch 
            
            //Check if finger still on the screen
            if (touch.phase == TouchPhase.Moved)
            {
                handTransform.position = new Vector3(handTransform.position.x + touch.deltaPosition.x * _speed * Time.deltaTime,
                    handTransform.position.y + touch.deltaPosition.y * _speed * Time.deltaTime,
                    handTransform.position.z);
            }
            else if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos =
                    Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, handTransform.localPosition.z));
                touchPos.z = handTransform.position.z;
                handTransform.position = touchPos;
            }
        }
        else
        {
            //Debug.Log("Touch not found");
        }
    }

    
    //Make Level over in this function
    private bool HealthCheck()
    {
        if (health > 1)
        {
            health--;
            return true;
        }
        else
        {
            Debug.Log("You are death");
            return false;
        }
    }

    public void MovePlayer(Vector3 destination)
    {
        agent.destination = destination;
        isMoving = true;
    }
    
    //Rotate Player's view towards to enemy make catching arrow easy
    public void LookToEnemy(Vector3 enemyPos)
    {
        Vector3 rotVector = enemyPos - transform.position; //Get rotation vector by referencing enemy pos to player position 
        Quaternion rotTarget = Quaternion.LookRotation(new Vector3(rotVector.x, 0,rotVector.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * 30);
    }
    //Player Death Check and Handler
    private void PlayerDeath(GameObject projectile)
    {
        //Check if player has health
        if (!HealthCheck())
        {
            controller.GameOverHandler();
        }
        Destroy(projectile);
    }
    
    //Check if Projectile hit player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            PlayerDeath(other.gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int desiredHealth)
    {
        health = desiredHealth;
    }
    
    
}
