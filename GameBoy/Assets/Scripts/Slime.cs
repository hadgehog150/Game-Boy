using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int hp;
    public float speed;
    public float damageTimer;
    public float damageTime;
    public float visionRadious;
    public Transform[] move_spots;
    public GameObject[] loot;

    private Transform targets;
    private GameObject player;
    private Rigidbody2D rigidbody2D;
    private int spots;
    private bool angry;
    private bool canDamage;
    private Vector3 initialPosition;

    [SerializeField] private float thrust;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 target = initialPosition;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, visionRadious, 1 << LayerMask.NameToLayer("Default"));

        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;
                angry = true;
            }
        }

        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = target - transform.position;

        if (angry)
        {
            angry = false;
            rigidbody2D.MovePosition(transform.position + dir * speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, move_spots[spots].position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, move_spots[spots].position) < 0.2f)
            {
                if (spots == move_spots.Length - 1)
                {
                    spots = 0;
                    targets = move_spots[0];
                }
                else
                {
                    spots++;
                    targets = move_spots[spots];
                }
            }
        }


        if (!canDamage)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0)
            {
                canDamage = true;
                damageTimer = damageTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (canDamage)
            {
                collision.SendMessage("Attacked");
                canDamage = false;
            }
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D player = collision.GetComponent<Rigidbody2D>();
            if (player != null)
            {
                StartCoroutine(KnockCoroutine(player));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadious);
    }


    public void Attacked()
    {
        if (--hp <= 0)
        {
            int random = Random.Range(0, loot.Length);
            Instantiate(loot[random], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Can this be put in a separate file?
    private IEnumerator KnockCoroutine(Rigidbody2D player)
    {
        Vector2 forceDirection = player.transform.position - transform.position;
        Vector2 force = forceDirection.normalized * thrust;

        player.velocity = force;
        yield return new WaitForSeconds(.3f);

        player.velocity = new Vector2();
    }
}
