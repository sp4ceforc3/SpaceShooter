using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHandling : MonoBehaviour
{
    //TODO: later load from enemy_data 
    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject player; 
    [SerializeField] EnemyData data;
    Rigidbody2D rb;
    bool destroyed = false;

    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    private void FixedUpdate()
    {
        if(!destroyed)
        {
            rb.velocity =  direction * Vector3.up * speed * Time.fixedDeltaTime;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Border":
                direction *= -1;
                // TODO: This is just a Test dummy to see, whether explosion works
                Instantiate(explosion, this.transform).Play();
                break;
            default:
                break;
        }

    }

}
