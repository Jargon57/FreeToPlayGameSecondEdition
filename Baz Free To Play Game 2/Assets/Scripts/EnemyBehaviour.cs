using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour
{
    Transform player_;

    public float enemyDifficulty;
    public float DefaultSpeed = 200;

    public float nextWayPointDistance = 3;

    Path path_;

    Seeker seeker_;

    int currentWayPointIndex;

    bool reachedEndOfPath;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker_ = GetComponent<Seeker>();

        enemyDifficulty = Random.Range(1f, 4f);

        player_ = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("updatePath", 0, 0.5f);
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
        transform.right = new Vector2(
            player_.position.x - transform.position.x,
            player_.position.y - transform.position.y
        );
    }

    void FixedUpdate()
    {
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

        float disance = Vector2.Distance (rb.position, path_.vectorPath[currentWayPointIndex]);

        if (disance < nextWayPointDistance)
        {
            currentWayPointIndex++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision_)
    {
        if (collision_.gameObject.CompareTag("Player"))
        {
            collision_.gameObject.GetComponent<HealthSystem>().looseHealth();
            Destroy(gameObject);
        }
    }
}
