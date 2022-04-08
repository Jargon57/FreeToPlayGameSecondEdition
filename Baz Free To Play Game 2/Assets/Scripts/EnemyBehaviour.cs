using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour
{
    public float enemyDifficulty;
    public float DefaultSpeed = 200;
    public float nextWayPointDistance = 3;

    public GameObject deathExplosion;

    int currentWayPointIndex;

    bool reachedEndOfPath;

    Transform player_;

    Path path_;

    Seeker seeker_;

    Rigidbody2D rb;

    GameManager gameManager;
    [SerializeField] RoundManager roundManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker_ = GetComponent<Seeker>();

        enemyDifficulty = Random.Range(1f, 4f);

        player_ = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("updatePath", 0, 0.1f);

        gameManager = FindObjectOfType<GameManager>();
        roundManager = FindObjectOfType<RoundManager>();

        roundManager.enemiesOnScreen.Add(gameObject);
    }

    void updatePath()
    {
        seeker_.StartPath(rb.position, player_.position, hasFinishedCalculating);
    }

    void hasFinishedCalculating(Path p)
    {
        if (!p.error)
        {
            path_ = p;
            currentWayPointIndex = 0;
        }
    }


    void Update()
    {
        transform.up = new Vector2(
            player_.position.x - transform.position.x,
            player_.position.y - transform.position.y
        );
    }

    void FixedUpdate()
    {
        if (gameManager.isInGameScreen())
        {
            rb.drag = 1;
            if (path_ == null) return;

            if (currentWayPointIndex >= path_.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 dir = ((Vector2)path_.vectorPath[currentWayPointIndex] - rb.position).normalized;
            Vector2 force = dir * DefaultSpeed * enemyDifficulty;

            rb.AddForce(force);

            float disance = Vector2.Distance(rb.position, path_.vectorPath[currentWayPointIndex]);

            if (disance < nextWayPointDistance)
            {
                currentWayPointIndex++;
            }
        }
        else
        {
            rb.drag = 200;
        }
    }

    public void Die()
    {
        GameObject deathexplosion_ = Instantiate(deathExplosion, transform.position, transform.rotation);

        deathExplosion.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);

        Destroy(deathexplosion_, 2f);

        roundManager.enemiesOnScreen.Remove(gameObject);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision_)
    {
        if (collision_.gameObject.CompareTag("Player"))
        {
            collision_.gameObject.GetComponent<HealthSystem>().looseHealth();

            collision_.gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            collision_.gameObject.GetComponent<AudioSource>().Play();

            Die();
        }
    }

}
