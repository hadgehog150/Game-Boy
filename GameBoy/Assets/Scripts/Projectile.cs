using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    GameObject player;
    Rigidbody2D rigidbody;
    Vector3 target, dir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();

        if (player != null) {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }

    private void FixedUpdate()
    {
        if (target != Vector3.zero) {
            rigidbody.MovePosition(transform.position + (dir * speed) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player") {
            Destroy(gameObject);
        }

        if (collision.tag == "Player")
        {
            collision.SendMessage("Attacked");
        }
    }

    public void Attacked()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
