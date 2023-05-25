using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] float speed = 1f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
       rb.velocity =  Input.GetAxisRaw("Vertical") * Vector3.up * speed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("This is a log message.");
        switch (collision.gameObject.tag)
        {
            case "Border":
                Debug.Log("This is a log message.");
                rb.velocity = new Vector3(0, 0, 0);
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
