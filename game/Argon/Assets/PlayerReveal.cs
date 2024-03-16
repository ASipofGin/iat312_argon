using UnityEngine;

public class PlayerReveal : MonoBehaviour
{
    public KeyCode revealKey = KeyCode.R; // Key to toggle the reveal ability
    private bool isRevealActive = false; // Tracks the state of the reveal ability

    public bool learnedReveal = false; // Initially set to false.
    private ElementalSightTestController estc;

    public float cooldownDuration = 5.0f; // Duration of cooldown in seconds
    private float cooldownTimer = 0.0f; // Timer to track cooldown

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide all revealable objects
        ToggleReveal(false);
        estc = GetComponentInChildren<ElementalSightTestController>();
    }

    // Update is called once per frame
    void Update()
    {
        // If on cooldown, decrement the cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Check for the reveal key press and ensure ability is not on cooldown
        if (Input.GetKeyDown(revealKey) && learnedReveal && cooldownTimer <= 0)
        {
            isRevealActive = !isRevealActive; // Toggle the state
            ToggleReveal(isRevealActive);
            if (isRevealActive){
                estc.SightActive();
            }else{
                estc.SightDisable();
            }

            // Reset the cooldown timer
            cooldownTimer = cooldownDuration;
        }
    }

    void ToggleReveal(bool reveal)
    {
        GameObject[] revealables = GameObject.FindGameObjectsWithTag("Revealable"); // Find all objects with the "Revealable" tag
        foreach (GameObject obj in revealables)
        {
            // Toggle visibility
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = reveal;
            }

            // Optionally, disable collider if you don't want the player to interact with it when not visible
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = reveal;
            }

            // Freeze or unfreeze Rigidbody2D if it exists
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (reveal)
                {
                    // Unfreeze when revealed
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Presuming you still want to freeze rotation
                }
                else
                {
                    // Freeze when hidden
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }

    public void learnReveal(){
        learnedReveal = true;
    }
}
