using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{

    private Animator animator;
    public Animator Doors;

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {

            animator.Play("OpenDoor");

        }
    }
}