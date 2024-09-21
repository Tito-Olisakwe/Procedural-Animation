using UnityEngine;

public class ProceduralControl : MonoBehaviour
{
    public float detectionDistance = 1.0f;

    // Joints to control
    public Transform leftHand;
    public Transform rightHand;
    public Transform leftLeg;
    public Transform rightLeg;
    public Transform head;
    public Transform hips;
    public Transform spine1;
    public Transform spine2;

    private SimpleMovement movementScript;
    private Animator animator;
    private bool isReacting = false;
    private Vector3 lastObstaclePosition;
    private bool isWalkingAway = false;
    private bool isRotating = false;

    private Quaternion targetRotation;
    private Vector3 walkAwayDirection;

    void Start()
    {
        movementScript = GetComponent<SimpleMovement>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (movementScript == null)
        {
            return;
        }

        if (isReacting || isRotating || isWalkingAway)
        {
            ProcedurallyWalkAway();
            return;
        }

        // Detect obstacles
        if (IsMoving() && DetectObstacle())
        {
            if (Vector3.Distance(transform.position, lastObstaclePosition) < 0.5f)
            {
                return;
            }

            animator.enabled = false;
            BendAndTouchObstacle();

            lastObstaclePosition = transform.position;

            movementScript.enabled = false;
            isReacting = true;

            Invoke("EndReaction", 0.1f);

            return;
        }

        // Ensuring the movement script is enabled if we are not reacting or walking away
        if (!movementScript.enabled && !isReacting && !isWalkingAway && !isRotating)
        {
            movementScript.enabled = true;
        }
    }

    // Method to detect obstacles in front of the character
    bool DetectObstacle()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, detectionDistance))
        {
            return true;
        }
        return false;
    }

    // Character bending
    void BendAndTouchObstacle()
    {
        hips.localRotation = Quaternion.Euler(20, 0, 0);
        spine1.localRotation = Quaternion.Euler(20, 0, 0);
        spine2.localRotation = Quaternion.Euler(15, 0, 0);
        leftHand.localRotation = Quaternion.Euler(0, 0, 45);
        rightHand.localRotation = Quaternion.Euler(0, 0, 45);
        head.localRotation = Quaternion.Euler(30, 0, 0);
    }

    // End reaction and start rotating and walking away procedurally
    void EndReaction()
    {
        hips.localRotation = Quaternion.identity;
        spine1.localRotation = Quaternion.identity;
        spine2.localRotation = Quaternion.identity;
        leftHand.localRotation = Quaternion.identity;
        rightHand.localRotation = Quaternion.identity;
        head.localRotation = Quaternion.identity;

        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        isRotating = true;

        InvokeRepeating("RotateCharacter", 0f, 0.02f);
    }

    // Rotate the character to the opposite direction
    void RotateCharacter()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8.0f);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isRotating = false;
            CancelInvoke("RotateCharacter");
            StartWalkingAway();
        }
    }

    // Start walking away procedurally
    void StartWalkingAway()
    {
        isWalkingAway = true;
        Invoke("StopWalkingAway", 0.5f);

        walkAwayDirection = transform.forward;

        ProcedurallyWalkAway();
    }

    // Procedurally walk away
    void ProcedurallyWalkAway()
    {
        transform.position += walkAwayDirection * Time.deltaTime * 2.0f;

        leftLeg.localRotation = Quaternion.Euler(30, 0, 0);
        rightLeg.localRotation = Quaternion.Euler(-30, 0, 0);
    }

    // Stop walking after walking away for a set time
    void StopWalkingAway()
    {
        isWalkingAway = false;
        movementScript.enabled = true;
        animator.enabled = true;
        isReacting = false;
        isRotating = false;
        isWalkingAway = false;
    }

    // Check if the character is moving by referencing SimpleMovement's movement logic
    bool IsMoving()
    {
        if (movementScript != null && movementScript.enabled && animator.GetBool("isWalking"))
        {
            return true;
        }
        return false;
    }
}
