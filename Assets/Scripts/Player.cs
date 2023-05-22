using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        Vector3 mov = Vector3.up * v * speed * Time.deltaTime;
        // transform.position += Time.deltaTime * speed * moveVector;
        transform.position += mov;
    }
    
    // Detect and handle collision with other objects
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("This is a log message.");
        switch (collision.gameObject.tag)
        {
            case "Border":
                Debug.Log("This is a log message.");
                break;
            default:
                break;
        }
    }
}
