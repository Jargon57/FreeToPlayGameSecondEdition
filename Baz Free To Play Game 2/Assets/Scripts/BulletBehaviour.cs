using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float blastRadius;
    public float blastForce;

    [Space]
    public AnimationCurve falloff;

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

    void OnCollisionEnter2D()
    {
        //get a list of all colliders in a radius
        Collider2D[] cols_ = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach (Collider2D collider in cols_)
        {
            if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Player"))
            {
                //calculate the direction the force should apply (DIRECTION)
                Vector3 collisionPoint = collider.transform.position;
                Vector2 directionVector = new Vector2(
                    collisionPoint.x - transform.position.x,
                    collisionPoint.y - transform.position.y
                );
                //calculate how much force to add based on distance and blast force (FORCE)
                float totalForce = falloff.Evaluate(Vector3.Distance(transform.position, collisionPoint)) * blastForce;

                //add force in direction by force
                collider.GetComponent<Rigidbody2D>().AddForce(directionVector * totalForce);
            }
        }

        trail_.GetComponent<ParticleSystem>().emissionRate = 0;
        Destroy(trail_.gameObject, 1f);
        Destroy(gameObject);
    }
}
