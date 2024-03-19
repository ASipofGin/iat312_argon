using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleTracker : MonoBehaviour
{

    public static CollectibleTracker instance;
    public float collected = 0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "StartMenu")
        {
            Destroy(gameObject);
            return;
        }

    }

    public void addCollected()
    {
        collected += 1f;
    }
}
