using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float minSize;
    public float sizeLerpSpeed;

    public float healthAsPercent;

    public GameObject deathParticle;
    public Transform sprite;

    public SpriteRenderer healthSprite;
    public Gradient colourGradient;

    RoundManager roundManager;

    float currentHealth;

    void Start()
    {
        //maxHealth = sprite.localScale.x;
        // minSize = sprite.localScale.x - 0.3f;

        currentHealth = maxHealth;

        healthAsPercent = 1;

        roundManager = FindObjectOfType<RoundManager>();

        roundManager.enemiesOnScreen.Remove(gameObject);
    }

    public void takeDamage(float damage_)
    {
        currentHealth -= damage_;

        healthAsPercent = (currentHealth / maxHealth);

        Vector3 newScale = new Vector3(currentHealth, currentHealth, 1);

        healthSprite.color = colourGradient.Evaluate(healthAsPercent);

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
        GameObject deathexplosion_ = Instantiate(deathParticle, transform.position, transform.rotation);

        deathexplosion_.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);

        Destroy(deathexplosion_, 2f);

        FindObjectOfType<GameManager>().addMoney(10);

        roundManager.enemiesOnScreen.Remove(gameObject);

        Destroy(gameObject);
    }
}
