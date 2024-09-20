using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yRotation = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        HandleMouseLook();

        Vector3 direction = new Vector3(0, 0, -offset.magnitude);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 newPosition = target.position + rotation * direction;

        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
        transform.LookAt(target);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;
    }
}
