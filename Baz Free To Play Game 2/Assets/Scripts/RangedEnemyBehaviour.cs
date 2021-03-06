using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyBehaviour : MonoBehaviour
{
    public float enemyDifficulty;
    public float DefaultSpeed = 200;

    public float nextWayPointDistance = 3;
    public float distanceToRecalPos;

    public float reloadTime;
    public float fireForce;

    float distanceMoved;
    int currentWayPointIndex;

    bool reachedEndOfPath;
    bool canShoot;

    public GameObject bullet;
    public GameObject deathExplosion;

    public Transform shootPos;

    GameObject[] targets;
    Transform player;

    Path path_;

    Seeker seeker_;
    Vector3 lastPos;

    Rigidbody2D rb;

    GameManager gameManager;
    RoundManager roundManager;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker_ = GetComponent<Seeker>();

        enemyDifficulty = Random.Range(1f, 4f);

        targets = GameObject.FindGameObjectsWithTag("rangedTarget");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();
        roundManager = FindObjectOfType<RoundManager>();

        updatePath();

        distanceMoved = Mathf.Infinity;
        canShoot = true;

        InvokeRepeating("updatePath", 0, 2);
        Invoke("shoot", Random.Range(0f, 5f));
    }

    void updatePath()
    {
        seeker_.StartPath(rb.position, targets[Random.Range(0, targets.Length)].transform.position, hasFinishedCalculating);
    }

    void hasFinishedCalculating(Path p)
    {
        if (!p.error)
        {
            path_ = p;
            currentWayPointIndex = 0;
        }
    }

    void calcuateDistance()
    {
        distanceMoved = Vector3.Distance(transform.position, lastPos);

        lastPos = transform.position;
    }

    void Update()
    {
        transform.up = new Vector2(
            player.position.x - transform.position.x,
            player.position.y - transform.position.y
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

            float distance = Vector2.Distance(rb.position, path_.vectorPath[currentWayPointIndex]);

            /*
                        if (distanceMoved < 0.5f)
                        {
                            updatePath();
                            distanceMoved = Mathf.Infinity;
                        }*/

            if (distance < nextWayPointDistance)
            {
                currentWayPointIndex++;
            }
        }
        else
        {
            rb.drag = 200;

        }

        if (reachedEndOfPath)
        {
            updatePath();
        }
    }

    void shoot()
    {
        if (canShoot && gameManager.isInGameScreen())
        {
            StartCoroutine(reload());

            Vector2 dir = new Vector2(
                 player.position.x - shootPos.position.x,
                 player.position.y - shootPos.position.y
                 );

            // RaycastHit2D rayHitInfo = Physics2D.Raycast(shootPos.position, transform.up, Mathf.Infinity, 8);

            //Debug.DrawLine(shootPos.position, transform.up);

            /*
                    //see if we have line of sight with player
                    if (rayHitInfo.collider !=null)
                    {
                        Debug.Log(rayHitInfo.transform.gameObject);
                        if (rayHitInfo.transform.gameObject.CompareTag("Player"))
                        {
                            GameObject bulletInstance_ = Instantiate(bullet, shootPos.position, shootPos.rotation);
                            bulletInstance_.GetComponent<Rigidbody2D>().AddForce(shootPos.up * fireForce);
                        }
                    }
                    */

            GameObject bulletInstance_ = Instantiate(bullet, shootPos.position, shootPos.rotation);
            bulletInstance_.GetComponent<Rigidbody2D>().AddForce(shootPos.up * fireForce);
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

    IEnumerator reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
        shoot();
    }
}
