using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioClip[] walkingClips;
    [SerializeField] private float stepInterval = 0.5f;


    private float stepTimer;

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

        bool isMoving = moveInput.magnitude > 0.1f && controller.isGrounded;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Door"))
        {
            Debug.Log("Player hit the door");
            UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryScene");
        }
    }

    void PlayFootstep()
    {
        if (walkingClips.Length == 0) return;

        int index = Random.Range(0, walkingClips.Length); // Pak een random audioclip uit de array
        walkingSource.PlayOneShot(walkingClips[index]); // Speel deze af
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