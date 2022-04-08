using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject shootParticle;
    public Transform shootPos;
    public Animator crossHairAni;
    public SpriteRenderer crosshairRend;
    public GameManager gameManager;

    public float shootForce;
    public float reloadtime;

    public float damage;

    bool canShoot = true;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            crossHairAni.SetBool("IsShooting", true);
        }
        else
        {
            crossHairAni.SetBool("IsShooting", false);
        }

        if (canShoot)
        {
            crosshairRend.color = Color.Lerp(crosshairRend.color, Color.green, 0.1f);
        }
        else
        {
            crosshairRend.color = Color.Lerp(crosshairRend.color, Color.red, 0.1f);
        }

        if (canShoot && Input.GetMouseButton(0) && gameManager.isInGameScreen() && !Input.GetKey(KeyCode.LeftShift))
        {
            canShoot = false;

            shoot();
            StartCoroutine(reload());
        }
    }

    public void increaseShootRate(float amount)
    {
        if (reloadtime > 0.2)
        {
            reloadtime -= amount;
        }
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadtime);
        canShoot = true;
    }

    public void shoot()
    {
        GameObject particleInstnace = Instantiate(shootParticle, shootPos.position, shootPos.rotation);
        GameObject bulletInstance = Instantiate(bullet, shootPos.position, shootPos.rotation);

        bulletInstance.GetComponent<Rigidbody2D>().AddForce(bulletInstance.transform.right * shootForce);
        bulletInstance.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);

        bulletInstance.GetComponent<BulletBehaviour>().baseDamage = damage;

        Destroy(particleInstnace, 2f);

    }
}
