using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public int currentRound;
    [HideInInspector] public int highestRound;

    public Text roundCounter;

    public List<GameObject> enemiesOnScreen = new List<GameObject>();
    public enemySpawner[] enemySpawners;
    public GameManager gameManager;

    bool hasFinished;
    bool roundDelay = true;
    bool isInGameScreen;

    void Start()
    {
        currentRound = PlayerPrefs.GetInt("currentRound");
        enemySpawners = FindObjectsOfType<enemySpawner>();

        roundDelay = true;
    }

    void FixedUpdate()
    {
        if (gameManager.isInGameScreen() && !isInGameScreen)
        {
            isInGameScreen = true;
            InvokeRepeating("checkForEnemyDeath", 0, 0.1f);
        }

        if (!gameManager.isInGameScreen() && isInGameScreen)
        {
            isInGameScreen = false;
            CancelInvoke("checkForEnemyDeath");
        }

        if (gameManager.isInGameScreen())
        {
            if (enemiesOnScreen.Count < 1)
            {
                startRound();
            }

            hasFinished = false;
        }
        else
        {
            if (!hasFinished)
            {
                finishGame(false);
                hasFinished = true;

            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.R))
        {
            PlayerPrefs.SetInt("currentRound", 1);
        }
    }

    void checkForEnemyDeath()
    {
        //means there are less enemies on the screen then there are in the list
        if (allEnemies().Length < enemiesOnScreen.Count)
        {
            for (int i = 0; i < enemiesOnScreen.Count; i++)
            {
                if (enemiesOnScreen[i].gameObject == null)
                {
                    enemiesOnScreen.RemoveAt(i);
                }
            }
        }
    }

    public void finishGame(bool isButton)
    {
        if (isButton)
        {
            FindObjectOfType<HealthSystem>().dieAndReset();
        }
        currentRound = 0;
        for (int i = 0; i < enemiesOnScreen.Count; i++)
        {
            if (enemiesOnScreen[i].GetComponent<EnemyBehaviour>())
            {
                enemiesOnScreen[i].GetComponent<EnemyBehaviour>().Die();
            }
            else
            {
                enemiesOnScreen[i].GetComponent<RangedEnemyBehaviour>().Die();
            }
            //Destroy(enemiesOnScreen[i]);
        }

        enemiesOnScreen.Clear();

        if (currentRound > highestRound)
        {
            highestRound = currentRound;
        }
    }

    void startRound()
    {
        if (roundDelay)
        {
            StartCoroutine(roundWait());

            PlayerPrefs.SetInt("currentRound", currentRound + 1);
            currentRound = PlayerPrefs.GetInt("currentRound");

            roundCounter.text = "Round: " + currentRound.ToString("00");

            for (int i = 0; i < enemySpawners.Length; i++)
            {
                enemySpawners[i].startRound(currentRound);
            }

            roundCounter.GetComponent<Animator>().Play("New Round");
        }
    }

    GameObject[] allEnemies()
    {
        EnemyBehaviour[] enemies = FindObjectsOfType<EnemyBehaviour>();
        RangedEnemyBehaviour[] rangedEnemies = FindObjectsOfType<RangedEnemyBehaviour>();


        GameObject[] enemyGameObjects = new GameObject[enemies.Length + rangedEnemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemyGameObjects[i] = enemies[i].gameObject;
        }

        for (int i = 0; i < rangedEnemies.Length; i++)
        {
            enemyGameObjects[i + enemies.Length] = rangedEnemies[i].gameObject;
        }

        //        Debug.Log(enemyGameObjects.Length);
        return enemyGameObjects;


        //return //GameObject.FindGameObjectsWithTag("Enemy");///
    }

    IEnumerator roundWait()
    {
        roundDelay = false;

        yield return new WaitForSeconds(1f);

        enemiesOnScreen.AddRange(allEnemies());

        yield return new WaitForSeconds(0.5f);

        roundDelay = true;
    }
}
