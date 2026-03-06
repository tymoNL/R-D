using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 50f;

    public Transform playerCamera;

    private Vector3 velocity;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Mouse Look
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}