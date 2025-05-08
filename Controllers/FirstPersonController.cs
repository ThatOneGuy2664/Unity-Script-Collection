using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float crouchMultiplier = 0.5f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 3f;

    [Header("Zoom Settings")]
    public bool allowZoom = false;
    public float zoomDistance = 1f;
    public float minZoom = 0f;
    public float maxZoom = 3f;

    [Header("Headbob Settings")]
    public bool enableHeadbob = true;
    public float headbobSpeed = 10f;
    public float headbobAmount = 0.05f;

    private Rigidbody rb;
    private Vector3 movementInput;
    private float yaw;
    private float pitch;
    private Vector3 cameraDefaultLocalPos;
    private float bobTimer;
    private float bobFade = 0f;
    private float zoomAmount = 0f;
    private bool isCrouching;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 3; // To stop from sliding

        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                cameraTransform = mainCam.transform;
        }

        if (cameraTransform != null)
        {
            // Parent the camera to this object
            cameraTransform.SetParent(transform);
            cameraTransform.localPosition = new Vector3(0, 0.8f, 0);
            cameraDefaultLocalPos = cameraTransform.localPosition;
        }

        yaw = transform.eulerAngles.y;
        pitch = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Mouse Look
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Input
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Zoom
        if (allowZoom && Input.GetKeyDown(KeyCode.Z))
        {
            zoomAmount += zoomDistance;
            if (zoomAmount > maxZoom)
                zoomAmount = minZoom;
        }

        // Headbob
        float moveAmount = movementInput.magnitude;
        bool isMoving = moveAmount > 0.1f;
        Vector3 camTarget = cameraDefaultLocalPos + Vector3.back * zoomAmount;

        // Smoothly fade in/out headbob when moving/stopping
        bobFade = Mathf.Lerp(bobFade, isMoving ? 1f : 0f, Time.deltaTime * 6f);

        if (enableHeadbob)
        {
            if (isMoving)
                bobTimer += Time.deltaTime * headbobSpeed;

            float bobOffset = Mathf.Sin(bobTimer) * headbobAmount * bobFade;
            camTarget.y += bobOffset;
        }

        cameraTransform.localPosition = camTarget;
    }

    private void FixedUpdate()
    {
        float speed = walkSpeed;

        if (isCrouching)
            speed *= crouchMultiplier;
        else if (Input.GetKey(KeyCode.LeftShift))
            speed *= sprintMultiplier;

        Vector3 direction = transform.forward * movementInput.z + transform.right * movementInput.x;
        Vector3 velocity = direction.normalized * speed;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
