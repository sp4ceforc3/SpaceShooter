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
    //      - Projectile should destroyed in any case
    //      - Other effects are handled by the collision gameobject itself, e.g. enemy
    void OnCollisionEnter2D(Collision2D collision) => Destroy(bullet);
}
