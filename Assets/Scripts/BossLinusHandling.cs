using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossLinusHandling : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject enemy; 
    [SerializeField] EnemyData data;
    [SerializeField] SpriteRenderer projectile;
    [SerializeField] SpriteRenderer laserBeam;
    [SerializeField] GameObject firePoint1;
    [SerializeField] GameObject firePoint2;
    [SerializeField] GameObject beamShootPoint;

    //Audio
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip explosionSFX;

    //UI
    [SerializeField] private Transform damagePopUp;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D cl;
    private GameObject firePoint;
    int hp;
    bool destroyed = false;
    float shootIntervall = 1f;

    int direction = 1;

    // Player -> Position
    private GameObject player;

    // Awake is called after creation 
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
    }

    void Shoot()
    {
        Instantiate(projectile, firePoint1.transform.position, firePoint1.transform.rotation);
        Instantiate(projectile, firePoint2.transform.position, firePoint2.transform.rotation);
    }
    
    void FireBeam()
    {
        Instantiate(laserBeam, beamShootPoint.transform.position, beamShootPoint.transform.rotation);
    }

    // Start is called before the first frame update
    void Start() {
        //TODO: load other data needed on Creation
        speed = data.movespeed;
        hp = data.hp;
        shootIntervall = data.shootIntervall;

        InvokeRepeating("Shoot", 1f, shootIntervall);
        InvokeRepeating("FireBeam", 8f, 10f);
    }

    private void FixedUpdate() {
        if(!destroyed) {
            // Movement
            rb.velocity =  direction * Vector3.up * speed * Time.fixedDeltaTime;

            // TODO: Increase properbility of rotation by wave
            if (true) {
                // Rotation
                Vector2 playerPos = (Vector2) player.transform.position;
                Vector2 lookDirection = ((Vector2) gameObject.transform.position - playerPos).normalized;
                gameObject.transform.up = lookDirection;
            }
        }        
    }

    IEnumerator EnemyDestroyedEffects()
    {
        Destroy(cl);
        ParticleSystem tmp = Instantiate(explosion);
        tmp.transform.position = enemy.transform.position;
        tmp.Play();
        // stay under explosion effect
        rb.velocity = new Vector3(0, 0, 0);
        Destroy(rb);
        sfx.PlayOneShot(explosionSFX, 1f);

        yield return new WaitForSeconds(1.5f);

        Destroy(enemy);
    }

    // Update is called once per frame
    void Update() {}

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
                CreateDamagePopUp();
                destroyed = hp == 0;
                if(destroyed)
                    StartCoroutine("EnemyDestroyedEffects");
                break;
            case "Mine":
                hp -= 1;
                CreateDamagePopUp();
                destroyed = hp == 0;
                if(destroyed)
                    StartCoroutine("EnemyDestroyedEffects");
                break;
                 
            default:
                break;
        }

    }

    private void CreateDamagePopUp() {        
        Transform popUpTrans = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        TextMeshPro popUpText = popUpTrans.GetComponent<TextMeshPro>();
        Rigidbody2D popUpRB = popUpTrans.GetComponent<Rigidbody2D>();

        popUpRB.velocity = rb.velocity;
        popUpText.SetText("-1");
        Destroy(popUpTrans.gameObject, 0.75f);
    }
}
