using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] SpriteRenderer projectile;
    [SerializeField] GameObject player;
    [SerializeField] GameObject minefield;
    [SerializeField] GameObject mine;

    Rigidbody2D rb;
    Collider2D cl; 
    bool destroyed = false;
    bool godmode = false;

    // UI
    [SerializeField] Image drippleLaser;

    // Audio 
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip godmodeSFX;
    [SerializeField] AudioClip specialSFX;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioMixer audioMixer;

    // Shooting variables
    private float lastShot = -0.5f;
    private GameObject firePoint;
    private bool shooting = false;
    private float lastSpecialShot = -3f;
    private float specialCoolDown = 3f;
    int numberOfMines = 20;

    // Highscore = number of survived waves
    private int highscore = 0;

    // Input System to control the player
    private PlayerControls playerControls;
    private InputAction movement;
    private InputAction look;

    // Enble input manager when object is enabled
    private void OnEnable() {
        // Keyboard: "WASD"
        movement = playerControls.Player.Move;
        movement.Enable();

        // Mousepointer
        look = playerControls.Player.Look;
        look.Enable();

        // Keyboard: "Space"
        playerControls.Player.Fire.started += _ => { shooting = true; };
        playerControls.Player.Fire.canceled += _ => { shooting = false; };
        playerControls.Player.Fire.Enable();

        // Keyboard: "1"
        playerControls.Player.Special.performed += _ => { Shoot(isSpecial: true); };
        playerControls.Player.Special.Enable();

        // Keyboard: "2"
        playerControls.Player.SpecialDomai.performed += _ => { /* TODO: */ };
        playerControls.Player.SpecialDomai.Enable();

        // Keyboard: "3"
        playerControls.Player.SpecialLinus.performed += _ => { ShootLinusSpecial(); };
        playerControls.Player.SpecialLinus.Enable();

        // Keyboard: "G"
        playerControls.Player.Godmode.performed += _ => {
            godmode = !godmode;
            // play sound at change
            sfx.PlayOneShot(godmodeSFX, 1f);
        };
        playerControls.Player.Godmode.Enable();
    }
    
    // Disable input manager when object is disabled
    private void OnDisable() {
        movement.Disable();
        playerControls.Player.Fire.Disable();
        playerControls.Player.Godmode.Disable();
        playerControls.Player.Special.Disable();
        playerControls.Player.SpecialDomai.Disable();
        playerControls.Player.SpecialLinus.Disable();
    }

    // Awake is called before Start
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        playerControls = new PlayerControls();
        firePoint = gameObject.transform.GetChild(0).gameObject;
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MasterVolume", 2f, 1));
    }

    private void FixedUpdate() {
        if(!destroyed) {
            // Movement
            rb.velocity = movement.ReadValue<Vector2>() * speed * Time.fixedDeltaTime;

            // Rotation
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());
            Vector2 direction = (mouseScreenPosition - (Vector2) gameObject.transform.position).normalized;
            gameObject.transform.up = direction;

            // Time between each shots has to be a half second or more.
            // Otherwise skip starting coroutine for shooting.
            if (shooting && (lastShot - Time.time) <= -0.5f) {
                lastShot = Time.time;
                Shoot(isSpecial: false);
            }

            if (drippleLaser.fillAmount < 1f)
                drippleLaser.fillAmount += 0.9f / (specialCoolDown / Time.fixedDeltaTime);
        }
    }

    // Update is called once per frame
    void Update() {}
    
    // TODO: Expand this function with music fading and return to Main Menu (because delay needed)
    IEnumerator PlayerLooseEffects()
    {
        Destroy(cl);
        ParticleSystem tmp = Instantiate(explosion);
        tmp.transform.position = player.transform.position;
        tmp.Play();
        sfx.PlayOneShot(explosionSFX, 1f);
        // Player should stay under explosion
        rb.velocity = new Vector3(0, 0, 0);
        Destroy(rb);

        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MasterVolume", 1.5f, 0f));

        yield return new WaitForSeconds(1.5f);

        Destroy(player);
        SetHighScore();
        SceneManager.LoadScene("MainMenu");
    }

    // Fire one projectile
    private void Shoot(bool isSpecial) {
        if(!destroyed) {
            if (!isSpecial) {
                sfx.PlayOneShot(shootSFX, 1f);
                Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            } else if (lastSpecialShot - Time.time <= -specialCoolDown) {
                sfx.PlayOneShot(specialSFX, 1f);

                lastSpecialShot = Time.time;
                drippleLaser.fillAmount = 0f;

                Vector3 fireDirection = firePoint.transform.up.normalized;
                Instantiate(projectile, firePoint.transform.position + 2*fireDirection, firePoint.transform.rotation);
                Instantiate(projectile, firePoint.transform.position + fireDirection, firePoint.transform.rotation);
                Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            }
        }
    }

    private void ShootLinusSpecial() {
        if(!destroyed)
        {
            Debug.Log("Linus Special");
            for(int i = 0; i < numberOfMines; i++)
            {
                Transform mft = minefield.transform;
                float rangeXBounds = -(mft.lossyScale.x - 1) / 2;
                float rangeYBounds = -(mft.lossyScale.y - 1) / 2;
                float x = Random.Range(- rangeXBounds, rangeXBounds);
                float y = Random.Range(- rangeYBounds, rangeYBounds);
                Vector3 spawnPos = new Vector3(x, y, 0); 
                Instantiate(mine, spawnPos, mine.transform.rotation);
            }
                 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
           case "EnemyProjectile":
                if(!godmode) {
                    rb.velocity = new Vector3(0, 0, 0);
                    destroyed = true;
                    StartCoroutine(nameof(PlayerLooseEffects));
                }
                break;
        }
    }
    // Detect and handle collision with other objects
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Border":
                rb.velocity = new Vector3(0, 0, 0);
                break;

            case "Enemy":
                if(!godmode) {
                    rb.velocity = new Vector3(0, 0, 0);
                    destroyed = true;
                    Destroy(collision.gameObject);
                    StartCoroutine(nameof(PlayerLooseEffects));
                } else {
                    Destroy(collision.gameObject);
                }
                break;

            case "EnemyProjectile":
                if(!godmode) {
                    rb.velocity = new Vector3(0, 0, 0);
                    destroyed = true;
                    Destroy(collision.gameObject);
                    StartCoroutine(nameof(PlayerLooseEffects));
                } else {
                    Destroy(collision.gameObject);
                }
                break;
            
            default:
                break;
        }

    }

    // Set highscore if it is really the highest score 
    private void SetHighScore() {
        if (PlayerPrefs.GetInt(nameof(highscore)) < highscore)
            PlayerPrefs.SetInt(nameof(highscore), highscore);
    }

}
