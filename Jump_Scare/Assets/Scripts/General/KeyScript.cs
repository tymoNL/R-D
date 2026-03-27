using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private LightManager lightManager;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioSource pickupSound;

    [SerializeField] private GameObject monster;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupSound.Play();
            playerMovement.canEscape = true;

            lightManager.ToggleLights();

            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            monster.SetActive(true);
        }
    }
}