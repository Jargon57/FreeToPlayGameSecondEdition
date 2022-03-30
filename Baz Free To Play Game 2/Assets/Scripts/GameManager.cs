using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isInGame;
    public bool isInStore;

    public int money;

    [Header("UI Elements")]
    [Space]
    public GameObject gameUIElements;
    public GameObject menuUIElements;

    public Text moneyText;

    [Space]
    public GameObject player;

    [Space]
    public Animator shopAni;
    public Animator menuAni;
    public Animator moneyAni;

    void Start()
    {
        isInGame = false;

        player.SetActive(false);

        PlayerPrefs.SetInt("isInStore", 0);

        money = PlayerPrefs.GetInt("money");
        moneyText.text = "$" + money.ToString("0");
    }

    public void finishGame()
    {
        menuAni.SetBool("isInGame", false);

        player.SetActive(true);

        isInGame = false;
    }

    public void startGame()
    {
        //load game Data
        money = PlayerPrefs.GetInt("money");
        //menuUIElements.SetActive(false);
        // gameUIElements.SetActive(true);

        menuAni.SetBool("isInGame", true);

        isInGame = true;

        player.SetActive(true);
    }

    public void addMoney(int amount)
    {
        money += amount;

        PlayerPrefs.SetInt("money", money);

        moneyText.text = "$" + money.ToString("0");

        moneyAni.Play("MoneyIdle");
    }

    public void enterStore()
    {
        //Debug.Log(isInGame);
        if (isInStore)
        {
            isInStore = false;
            shopAni.SetBool("store", false);

            if (isInGame)
            {
                PlayerPrefs.SetInt("isInStore", 0);
                //FindObjectOfType<CharacterMovement>().isInStore = false;
            }
        }
        else
        {
            isInStore = true;
            shopAni.SetBool("store", true);

            if (isInGame)
            {
                PlayerPrefs.SetInt("isInStore", 1);
            }
        }
    }
}
