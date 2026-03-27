using UnityEngine;

public class JumpScareScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject jumpScareObject;

    [SerializeField] private float triggerDistance = 2f;
    [SerializeField] private float lookThreshold = 0.6f;

    private bool triggered = false;

    private Vector3 startLocation;

    private PlayerHealthScript playerHealth;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealthScript>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        startLocation = transform.position;
    }

    void Update() {
        if (triggered) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= triggerDistance && !IsPlayerLooking()) {
            TriggerJumpscare();
        }
    }

    bool IsPlayerLooking() {
        Vector3 dirToEnemy = (transform.position - playerCamera.transform.position).normalized;
        float dot = Vector3.Dot(playerCamera.transform.forward, dirToEnemy);

        if (dot > lookThreshold) {
            Ray ray = new Ray(playerCamera.transform.position, dirToEnemy);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f)) {
                if (hit.transform == transform) { return true; }
            }
        }

        return false;
    }

    private void TriggerJumpscare() {
        triggered = true;
        jumpScareObject.SetActive(true);

        playerHealth.IncreaseHeartRate(50f);

        transform.position = startLocation;
        playerMovement.enabled = false;

        StartCoroutine(ResetMonster());
    }

    private System.Collections.IEnumerator ResetMonster()
    {
        yield return new WaitForSeconds(3f);

        jumpScareObject.SetActive(false);
        triggered = false;

        playerMovement.enabled = true;
    }
}