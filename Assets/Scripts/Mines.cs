using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : MonoBehaviour
{
    // The gameobject of the projectile
    private GameObject bullet;
    
    // Awake is called before Start once
    void Awake() => bullet = this.gameObject;

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Destroy(bullet);
                break;
            case "EnemyProjectile":
                Destroy(bullet);
                break;
        }
    }
}
