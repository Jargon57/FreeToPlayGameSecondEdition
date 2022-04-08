using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public struct gameData
{
    public int money;
    public float currentShootSpeed;
    public int currentMaxHealth;
    public float currentBulletDamage;
    public float currentMaxSpeed;
    public int highestRound;

    public int shootSpeedLevel;
    public int maxHealthLevel;
    public int BulletDamageLevel;
    public int maxSpeedLevel;
    public int cost;
}

public class GameManager : MonoBehaviour
{
    const string fileName = "saveData.Json";
    string filePath;
    string completedfilePath;

    public bool isInGame;
    public bool isInStore;

    public int money;

    [Space]
    public float defaultShootSpeed;
    public int defaultMaxHealth;
    public float defaultBulletDamage;
    public float defaultMaxMoveSpeed;

    [Header("UI Elements")]
    [Space]
    public GameObject gameUIElements;
    public GameObject menuUIElements;

    public Text moneyText;
    public Text highScoreText;

    [Space]
    public GameObject player;

    [Space]
    public Animator shopAni;
    public Animator menuAni;
    public Animator moneyAni;

    public Shoot gunBehaviour;
    public RoundManager roundManager;
    public StoreManager storeManager;
    public HealthSystem healthSystem;

    AudioSource clickSound;
    gameData gD;

    void Awake()
    {
        gD = new gameData();
        filePath = Application.persistentDataPath;
        completedfilePath = filePath + "/" + fileName;

        gunBehaviour = FindObjectOfType<Shoot>();
        roundManager = FindObjectOfType<RoundManager>();
        storeManager = FindObjectOfType<StoreManager>();
        healthSystem = FindObjectOfType<HealthSystem>();

        loadGameData();
    }

    void Start()
    {
        isInGame = false;

        player.SetActive(false);

        PlayerPrefs.SetInt("isInStore", 0);
        PlayerPrefs.SetInt("isInGame", 0);

        //money = PlayerPrefs.GetInt("money");
        moneyText.text = "$" + money.ToString("0");

        clickSound = GetComponent<AudioSource>();
    }

    public void saveGameData()
    {
        gD.money = money;
        gD.currentShootSpeed = gunBehaviour.reloadtime;
        gD.currentMaxHealth = player.GetComponent<HealthSystem>().maxHealth;
        gD.currentBulletDamage = gunBehaviour.damage;
        gD.currentMaxSpeed = player.GetComponent<CharacterMovement>().maxSpeed;
        gD.highestRound = roundManager.highestRound;

        gD.shootSpeedLevel = storeManager.fireSpeedLevel;
        gD.maxHealthLevel = storeManager.healthLevel;
        gD.BulletDamageLevel = storeManager.damageLevel;
        gD.maxSpeedLevel = storeManager.moveSpeedLevel;
        gD.cost = storeManager.cost;

        string savedJsonData = JsonUtility.ToJson(gD);
        File.WriteAllText(completedfilePath, savedJsonData);
    }

    public void loadGameData()
    {
        if (File.Exists(completedfilePath))
        {
            string loadedJson = File.ReadAllText(completedfilePath);
            gD = JsonUtility.FromJson<gameData>(loadedJson);
        }
        else
        {
            resetGameData();
        }

        money = gD.money;
        gunBehaviour.reloadtime = gD.currentShootSpeed;
        player.GetComponent<HealthSystem>().maxHealth = gD.currentMaxHealth;
        gunBehaviour.damage = gD.currentBulletDamage;
        gD.currentMaxSpeed = player.GetComponent<CharacterMovement>().maxSpeed = gD.currentMaxSpeed;
        roundManager.highestRound = gD.highestRound;

        storeManager.fireSpeedLevel = gD.shootSpeedLevel;
        storeManager.healthLevel = gD.maxHealthLevel;
        storeManager.damageLevel = gD.BulletDamageLevel;
        storeManager.moveSpeedLevel = gD.maxSpeedLevel;
        storeManager.cost = gD.cost;
    }

    public void resetGameData()
    {
        money = 0;
        gunBehaviour.reloadtime = defaultShootSpeed;
        healthSystem.maxHealth = defaultMaxHealth;
        gunBehaviour.damage = defaultBulletDamage;
        gD.currentMaxSpeed = player.GetComponent<CharacterMovement>().maxSpeed = defaultMaxMoveSpeed;
        roundManager.highestRound = 0;

        storeManager.fireSpeedLevel = 0;
        storeManager.healthLevel = 0;
        storeManager.damageLevel = 0;
        storeManager.moveSpeedLevel = 0;
        storeManager.cost = 5;

        saveGameData();
    }

    public void finishGame()
    {
        menuAni.SetBool("isInGame", false);

        PlayerPrefs.SetInt("isInStore", 0);

        player.SetActive(false);

        isInGame = false;

        highScoreText.text = "Highscore: " + roundManager.highestRound.ToString("0");
    }

    public void startGame()
    {
        //load game Data
        //menuUIElements.SetActive(false);
        // gameUIElements.SetActive(true);

        menuAni.SetBool("isInGame", true);
        PlayerPrefs.SetInt("isInStore", 1);

        isInGame = true;

        player.SetActive(true);

        healthSystem.startGame();

        clickSound.pitch = Random.Range(0.8f, 1.2f);
        clickSound.Play();
    }

    public void addMoney(int amount)
    {
        money += amount;

        moneyText.text = "$" + money.ToString("0");

        moneyAni.Play("MoneyIdle");
    }

    public void enterStore()
    {
        clickSound.pitch = Random.Range(0.8f, 1.2f);
        clickSound.Play();
        //Debug.Log(isInGame);
        if (isInStore)
        {
            isInStore = false;
            shopAni.SetBool("store", false);

            saveGameData();

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

    public bool isInGameScreen()
    {
        if (isInGame && !isInStore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Exit()
    {
        clickSound.pitch = Random.Range(0.8f, 1.2f);
        clickSound.Play();
        Application.Quit();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            saveGameData();
        }
        else
        {
            loadGameData();
        }
    }

    void OnApplicationQuit()
    {
        saveGameData();
    }
}
