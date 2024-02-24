using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 checkpointPos;
    public Rigidbody2D rb;
    Vector3 currScale;
    public PlayerMovement pm;
    // Start is called before the first frame update
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        currScale = transform.localScale;

    }
    
    void Start()
    {
        checkpointPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die(){
        StartCoroutine(Respawn(0.5f));
    }

    public void UpdateCheckpoint(Vector2 pos){
        checkpointPos = pos;
    }

    IEnumerator Respawn(float duration){

        rb.simulated = false;
        transform.localScale = new Vector3(0,0,0);
        pm.ResetAnimation();
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        transform.localScale = currScale;
        pm.ResetFace();
        pm.RestControl();
        rb.simulated = true;
    }
}
