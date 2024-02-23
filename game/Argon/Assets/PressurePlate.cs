using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject door; // Assign your door GameObject here in the inspector
    
    private DoorOpen doorOpen;

    private bool isActive = false;

    private void Start() {
        if (door != null)
        {
            doorOpen = door.GetComponent<DoorOpen>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") && !isActive) // Make sure your box has the tag "Box"
        {
            OpenDoor();
            isActive = true;

            Debug.Log("opening!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") && isActive)
        {
            isActive = false;
            CloseDoor(); // Optional: Implement this if you want the door to close when the box is removed

            Debug.Log("closing!");
        }
    }

    void OpenDoor()
    {
        doorOpen.StartMovingUp(); // Call to start moving the door up
    }

    void CloseDoor()
    {
        doorOpen.StartMovingDown(); // Call to start moving the door down
    }
}