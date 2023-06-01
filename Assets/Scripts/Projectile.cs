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
        if (this.gameObject.tag == "PlayerProjectile") 
        {   // Player's projectile 
            switch (collision.gameObject.tag) {
                case "Border":
                    Destroy(this.gameObject);
                    break;

                case "Enemy":
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                    break;

                case "EnemyProjectile":
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                    break;
                
                default:
                    break;
            }
        } else {
            // Enemy's projectile            
            switch (collision.gameObject.tag) {
                case "Border":
                    Destroy(this.gameObject);
                    break;

                case "Enemy":
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                    break;

                case "Player":
                    // Ending game is handled inside player 
                    Destroy(this.gameObject);
                    break;
                
                default:
                    break;
            }
        }
    }
}
