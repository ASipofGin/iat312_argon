using UnityEngine;

public class PlayerSummon : MonoBehaviour
{
    public GameObject prefabToSummon;
    // Set the offset to spawn the prefab in front of the player. Adjust as needed.
    public Vector2 summonOffset = new Vector2(1, 0.5f); // 1 unit in front, 0.5 units above in 2D space.

    private GameObject currentSummonedObject = null;

    public bool learnedSummon = false; // Initially set to false.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && learnedSummon) // Change to KeyCode.C for the 'C' key
        {
            SummonPrefab();
        }
    }

    void SummonPrefab()
    {
        if (currentSummonedObject != null)
        {
            Destroy(currentSummonedObject);
        }

        if (prefabToSummon != null)
        {
            // Determine the direction the player is facing.
            float direction = Mathf.Sign(transform.localScale.x);
            // Adjust the spawn position based on the player's facing direction.
            Vector2 summonPosition = new Vector2(transform.position.x + summonOffset.x * direction, transform.position.y + summonOffset.y);

            // Instantiate the prefab at the calculated 2D position.
            currentSummonedObject = Instantiate(prefabToSummon, summonPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Prefab to summon is not assigned.");
        }
    }

    public void learnSummon(){
        learnedSummon = true;
    }
}
