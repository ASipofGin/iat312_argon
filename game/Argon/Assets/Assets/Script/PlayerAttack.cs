using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;

    public bool canAttack = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack == true){
            if(timeBtwAttack <= 0){
                    animator.SetBool("isAttacking", false);
                if(Input.GetKey(KeyCode.F)){
                    Debug.Log("Attack pressed");
                    animator.SetBool("isAttacking", true);
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                    for (int i = 0; i < enemiesToDamage.Length; i++) {
                        enemiesToDamage[i].GetComponentInParent<enemyPatrol>().TakeDamage(damage);
                        Debug.Log("Enemy Hit");
                        
                    }
                    timeBtwAttack = startTimeBtwAttack;
                }


            }else{
                timeBtwAttack -= Time.deltaTime;
            }
        }

    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void learnAttack(){
        canAttack = true;
    }
}
