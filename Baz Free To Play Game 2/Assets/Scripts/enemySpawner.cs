using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public AnimationCurve amountToSpawnPerRound;
    public RoundManager roundManager;
    public GameObject spawnParticle;

    Transform spawnPos;

    int index;
    int amountToSpawn;

    public float speedToSpawn;

    public void startRound(int currentRound_)
    {
        //currentRound = currentRound_;

        amountToSpawn = (int)amountToSpawnPerRound.Evaluate(currentRound_);

        InvokeRepeating("spawnEnemy", 0, speedToSpawn);

        index = 0;
    }

    void spawnEnemy()
    {
        if (index < amountToSpawn)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomY = Random.Range(-0.5f, 0.5f);

            Vector3 randomoffset = new Vector3(randomX, randomY, 0);
            GameObject enemyInstance_ = Instantiate(enemyToSpawn, transform.position + randomoffset, Quaternion.identity);

            GameObject particleInstance = Instantiate(spawnParticle, enemyInstance_.transform.position, Quaternion.identity);
            particleInstance.SetActive(true);
            Destroy(particleInstance, 1f);

            index++;
        }
        else
        {
            CancelInvoke("spawnEnemy");
            
        }

    }
}
