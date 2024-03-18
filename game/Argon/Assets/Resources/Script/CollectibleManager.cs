using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] GameObject tracker;

    [SerializeField] GameObject text;

    public bool isPickedUp;

    public float amp;
    public float freq;

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
            text = GameObject.FindGameObjectWithTag("CollectibleText");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (isPickedUp)
        {
            StartCoroutine(TextTransition());
            ct = tracker.GetComponent<CollectibleTracker>();
            ct.addCollected();
            Destroy(gameObject);
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
        collectedText = text.GetComponent<CollectibleText>();
        collectedText.StartCoroutine(collectedText.FadeTextToFullAlpha(1f, collectedText.GetComponent<Text>()));

        yield return new WaitForSecondsRealtime(3);

        collectedText.StartCoroutine(collectedText.FadeTextToZeroAlpha(1f, collectedText.GetComponent<Text>()));
    }


}
