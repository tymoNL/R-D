using UnityEngine;

public class TriggerJumpScare : MonoBehaviour
{
    [SerializeField] private bool hasTriggeredJumpScare = false;
    [SerializeField] private GameObject jumpScareUI;

    private void OnTriggerEnter(Collider other)
    {
        // Check if jumpscare already triggered
        if (!hasTriggeredJumpScare && other.CompareTag("Player"))
        {
            hasTriggeredJumpScare = true;

            Debug.Log("Speler is de zone binnengekomen!");

            // Enable the UI
            if (jumpScareUI != null)
            {
                jumpScareUI.SetActive(true);
            }
        }
    }
}