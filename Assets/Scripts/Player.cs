using UnityEngine;
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
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip godmodeSFX;
    [SerializeField] AudioMixer audioMixer;

    Rigidbody2D rb;
    bool destroyed = false;
    bool godmode = false;

    // Shooting variables
    private bool shooting = false;
    private float lastShot = 0f;
    private GameObject firePoint;

    // Highscore = number of survived waves
    private int highscore = 0;

    // Input System to control the player
    private PlayerControls playerControls;
    private InputAction movement;

    // Enble input manager when object is enabled
    private void OnEnable() {
        // Keyboard: "WASD"
        movement = playerControls.Player.Move;
        movement.Enable();

        // Keyboard: "Space"
        playerControls.Player.Fire.started += _ => { shooting = true; };
        playerControls.Player.Fire.canceled += _ => { shooting = false; };
        playerControls.Player.Fire.Enable();

        // Keyboard: "1"
        playerControls.Player.Special.performed += _ => { /* TODO: */ };
        playerControls.Player.Special.Enable();

        // Keyboard: "2"
        playerControls.Player.SpecialDomai.performed += _ => { /* TODO: */ };
        playerControls.Player.Special.Enable();

        // Keyboard: "3"
        playerControls.Player.SpecialLinus.performed += _ => { /* TODO: */ };
        playerControls.Player.Special.Enable();

        // Keyboard: "G"
        playerControls.Player.Godmode.performed += _ => {
            godmode = !godmode;
            sfx.PlayOneShot(godmodeSFX, 1f);    // play sound at change
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
    void Awake()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MasterVolume", 2f, 1));
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        firePoint = gameObject.transform.GetChild(0).gameObject;
    }

    private void FixedUpdate() {
        if(!destroyed) {
            rb.velocity = movement.ReadValue<Vector2>() * speed * Time.fixedDeltaTime;

            // Time between each shots has to be a half second or more.
            // Otherwise skip starting coroutine for shooting.
            if (shooting && (lastShot - Time.time) <= -0.5f) {
                lastShot = Time.time;
                StartCoroutine(nameof(Shoot));
            }
        }
    }

    // Update is called once per frame
    void Update() {}
    
    // TODO: Expand this function with music fading and return to Main Menu (because delay needed)
    IEnumerator PlayerLooseEffects()
    {
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
    private void Shoot() {
        sfx.PlayOneShot(shootSFX, 1f);
        Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
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

            case "PlayerProjectile":
                Destroy(collision.gameObject);
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
