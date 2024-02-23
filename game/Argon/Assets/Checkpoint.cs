using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameController gameController;
    public Transform respawnPoint;
    // Start is called before the first frame update
    private void Awake() {
        gameController= GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
    }


    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(respawnPoint.position);
        }
    }
}
