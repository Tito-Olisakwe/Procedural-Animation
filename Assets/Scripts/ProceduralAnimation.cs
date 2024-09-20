using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);

        moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            verticalVelocity = 0;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
