using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHandling : MonoBehaviour
{
    //TODO: later load from enemy_data 
    [SerializeField] protected float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject enemy; 
    [SerializeField] EnemyData data;
    [SerializeField] protected SpriteRenderer projectile;

    //Audio
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip explosionSFX;

    protected Rigidbody2D rb;
    SpriteRenderer sr;
    bool srFound = false;
    protected Collider2D cl;
    GameObject firePoint;
    int hp;
    protected bool destroyed = false;
    float shootIntervall = 1f;
    protected int direction = 1;

    // WaveHandler
    public WaveHandler waveHandlerScript;
    
    // UI
    [SerializeField] private Transform damagePopUp;

    // Player -> Position
    protected GameObject player;

    // Awake is called after creation 
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        srFound = TryGetComponent<SpriteRenderer>(out sr);
        cl = GetComponent<Collider2D>();
        firePoint = gameObject.transform.GetChild(0).gameObject;
    }

    void Shoot(){
        if(!destroyed)
            Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        //TODO: load other data needed on Creation
        speed = data.movespeed;
        hp = data.hp;
        if(srFound)
            sr.sprite = data.image;
        shootIntervall = data.shootIntervall;

        InvokeRepeating("Shoot", 1f, shootIntervall);
    }

    public virtual void Movement()
    {
        // Movement
        rb.velocity = direction * Vector3.up * speed * Time.fixedDeltaTime;

        // TODO: Increase properbility of rotation by wave
        if (true) {
            // Rotation
            Vector2 playerPos = (Vector2) player.transform.position;
            Vector2 lookDirection = ((Vector2) gameObject.transform.position - playerPos).normalized;
            gameObject.transform.up = lookDirection;
        }
    }

    private void FixedUpdate() {
        if(!destroyed)
            Movement();            
    }

    IEnumerator EnemyDestroyedEffects()
    {
       
        Destroy(cl);
        ParticleSystem tmp = Instantiate(explosion);
        tmp.transform.position = enemy.transform.position;
        tmp.Play();
        // stay under explosion effect
        rb.velocity = new Vector3(0, 0, 0);
        sfx.PlayOneShot(explosionSFX, 1f);
        Destroy(rb);

        yield return new WaitForSeconds(1.5f);

        waveHandlerScript.enemiesLeft -= 1;
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
                break;
            case "PlayerProjectile":
                hp -= 1;
                CreateDamagePopUp();
                destroyed = hp == 0;
                if(destroyed)
                    StartCoroutine(nameof(EnemyDestroyedEffects));
                break;
            case "Mine":
                hp -= 1;
                CreateDamagePopUp();
                destroyed = hp == 0;
                if(destroyed)
                    StartCoroutine(nameof(EnemyDestroyedEffects));
                break;
            case "Player":
                StartCoroutine(nameof(EnemyDestroyedEffects));
                break;
            default:
                break;
        }

    }

    // Popup over Enemy that show the taken damage
    private void CreateDamagePopUp() {
        Transform popUpTrans = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        TextMeshPro popUpText = popUpTrans.GetComponent<TextMeshPro>();
        Rigidbody2D popUpRB = popUpTrans.GetComponent<Rigidbody2D>();
        popUpRB.velocity = rb.velocity;
        popUpText.SetText("-1");
        Destroy(popUpTrans.gameObject, 0.75f);
    }
}
