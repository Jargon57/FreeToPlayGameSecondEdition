using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float minSize;
    public float sizeLerpSpeed;

    public GameObject deathParticle;
    public Transform sprite;

    public SpriteRenderer healthSprite;
    public Gradient colourGradient;

    float currentHealth;

    void Start()
    {
        maxHealth = sprite.localScale.x;
        minSize = sprite.localScale.x - 0.3f;

        currentHealth = maxHealth;
    }

    public void takeDamage(float damage_)
    {
        currentHealth -= damage_;

        Vector3 newScale = new Vector3(currentHealth, currentHealth, 1);

        healthSprite.color = colourGradient.Evaluate(currentHealth);

        //sprite.localScale = Vector3.Lerp (sprite.localScale, newScale, sizeLerpSpeed);
    }

    void FixedUpdate()
    {
        if (currentHealth < 0)
        {
            die();
        }
    }

    void die()
    {
        //insan
        FindObjectOfType<GameManager>().addMoney(10);
        Destroy(gameObject);
    }
}
