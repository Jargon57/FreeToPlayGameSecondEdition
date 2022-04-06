using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public int currentRound;

    public Text roundCounter;

    public List<GameObject> enemiesOnScreen = new List<GameObject>();

    void Start()
    {
        currentRound = PlayerPrefs.GetInt("currentRound");
    }

    void FixedUpdate()
    {
        if (enemiesOnScreen.Count < 1)
        {
            startRound();
        }
    }

    void startRound()
    {
        PlayerPrefs.SetInt("currentRound", currentRound + 1);
        currentRound = PlayerPrefs.GetInt("currentRound");

        roundCounter.text = "Round: " + currentRound.ToString("00");
    }
}
