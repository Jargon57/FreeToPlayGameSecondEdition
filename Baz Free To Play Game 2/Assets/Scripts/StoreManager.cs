using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public float david = 0.5f;

    public int cost = 10;

    public int fireSpeedLevel;
    public int moveSpeedLevel;
    public int damageLevel;

    [Space]
    public float reloadDecrease = -0.1f;
    public float movementIncrease = 0.5f;
    public float damageIncrease = 0.05f;

    [Space]
    public Slider fireSpeedSlider;
    public Slider moveSpeedSlider;
    public Slider damageSlider;

    [Space]
    public Text[] ButtonTexts;

    public Text shootSpeedImprovement;
    public Text moveSpeedImprovement;
    public Text bulletDamageImprovement;

    void FixedUpdate()
    {
        fireSpeedSlider.value = Mathf.Lerp(fireSpeedSlider.value, fireSpeedLevel, david);
        moveSpeedSlider.value = Mathf.Lerp(moveSpeedSlider.value, moveSpeedLevel, david);
        damageSlider.value = Mathf.Lerp(damageSlider.value, damageLevel, david);
    }

    void Start()
    {
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

    public void upgradeFireSpeed()
    {
        if (FindObjectOfType<GameManager>().money >= cost)
        {
            if (fireSpeedLevel < 10)
            {
                fireSpeedLevel++;
            }

            FindObjectOfType<GameManager>().addMoney(-cost);
            cost = (int)(cost * 1.5f);

            for (int i = 0; i < ButtonTexts.Length; i++)
            {
                ButtonTexts[i].text = "$" + cost;
            }

            FindObjectOfType<Shoot>().increaseShootRate(reloadDecrease);
        }
    }

    public void upgradeMoveSpeed()
    {
        if (FindObjectOfType<GameManager>().money >= cost)
        {
            if (moveSpeedLevel < 10)
            {
                moveSpeedLevel++;
            }

            FindObjectOfType<GameManager>().addMoney(-cost);
            cost = (int)(cost * 1.5f);

            for (int i = 0; i < ButtonTexts.Length; i++)
            {
                ButtonTexts[i].text = "$" + cost;
            }

            FindObjectOfType<CharacterMovement>().maxSpeed += movementIncrease;
        }
    }

    public void upgradeDamage()
    {
        if (FindObjectOfType<GameManager>().money >= cost)
        {
            if (damageLevel < 10)
            {
                damageLevel++;
            }

            FindObjectOfType<GameManager>().addMoney(-cost);
            cost = (int)(cost * 1.5f);

            for (int i = 0; i < ButtonTexts.Length; i++)
            {
                ButtonTexts[i].text = "$" + cost;
            }

            FindObjectOfType<Shoot>().damage += damageIncrease;
        }
    }
}
