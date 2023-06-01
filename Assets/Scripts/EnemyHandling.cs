using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandling : MonoBehaviour
{
    //TODO: later load from enemy_data 
    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject enemy; 
    [SerializeField] EnemyData data;
    Rigidbody2D rb;
    SpriteRenderer sr;
    int hp;
    bool destroyed = false;
    float shootIntervall = 1f;

    int direction = 1;

    // Start is called before the first frame update

    void Shoot()
    {
        Debug.Log("Shooting");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //TODO: load other data needed on Creation
        // speed = data.movespeed;
        // hp = data.hp;
        // sr.sprite = data.image;
        // shootIntervall = data.shootIntervall;

        InvokeRepeating("Shoot", 1f, shootIntervall);
    }

    private void FixedUpdate()
    {
        if(!destroyed)
        {
            rb.velocity =  direction * Vector3.up * speed * Time.fixedDeltaTime;
        }        
    }

    IEnumerator EnemyDestroyedEffects()
    {
        ParticleSystem tmp = Instantiate(explosion);
        tmp.transform.position = enemy.transform.position;
        tmp.Play();

        yield return new WaitForSeconds(1.5f);

        Destroy(enemy);
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
                //Instantiate(explosion, this.transform).Play();
                break;
            case "PlayerProjectile":
                hp -= 1;
                destroyed = hp == 0;
                // Remove projectile
                Destroy(collision.gameObject);
                if(destroyed)
                    StartCoroutine("EnemyDestroyedEffects");
                break;
            default:
                break;
        }

    }

}
