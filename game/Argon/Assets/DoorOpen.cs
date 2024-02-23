using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public float speed = 5f;

    
    public float distanceToMove = 3f;

    private Vector3 startPosition;
    private Vector3 endPositionUp; // Target position when moving up
    private Vector3 endPositionDown;

    public float smoothTime;

    public float moveUpDistance = 3f;
    

    public GameObject keyObject; // Assign this in the Inspector
    [SerializeField]private bool hasMoved = false;

    private Vector2 vel;

    private bool moveUp = false;
    private bool moveDown = false;

    private Vector3 velocity = Vector3.zero;

    

    Door DoorChild;

    void Start()
    {

        startPosition = transform.position;

        endPositionUp = startPosition + new Vector3(0, moveUpDistance, 0); // Set end position for moving up
        endPositionDown = startPosition; // Set end position for moving down
    }

    // Update is called once per frame
    void Update()
    {
        Door DoorChild = GetComponentInChildren<Door>();
        if (DoorChild != null && (DoorChild.locked == false) && !hasMoved)
        {
            MoveObjectUp();
            if (keyObject != null)
                {
                    Destroy(keyObject); // Destroy the key object
                }
        }


        if (moveUp)
        {
            MoveObjectUpSmooth();
        }
        else if (moveDown)
        {
            MoveObjectDownSmooth();
        }
    }

    void MoveObjectUp()
    {
        float distanceMoved = Vector3.Distance(startPosition, transform.position);
        if (distanceMoved < distanceToMove)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            hasMoved = true; // Stop moving after reaching the set distance
        }
    }

    public void MoveObjectUpSmooth()
    {
        transform.position = Vector3.SmoothDamp(transform.position, endPositionUp, ref velocity, smoothTime);

        // Check if the door is close enough to the target position
        if (Vector3.Distance(transform.position, endPositionUp) < 0.01f)
        {
            moveUp = false; // Stop moving up
        }
    }

    public void MoveObjectDownSmooth()
    {
        // Move towards the end position down
        transform.position = Vector3.SmoothDamp(transform.position, endPositionDown, ref velocity, smoothTime);

        // Check if the door is close enough to the target position
        if (Vector3.Distance(transform.position, endPositionDown) < 0.01f)
        {
            moveDown = false; // Stop moving down
        }
    }

        public void StartMovingUp()
    {
        moveUp = true;
        moveDown = false;
    }

    public void StartMovingDown()
    {
        moveDown = true;
        moveUp = false;
    }
}