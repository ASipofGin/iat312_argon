using UnityEngine;

public class PlayerReveal : MonoBehaviour
{
    public KeyCode revealKey = KeyCode.R;
    private bool isRevealActive = false;

    public bool learnedReveal = false;
    private ElementalSightTestController estc;

    public float cooldownDuration = 5.0f;
    private float cooldownTimer = 0.0f;

    void Start()
    {
        ToggleReveal(false);
        estc = GetComponentInChildren<ElementalSightTestController>();
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(revealKey) && learnedReveal && cooldownTimer <= 0)
        {
            isRevealActive = !isRevealActive;
            ToggleReveal(isRevealActive);
            if (isRevealActive)
            {
                estc.SightActive();
            }
            else
            {
                estc.SightDisable();
            }

            cooldownTimer = cooldownDuration;
        }
    }

    void ToggleReveal(bool reveal)
    {
        GameObject[] revealables = GameObject.FindGameObjectsWithTag("Revealable");
        foreach (GameObject obj in revealables)
        {
            ToggleObjectAndChildren(obj, reveal);
        }
    }

    void ToggleObjectAndChildren(GameObject obj, bool reveal)
    {
        // Toggle visibility for the object
        ToggleComponents(obj, reveal);

        // Recursively toggle visibility for children
        foreach (Transform child in obj.transform)
        {
                ToggleObjectAndChildren(child.gameObject, reveal);
        }
    }

    void ToggleComponents(GameObject obj, bool reveal)
    {
        // Toggle visibility
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = reveal;
        }

        // Optionally, disable collider
        Collider2D collider = obj.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = reveal;
        }

        // Freeze or unfreeze Rigidbody2D
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (reveal)
            {
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    public void learnReveal()
    {
        learnedReveal = true;
    }

    public void DeactivateReveal()
    {
        learnedReveal = false;
        estc.SightDisable();
    }
}
