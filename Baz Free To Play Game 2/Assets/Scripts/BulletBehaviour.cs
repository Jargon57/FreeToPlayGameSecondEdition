using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float blastRadius;
    public float blastForce;

    public float baseDamage;

    [Space]
    public AnimationCurve blastFalloff;
    public AnimationCurve damageFalloff;

    [Space]
    public GameObject trail;

    Transform trail_;

    void Start()
    {
        trail_ = Instantiate(trail, transform.position, transform.rotation).transform;
    }

    void Update()
    {
        trail_.position = transform.position;
    }

    void OnCollisionEnter2D(Collision2D col_)
    {
        /*
        //get a list of all colliders in a radius
        Collider2D[] cols_ = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach (Collider2D collider in cols_)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Vector3 collisionPoint = collider.transform.position;

                //calculate the direction the force should apply (DIRECTION)
                Vector2 directionVector = new Vector2(
                    collisionPoint.x - transform.position.x,
                    collisionPoint.y - transform.position.y
                );
                //calculate how much force to add based on distance and blast force (FORCE)
                float totalForce = blastFalloff.Evaluate(Vector3.Distance(transform.position, collisionPoint)) * blastForce;

                //add force in direction by force
                collider.GetComponentInParent<Rigidbody2D>().AddForce(directionVector * totalForce);

                //calculate how much damage to apply based on the distance between me and u
                float totalDamage = baseDamage * damageFalloff.Evaluate(Vector3.Distance(transform.position, collisionPoint));
            }
        }
        */

        if (col_.gameObject.CompareTag("Enemy"))
        {
            col_.gameObject.GetComponent<EnemyHealth>().takeDamage(baseDamage);
        }


        trail_.GetComponent<ParticleSystem>().emissionRate = 0;
        Destroy(trail_.gameObject, 1f);
        Destroy(gameObject);
    }
}
