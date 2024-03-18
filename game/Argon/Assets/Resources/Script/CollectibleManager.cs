using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] GameObject tracker;

    [SerializeField] GameObject ctext;

    public bool isPickedUp;

    public float amp;
    public float freq;

    private bool transitionStarted = false;
    private CollectibleTracker ct;
    private CollectibleText collectedText;
    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (ct == null)
        {
            tracker = GameObject.FindGameObjectWithTag("CollectibleTracker");
        }

        if (collectedText == null)
        {
            ctext = GameObject.FindGameObjectWithTag("CollectibleText");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (isPickedUp && !transitionStarted)
        {
            ct = tracker.GetComponent<CollectibleTracker>();
            collectedText = ctext.GetComponent<CollectibleText>();

            ct.addCollected();
            transitionStarted = false;
            StartCoroutine(TextTransition()); 
        }
        else{
            transform.position = new Vector3(initPos.x,Mathf.Sin(Time.time + freq) * amp + initPos.y, 0);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
        }
    }

    private IEnumerator TextTransition()
    {
            transitionStarted = true;
            Debug.Log("running");
            collectedText = ctext.GetComponent<CollectibleText>();
            collectedText.StartCoroutine(collectedText.FadeTextToFullAlpha(1f, collectedText.GetComponent<Text>()));

            yield return new WaitForSeconds(1f);
            Debug.Log("Waited");
            collectedText.StartCoroutine(collectedText.FadeTextToZeroAlpha(1f, collectedText.GetComponent<Text>()));
            
            Destroy(gameObject);

    }


}
