using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] float speed = 1f;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject player;
    Rigidbody2D rb;
    bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if(!destroyed)
        {
            rb.velocity =  Input.GetAxisRaw("Vertical") * Vector3.up * speed * Time.fixedDeltaTime;
        }        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    // TODO: Expand this function with music fading and return to Main Menu (because delay needed)
    IEnumerator PlayerLooseEffects()
    {
        ParticleSystem tmp = Instantiate(explosion);
        tmp.transform.position = player.transform.position;
        tmp.Play();

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
                destroyed = true;
                StartCoroutine("PlayerLooseEffects");
                // TODO: Save the Highcore here or in the case statement
                break;
            default:
                break;
        }

    }
    // Detect and handle collision with other objects
    void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
