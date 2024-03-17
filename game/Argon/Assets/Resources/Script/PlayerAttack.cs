using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Existing variables
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;

    public bool canAttack = false;

    Animator animator;

    // Audio variables
    public AudioClip swingClip;
    public AudioClip swingHitClip;
    private AudioSource audioSource;

    private PlayerMovement pm;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Ensure there's an AudioSource component attached
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (canAttack == true && pm.controllable())
        {
            if (timeBtwAttack <= 0)
            {
                animator.SetBool("isAttacking", false);
                if (Input.GetKey(KeyCode.F))
                {
                    animator.SetBool("isAttacking", true);
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                    if (enemiesToDamage.Length > 0)
                    {
                        // Hit enemy, play "swing hit" audio
                        audioSource.PlayOneShot(swingHitClip);
                        for (int i = 0; i < enemiesToDamage.Length; i++)
                        {
                            enemiesToDamage[i].GetComponentInParent<enemyPatrol>().TakeDamage(damage);
                        }
                    }
                    else
                    {
                        // Swung at the air, play "swing" audio
                        audioSource.PlayOneShot(swingClip);
                    }
                    timeBtwAttack = startTimeBtwAttack;
                }
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void learnAttack()
    {
        canAttack = true;
    }

    public void DeactivateAttack()
    {
        canAttack = false;
        if (animator != null){
            animator.SetBool("isAttacking", false);
        }

    }
}
