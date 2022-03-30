using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPos;

    public float shootForce;
    public float reloadtime;

    public float damage;

    bool canShoot = true;

    void LateUpdate()
    {
        if (canShoot && Input.GetMouseButton (0) && PlayerPrefs.GetInt ("isInStore") == 0 && !Input.GetKey (KeyCode.LeftShift))
        {
            canShoot = false;

            shoot();
            StartCoroutine(reload());
        }
    }

    public void increaseShootRate()
    {
        if (reloadtime > 0.2)
        {
            reloadtime -= 0.1f;
        }
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadtime);
        canShoot = true;
    }

    public void shoot()
    {
        GameObject bulletInstance = Instantiate(bullet, shootPos.position, shootPos.rotation);

        bulletInstance.GetComponent<Rigidbody2D>().AddForce(bulletInstance.transform.right * shootForce);

        bulletInstance.GetComponent<BulletBehaviour>().baseDamage = damage;

        //Destroy(bulletInstance, 2f);
    }
}
