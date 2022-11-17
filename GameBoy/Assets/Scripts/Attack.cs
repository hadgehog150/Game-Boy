using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform attackPosition;//точка/объект от которой будет спавниться колайдед. Создай отдельный объект и прекрепи его к персу
    public LayerMask enemy;//можно и через тег но я через слой сделал
    public float attackRadius;//радиус колайдера
    public GameObject projectile;

    private bool attacking;//атакует не атакует
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()//вот все что здесь это что бы анимация не срабатывала пока не закончится
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        attacking = stateInfo.IsName("atack_tree");

        if (attacking)
        {
            float playBackTime = stateInfo.normalizedTime;
        }
    }

    public void Attacking()
    {
        if (!attacking)
        {
            anim.SetTrigger("attacking");//триггер
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, enemy);//создание колайдера
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SendMessage("Attacked");//метод атаки из скрипка врага где у него hp отнимаются
            }
        }
    }

    void OnDrawGizmosSelected()//хрень для отрисовки 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
