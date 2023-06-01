using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // The gameobject of th projectile
    private GameObject bullet;

    // Impulse modifier / Speed
    [SerializeField] float speed = 15f;

    // Awake is called before Start once
    void Awake() => bullet = this.gameObject;

    // Start is called before first frame update => let the bullet move
    void Start() => bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up * speed, ForceMode2D.Impulse);

    // Detect and handle collision with other objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag) {
            case "Border":
                Destroy(bullet);
                break;

            case "Enemy":
                //! Should be handled by the enemy itself.
                //! Similiar to Player object.
                Destroy(collision.gameObject); // -> no difference between player and enemy projectiles

                Destroy(bullet);
                break;

            case "EnemyProjectile":
                // Destroy both projectiles
                Destroy(collision.gameObject);
                Destroy(bullet);
                break;

            case "Player":
                // Handled by Player object
                Destroy(bullet);
                break;

            case "PlayerProjectile":
                // Destroy both projectiles
                Destroy(collision.gameObject);
                Destroy(bullet);
                break;
            
            default:
                break;
        }
    }
}
