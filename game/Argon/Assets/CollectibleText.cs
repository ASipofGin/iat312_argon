using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectibleText : MonoBehaviour
{
    public Text collectedText;
    [SerializeField] GameObject tracker;

    private CollectibleTracker ct;

    private void Awake()
    {
        if (ct == null)
        {
            tracker = GameObject.FindGameObjectWithTag("CollectibleTracker");
        }
    }

    public void Update()
    {
        ct = tracker.GetComponent<CollectibleTracker>();
        collectedText.text = ct.collected.ToString() + "/5 Collected";
    }
    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}