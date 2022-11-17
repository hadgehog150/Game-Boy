using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public int hp;
    public int maxHp;
    public GameObject attackCollider;
    public Joystick joystick;

    private float x, y;
    private bool isWalking;
    private Rigidbody2D rigidbody;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        x = joystick.Horizontal;
        y = joystick.Vertical;

        if (x != 0 || y != 0)
        {
            if (!isWalking)
            {
                isWalking = true;
                anim.SetBool("isWalking", isWalking);
            }
            Move();

            Vector3 xy;
            xy = new Vector2(Mathf.RoundToInt(x), Mathf.RoundToInt(y)) / 2;

            attackCollider.transform.position = transform.position + xy;
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                anim.SetBool("isWalking", isWalking);
            }
        }
    }

    public void Move() 
    {
        anim.SetFloat("X", x);
        anim.SetFloat("Y", y);

        transform.Translate(x * Time.deltaTime * speed, y * Time.deltaTime * speed, 0);
    }

    public void Attacked()
    {
        if (--hp <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnGUI()
    {
        Vector2 position = Camera.main.WorldToScreenPoint(transform.position);

        GUI.Box(
            new Rect(
                position.x - 20,
                Screen.height - position.y + 60,
                40,
                20
                ), hp + "/" + maxHp
            ); 
        ;
    }
}
