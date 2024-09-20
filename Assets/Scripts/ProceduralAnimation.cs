using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    private float verticalVelocity = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 relativeMovement = (forward * moveZ + right * moveX).normalized * moveSpeed;

        bool isMoving = relativeMovement.magnitude > 0;
        animator.SetBool("isWalking", isMoving);

        if (controller.isGrounded)
        {
            verticalVelocity = 0;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        relativeMovement.y = verticalVelocity;

        controller.Move(relativeMovement * Time.deltaTime);

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(relativeMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }
}
