using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public Image sliderImage;

    [Space]
    public int maxHealth;
    public float lerpSpeed;

    [Space]
    public Gradient healthColours;

    [SerializeField] float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    void FixedUpdate()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, lerpSpeed);
        sliderImage.color = Color.Lerp(sliderImage.color, healthColours.Evaluate(currentHealth / 100), lerpSpeed);

        healthSlider.maxValue = maxHealth;
    }

    public void looseHealth()
    {
        currentHealth -= 5;

        if (currentHealth <= 0)
        {
            dieAndReset();
        }
    }

    public void startGame()
    {
        currentHealth = maxHealth;
    }

    public void dieAndReset()
    {
        FindObjectOfType<GameManager>().finishGame();

        currentHealth = maxHealth;

        transform.position = new Vector3(0, 0, 0);
    }


}
