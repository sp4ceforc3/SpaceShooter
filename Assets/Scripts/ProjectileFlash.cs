using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlash : MonoBehaviour
{
    // The gameobject of the projectile
    private GameObject bullet;

    // Rigbody -> Movement 
    private Rigidbody2D rb2d;

    // Impulse modifier / Speed
    [SerializeField] float speed = 15f;

    // Awake is called before Start once
    void Awake() { 
        bullet = this.gameObject;
        rb2d = bullet.GetComponent<Rigidbody2D>();
    }

    // Start is called before first frame update => let the bullet move
    void Start() {
        rb2d.AddForce(bullet.transform.up * speed, ForceMode2D.Impulse);
        InvokeRepeating(nameof(ZickZack), 0.5f, 0.25f);
    }

    // Detect and handle collision with other objects
    //      - Projectile should destroyed in any case
    //      - Other effects are handled by the collision gameobject itself, e.g. enemy
    void OnCollisionEnter2D(Collision2D collision) => Destroy(bullet);

    private void ZickZack() {
        rb2d.velocity = Vector3.zero;

        int upOrDown = 1;
        if (Random.value <= 0.5f)
            upOrDown = -1;

        bullet.transform.rotation = bullet.transform.rotation * Quaternion.Euler(0, 0, upOrDown * 45);
        rb2d.AddForce(bullet.transform.up * speed, ForceMode2D.Impulse);
    }
}
