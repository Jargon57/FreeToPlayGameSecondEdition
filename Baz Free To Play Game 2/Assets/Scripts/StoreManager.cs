using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public float david = 0.5f;
    public float moneyScaleAmount = 1.5f;

    public int cost = 10;

    public int fireSpeedLevel;
    public int moveSpeedLevel;
    public int damageLevel;
    public int healthLevel;

    [Space]
    public float reloadDecrease = -0.1f;
    public float movementIncrease = 0.5f;
    public float damageIncrease = 0.05f;
    public int healthIncrease = 10;

    [Space]
    public Slider fireSpeedSlider;
    public Slider moveSpeedSlider;
    public Slider damageSlider;
    public Slider healthSlider;

    [Space]
    public Text[] ButtonTexts;

    public Text shootSpeedImprovement;
    public Text moveSpeedImprovement;
    public Text bulletDamageImprovement;

    GameManager gameManager;

    [Space]
    [Header("Misc")]
    public Text shopMoneyText;

    public HealthSystem healthSystem;

    void FixedUpdate()
    {
        fireSpeedSlider.value = Mathf.Lerp(fireSpeedSlider.value, fireSpeedLevel, david);
        moveSpeedSlider.value = Mathf.Lerp(moveSpeedSlider.value, moveSpeedLevel, david);
        damageSlider.value = Mathf.Lerp(damageSlider.value, damageLevel, david);
        shopMoneyText.text = "You have $" + gameManager.money.ToString("00");
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Invoke("initialise", 0.1f);
    }

    void initialise()
    {
        for (int i = 0; i < ButtonTexts.Length; i++)
        {
            ButtonTexts[i].text = "$" + cost;
        }

        shootSpeedImprovement.text = reloadDecrease.ToString("0.0") + "!";
        moveSpeedImprovement.text = movementIncrease.ToString("0.0") + "!";
        bulletDamageImprovement.text = damageIncrease.ToString("0.00") + "!";
    }

    void upgradeGeneral()
    {
        //shopMoneyText.text = "You have $" + gameManager.money.ToString("00");

        shopMoneyText.GetComponent<Animator>().SetTrigger("looseMoney");

        FindObjectOfType<GameManager>().addMoney(-cost);

        cost = (int)(cost * 1.5f);

        for (int i = 0; i < ButtonTexts.Length; i++)
        {
            ButtonTexts[i].text = "$" + cost;
        }
    }

    public void upgradeFireSpeed()
    {
        if (gameManager.money >= cost)
        {
            if (fireSpeedLevel < 10)
            {
                fireSpeedLevel++;
            }

            upgradeGeneral();

            FindObjectOfType<Shoot>().increaseShootRate(reloadDecrease);
        }
    }

    public void upgradeMoveSpeed()
    {
        if (gameManager.money >= cost)
        {
            if (moveSpeedLevel < 10)
            {
                moveSpeedLevel++;
            }

            upgradeGeneral();

            FindObjectOfType<CharacterMovement>().maxSpeed += movementIncrease;
        }
    }

    public void upgradeDamage()
    {
        if (gameManager.money >= cost)
        {
            if (damageLevel < 10)
            {
                damageLevel++;
            }

            upgradeGeneral();

            FindObjectOfType<Shoot>().damage += damageIncrease;
        }
    }

    public void upgradeHealth()
    {
        if (gameManager.money >= cost)
        {
            if (healthLevel < 10)
            {
                healthLevel++;
            }

            upgradeGeneral();

            healthSystem.maxHealth += healthIncrease;
            // FindObjectOfType<HealthSystem>().maxHealth += healthIncrease;
        }
    }
}
