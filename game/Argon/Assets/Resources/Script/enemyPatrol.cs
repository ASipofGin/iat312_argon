// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class enemyPatrol : MonoBehaviour
// {
//     public int health;
//     public GameObject pointA;
//     public GameObject pointB;
//     private Rigidbody2D rb;
//     private Animator anim;
//     private Transform currentPoint;
//     public float setSpeed;
//     private float speed;
//     private float dazedTime;
//     public float startDazedTime;
//     public SpriteRenderer sprite;
//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//         sprite = GetComponent<SpriteRenderer>();
//         currentPoint = pointB.transform;
//         anim.SetBool("isRunning", true);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(health <= 0){
//             StartCoroutine(killSelf());
//         }

//         if(dazedTime <= 0){
//             speed = setSpeed;
//         }else{
//             speed = 0;
//             dazedTime -= Time.deltaTime;
//         }

//         Vector2 point = currentPoint.position - transform.position;
//         if (currentPoint == pointB.transform)
//         {
//             rb.velocity = new Vector2(speed, 0);
//         }
//         else
//         {
//             rb.velocity = new Vector2(-speed, 0);
//         }

//         if(Vector2.Distance(transform.position, currentPoint.position) <0.5f && currentPoint == pointB.transform)
//         {
//             flip();
//             currentPoint = pointA.transform;
//         }
//         if(Vector2.Distance(transform.position, currentPoint.position) <0.5f && currentPoint == pointA.transform)
//         {
//             flip();
//             currentPoint = pointB.transform;
//         }
//     }

//     private void flip()
//     {
//         Vector3 localScale = transform.localScale;
//         localScale.x *= -1;
//         transform.localScale = localScale;
//     }
    
//     private void OnDrawGizmos() {
//         Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
//         Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
//         Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
//     }

//     public IEnumerator FlashRed(){
//         sprite.color = Color.red;
//         yield return new WaitForSeconds(0.1f);
//         sprite.color = Color.white;
//     }

//     public IEnumerator killSelf(){
//         sprite.color = Color.red;
//         yield return new WaitForSeconds(0.1f);
//         Destroy(gameObject);
//     }

//     public void TakeDamage(int damage){
//         dazedTime = startDazedTime;
//         StartCoroutine(FlashRed());
//         health -= damage;
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    public int health;
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float setSpeed;
    private float speed;
    private float dazedTime;
    public float startDazedTime;
    public SpriteRenderer sprite;
    public bool isPatrol = true; // Added bool for patrol check
    public Vector2 walkDirection = new Vector2(1, 0); // Direction to walk when not patrolling

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
        if (walkDirection.x < 0){
            flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            StartCoroutine(killSelf());
        }

        if(dazedTime <= 0){
            speed = setSpeed;
        }else{
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (isPatrol)
        {
            Patrol();
        }
        else
        {
            WalkIndefinitely();
        }
    }

    private void Patrol()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
    }

    private void WalkIndefinitely()
    {
        rb.velocity = new Vector2(walkDirection.x * speed, walkDirection.y * speed);
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    public IEnumerator FlashRed(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public IEnumerator killSelf(){
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage){
        dazedTime = startDazedTime;
        StartCoroutine(FlashRed());
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Despawn"))
        {
            Destroy(transform.parent.gameObject); // Destroy the enemy object
        }
    }
}
