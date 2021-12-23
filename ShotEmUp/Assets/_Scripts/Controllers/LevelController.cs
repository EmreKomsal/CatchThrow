using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [Header("Player Properties")] 
    public GameObject playerObj; //Player's GameObject
    [SerializeField] private PlayerControls playerControls; //Player's Transform easy calling
    [SerializeField] private Transform playerPositionHolder;
    [SerializeField] private List<Vector3> playerPositions; //Player's Desired Location in Start of Game
    
    public bool readyForEncounter = false;
    [Header("Hand Properties")] 
    [SerializeField] private GameObject hand; //Player Hand

    [Header("Enemy Properties")] 
    public int currentEncounter = 0;
    [SerializeField] private EnemyProjectile currentEnemy;
    [SerializeField] private bool isLevelEnded = false; //
    [SerializeField] private GameObject enemyHolder;
    [SerializeField] private List<EnemyProjectile> enemyLevelList; //All enemy at list
    [SerializeField] private List<EnemyProjectile> enemyEncounterList; //All enemy's in current encounter
    [SerializeField] private float enemyTimer;
    [SerializeField] private float enemyTimerMax = 5;
    [SerializeField] private float enemyTimerMin = 3;

    [SerializeField] private UIController uiController;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        //Enemy Listing Operations will come here
        EnemyLister();
        if (playerObj == null)
        {
            Debug.LogError("PlayerObj couldn't found");
        }
        else
        {
            playerControls = playerObj.GetComponent<PlayerControls>();
            hand = FindObjectOfType<CatchProjectile>().gameObject;
            //SetEncounter(currentEncounter);
            Debug.Log("Player Starting Properties are done");
        }
        enemyTimer = Random.Range(enemyTimerMin, enemyTimerMax);
        for (int i = 0; i < playerPositionHolder.childCount; i++)
        {
            playerPositions.Add(playerPositionHolder.GetChild(i).transform.position);
        }
    }

    public void Update()
    {
        if (readyForEncounter)
        {
            OnEncounter();
        }
    }

    public void ThrowProjectile(Vector3 startPos, Vector3 aimedPos, GameObject projectile, float speed, bool createObj = true)
    {
        if (createObj)
        {
            //Create projectile
            projectile = Instantiate(projectile, startPos, projectile.transform.rotation);
        }
        //Calculate projectile (May need to normalize)
        Vector3 aimVector = Vector3.Normalize((aimedPos - startPos)) * speed;
        projectile.transform.LookAt(aimedPos);
        //Throw projectile with rigidbody force
        projectile.GetComponent<Rigidbody>().AddForce(aimVector, ForceMode.Impulse);
    }

    //Finds all enemy's in level
    private void EnemyLister()
    {
        foreach (Transform enemy in enemyHolder.transform)
        {
            EnemyProjectile enemyProjectile = enemy.GetComponent<EnemyProjectile>();
            if (enemyProjectile != null)
            {
                enemyLevelList.Add(enemyProjectile); //Add gameobjects with enemy projectile scripts to project
            }
        }
    }
    //Finds enemy's in encounter
    private void EnemyEncounterLister()
    {
        enemyEncounterList.Clear(); //Make list empty for new encounter
        foreach (EnemyProjectile enemy in enemyLevelList)
        {
            if (enemy.enemyEncounterNumber == currentEncounter)
            {
                if (enemy != null)
                {
                    enemyEncounterList.Add(enemy);//Add enemy to current encounter list if it is in desired encounter
                }
            }
        }
    }

    public void RemoveFromLists(EnemyProjectile target)
    {
        if (enemyLevelList.Contains(target))
        {
            enemyLevelList.Remove(target);
        }
        if (enemyEncounterList.Contains(target))
        {
            enemyEncounterList.Remove(target);
            if (enemyEncounterList.Count <= 0)
            {
                readyForEncounter = false;
                //encounterMenu.SetActive(true);
                NextEncounter();
                //Time.timeScale = 0f;
            }
        }
    }

    public void SetEncounter()
    {
        currentEnemy = enemyEncounterList[enemyEncounterList.Count / 2];
        Debug.Log("Hello");
        Vector3 playerPos = playerPositions[currentEncounter];
        playerControls.MovePlayer(playerPos); //Get player on desired location and rotation
        enemyTimer = Random.Range(0, 1);
    }

    //Create basic enemy AI for encounter
    private void OnEncounter()
    {
        //Checks if we still have enemy in our current enemy list
        if (enemyEncounterList.Count > 0)
        {
            if (currentEnemy == null)
            {
                currentEnemy = enemyEncounterList[enemyEncounterList.Count / 2];
            }
            playerControls.LookToEnemy(currentEnemy.transform.position);
            if (hand.GetComponent<CatchProjectile>().catchedObject == null && readyForEncounter)
            {
                if (enemyTimer > 0)
                {
                    enemyTimer -= Time.deltaTime;
                }
                //If player has projectile in hand dont throw new one
                else if(enemyTimer <= 0)
                {
                    Debug.Log("Get Ready");
                    EnemyProjectile selectedEnemy = SelectRandomEnemy(5);
                    if (selectedEnemy == null)
                    {
                        return;
                    }
                    currentEnemy = selectedEnemy;
                    Vector3 playerPos = playerObj.transform.position;
                    playerPos.y += 0.4f;
                    currentEnemy.AttackToPlayer(playerPos);
                    enemyTimer = Random.Range(enemyTimerMin, enemyTimerMax);
                }
            }
        }
    }
    //Selects random enemy from current enemy encounter list
    private EnemyProjectile SelectRandomEnemy(int attempt)
    {
        if (enemyEncounterList.Count < 1)
        {
            return null;
        }
        EnemyProjectile selectedEnemy = enemyEncounterList[Random.Range(0, enemyEncounterList.Count)];
        if (!selectedEnemy.canShoot)
        {
            selectedEnemy = SelectRandomEnemy(--attempt);
        }
        return selectedEnemy;
    }



    public void NextEncounter()
    {
        Debug.Log("Get Ready for next encounter");
        if (enemyLevelList.Count == 0)
        {
            uiController.LevelSuccess();
            Debug.Log("Get Ready for next level. This level is completed");
        }
        else
        {
            while (enemyEncounterList.Count == 0)
            {
                EnemyEncounterLister();
                if (enemyEncounterList.Count != 0)
                {
                    break;
                }
                currentEncounter++;
            }
            SetEncounter();
            if (enemyLevelList.Count <= 0)
            {
                isLevelEnded = true;
            }
            currentEncounter++;
        }
    }
    

    public void GameOverHandler()
    {
        uiController.LevelFail();
        readyForEncounter = false;
    }

    public void RewardResume(int healthAmount)
    {
        playerControls.SetHealth(healthAmount);
        uiController.ResumeGame();
    }

    public bool LevelEndCheck()
    {
        return isLevelEnded;
    }
    
}
