using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float mouseSensitivity = 100f;
    public float distanceFromTarget = 5f;

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

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distanceFromTarget + new Vector3(0, -offset.y, 0));
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
        transform.LookAt(target.position + new Vector3(0, offset.y, 0));
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
