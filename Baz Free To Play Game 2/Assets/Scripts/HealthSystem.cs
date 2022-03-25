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
    }

    void FixedUpdate()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, lerpSpeed);
        sliderImage.color = Color.Lerp(sliderImage.color, healthColours.Evaluate(currentHealth / 100), lerpSpeed);
    }

    public void looseHealth()
    {
        currentHealth -= 5;
    }

    public void dieAndReset()
    {
        FindObjectOfType<GameManager>().finishGame();

        currentHealth = maxHealth;

        transform.position = new Vector3(0, 0, 0);
    }


}
