using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;

    public Transform aimSquare;
    public Transform realGun;
    public Transform propGun;

    public float maxSpeed;
    public float accelerationSpeed;
    public float boostAmount;

    public float angularAcceleration;
    public float maxAngularVelocity;

    public float gunRotSpeed;

    public float drag;
    public float dragMultiplier;

    public bool isInStore;

    float boost;
    float leftRight;
    float updown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        cam = Camera.main;

        Application.targetFrameRate = 144;
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("isInStore") == 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                leftRight = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                leftRight = 1;
            }
            else
            {
                leftRight = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                updown = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                updown = -1;
            }
            else
            {
                updown = 0;
            }

            if (Input.GetMouseButton(0))
            {
                rb.drag = drag * dragMultiplier;
            }
            else
            {
                rb.drag = drag;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                boost = boostAmount;
            }else
            {
                boost = 1;
            }

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hitInformation = Physics2D.Raycast(mousePos2d, transform.forward);

            if (hitInformation.collider != null)
            {
                Vector2 direction = new Vector2(
                                        hitInformation.point.x - aimSquare.position.x,
                                        hitInformation.point.y - aimSquare.position.y
                );

                aimSquare.right = direction;

                propGun.position = transform.position;
                realGun.position = transform.position;

                realGun.transform.rotation = aimSquare.rotation;

                transform.rotation = Quaternion.Lerp(transform.rotation, aimSquare.rotation, gunRotSpeed);

                /*
                            if (Input.GetMouseButton(0))
                            {
                                transform.rotation = Quaternion.Lerp(transform.rotation, aimSquare.rotation, gunRotSpeed);
                                //transform.rotation = aimSquare.rotation;
                                propGun.transform.rotation = transform.rotation;
                            }else
                            {
                                propGun.transform.rotation = transform.rotation;
                            }
                            */

            }
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(Vector2.up * updown * accelerationSpeed * boost);
            rb.AddForce(Vector2.right * leftRight * accelerationSpeed * boost);
        }

        if (Mathf.Abs(rb.angularVelocity) < maxAngularVelocity)
        {
            // rb.AddTorque(angularAcceleration * -leftRight);
        }
    }
}
