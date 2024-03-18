using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleSceneLoader : MonoBehaviour
{
    [SerializeField] GameObject tracker;

    private CollectibleTracker ct;

    public string targetScene1; // Name of the scene to load
    public string targetScene2;
    private void Awake()
    {
        if (ct == null)
        {
            tracker = GameObject.FindGameObjectWithTag("CollectibleTracker");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Check if the colliding object is the player
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        ct = tracker.GetComponent<CollectibleTracker>();
        if(ct.collected == 5)
        {
            SceneManager.LoadScene(targetScene1);
        }
        else
        {
            SceneManager.LoadScene(targetScene2);
        }
    }
}
