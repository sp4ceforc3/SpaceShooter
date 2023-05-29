using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject player;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip godmodeSFX;
    Rigidbody2D rb;
    bool destroyed = false;
    bool godmode = false;

    // Input System to control the player
    private PlayerControls playerControls;
    private InputAction movement;

    // Enble input manager when object is enabled
    private void OnEnable() {
        movement = playerControls.Player.Move;
        movement.Enable();

        playerControls.Player.Fire.performed += Shoot;
        playerControls.Player.Fire.Enable();
    }
    
    // Disable input manager when object is disabled
    private void OnDisable() {
        movement.Disable();
        playerControls.Player.Fire.Disable();
    }

    // Awake is called before Start
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
    }

    private void FixedUpdate() {
        if(!destroyed)
        {
            //rb.velocity =  Input.GetAxisRaw("Vertical") * Vector3.up * speed * Time.fixedDeltaTime;
            rb.velocity = movement.ReadValue<Vector2>() * speed * Time.fixedDeltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
                godmode = !godmode;
                if(godmode)
                {
                    sfx.PlayOneShot(godmodeSFX, 1f);
                }
    }
    
    // TODO: Expand this function with music fading and return to Main Menu (because delay needed)
    IEnumerator PlayerLooseEffects()
    {
        ParticleSystem tmp = Instantiate(explosion);
        tmp.transform.position = player.transform.position;
        tmp.Play();
        sfx.PlayOneShot(explosionSFX, 1f);

        yield return new WaitForSeconds(1.5f);

        Destroy(player);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Border":
                rb.velocity = new Vector3(0, 0, 0);
                // TODO: This is just a Test dummy to see, whether explosion works
                Instantiate(explosion, this.transform).Play();
                break;
            case "EnemyProjectile":
                if(!godmode)
                {
                    destroyed = true;
                    Destroy(collision.gameObject);
                    StartCoroutine("PlayerLooseEffects");
                    // TODO: Save the Highcore here or in the case statement
                }
                break;
            default:
                break;
        }

    }
    // Detect and handle collision with other objects
    void OnTriggerEnter2D(Collider2D collision)
    {

    }

    // The Player shoot / press mouse button 
    private void Shoot(InputAction.CallbackContext ctx) {
        //TODO: Implement shoot
        sfx.PlayOneShot(shootSFX, 1f);
    }
}
