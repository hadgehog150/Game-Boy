using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int hp;
    public float attackRadious;

    private GameObject player;
    private Animator animator;
    private Vector3 initialPosition, target;

    public GameObject projectile;
    public float attackSpeed = 2f;
    bool attacing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        target = initialPosition;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, attackRadious, 1 << LayerMask.NameToLayer("Default"));

        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;
            }
        }

        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;

        if (target != initialPosition && distance < attackRadious)
        {
            animator.SetFloat("X", dir.x);
            animator.SetFloat("Y", dir.y);
            animator.Play("walking_tree", -1, 0);

            if (!attacing) StartCoroutine(Attack(attackSpeed));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadious);
    }

    IEnumerator Attack(float seconds) {
        attacing = true;
        if (target != initialPosition && projectile != null) {
            Instantiate(projectile, transform.position, transform.rotation);
            yield return new WaitForSeconds(seconds);
        }
        attacing = false;
    }


    public void Attacked()
    {
        if (--hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
