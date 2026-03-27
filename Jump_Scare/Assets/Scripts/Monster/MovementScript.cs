using UnityEngine;
using UnityEngine.AI;

public class MovementScript : MonoBehaviour
{
    public Transform player;
    public Camera playerCamera;
    private NavMeshAgent agent;

    [SerializeField] private float lookThreshold = 0.6f;

    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioClip[] walkingClips;
    [SerializeField] private float stepInterval = 0.5f;

    private TorchLight torchLight;

    private float stepTimer;

    void Start() {
        agent = GetComponent<NavMeshAgent>();

        torchLight = FindObjectOfType<TorchLight>();
    }

    void Update() {

        // Richting van camera naar de vijand berekenen
        Vector3 dirToEnemy = (transform.position - playerCamera.transform.position).normalized;

        // Om te checken of speler richting vijand kijkt
        float dot = Vector3.Dot(playerCamera.transform.forward, dirToEnemy);

        bool isLookingAtEnemy = false;

        // Check of de kijkrichting binnen de lookThreshold valt
        if (dot > lookThreshold) {
            Ray ray = new Ray(playerCamera.transform.position, dirToEnemy);
            RaycastHit hit;

            // Raycast om te checken of er niets tussen zit
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform == transform)
                {
                    isLookingAtEnemy = true;
                }
            }
        }

        if (isLookingAtEnemy && torchLight.IsTorchOn) { agent.isStopped = true; }
        else {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        HandleFootsteps();
    }

    void HandleFootsteps() {
        // Check of er beweging is
        bool isMoving = agent.velocity.magnitude > 0.1f && !agent.isStopped;

        if (isMoving) {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f) {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else { stepTimer = 0f; }
    }

    void PlayFootstep() {
        if (walkingClips.Length == 0) 
            return;

        int index = Random.Range(0, walkingClips.Length);
        walkingSource.PlayOneShot(walkingClips[index]);
    }
}