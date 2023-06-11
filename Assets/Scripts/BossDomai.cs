using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDomai : EnemyHandling
{
    [SerializeField] GameObject firePointR;
    [SerializeField] GameObject firePointC;
    [SerializeField] GameObject firePointL;
    [SerializeField] SpriteRenderer projectileC;

    private float lastShot = 0f;

    public override void Movement()
    {       
        // Rotation
        Vector2 playerPos = (Vector2) player.transform.position;
        Vector2 lookDirection = ((Vector2) gameObject.transform.position - playerPos).normalized;
        gameObject.transform.up = lookDirection;

        // Movement
        if(rb != null)
            rb.velocity = -direction * gameObject.transform.up * speed * 0.25f*Time.fixedDeltaTime;

        // Shooting
        if ((lastShot - Time.time) <= -1.5f) {
            lastShot = Time.time;

            Instantiate(projectile, firePointR.transform.position, firePointR.transform.rotation * Quaternion.Euler(0, 0, 15));
            Instantiate(projectile, firePointL.transform.position, firePointL.transform.rotation * Quaternion.Euler(0, 0, -15));
            Instantiate(projectileC, firePointC.transform.position, firePointC.transform.rotation);

            if (Random.value <= 0.33f) {
                Vector3 fireDirection = firePointC.transform.up.normalized;
                Instantiate(projectileC, firePointC.transform.position + 2.2f*fireDirection, firePointC.transform.rotation);
                Instantiate(projectileC, firePointC.transform.position + 1.2f*fireDirection, firePointC.transform.rotation * Quaternion.Euler(0, 0, 10));
                Instantiate(projectileC, firePointC.transform.position, firePointC.transform.rotation * Quaternion.Euler(0, 0, -10));
            }
        }
    }
}
