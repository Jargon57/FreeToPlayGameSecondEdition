using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public AnimationCurve amountToSpawnPerRound;
    public RoundManager roundManager;

    Transform spawnPos;

    int index;
    float amountToSpawn;

    float speedToSpawn;

    public void startRound(int currentRound_)
    {
        //currentRound = currentRound_;

        amountToSpawn = amountToSpawnPerRound.Evaluate(currentRound_);

        InvokeRepeating("spawnEnenmy", 0, speedToSpawn);
    }

    void spawnEnemy()
    {
        if (index < amountToSpawn)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomY = Random.Range(-0.5f, 0.5f);

            Vector3 randomoffset = new Vector3(randomX, randomY, 0);
            GameObject enemyInstance_ = Instantiate(enemyToSpawn, transform.position + randomoffset, Quaternion.identity);

            index++;
        }
        else
        {
            CancelInvoke("spawnEnemy");
        }

    }
}
