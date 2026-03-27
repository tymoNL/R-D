using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TorchLight : MonoBehaviour
{

    [SerializeField] private InputActionReference toggleAction;
    [SerializeField] private Light torchLight;
    [SerializeField] private AudioSource torchSound;


    [SerializeField] private Slider batterySlider;
    [SerializeField] private float batteryPercentage = 100;
    [SerializeField] private float drainRate = 5f;
    [SerializeField] private float rechargeRate = 1f;

    private PlayerHealthScript playerHealth;

    public bool IsTorchOn => torchLight.enabled;


    void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealthScript>();
    }

    private void Update()
    {
        if (torchLight.enabled && batteryPercentage > 0)
        {
            // Drain battery
            batteryPercentage -= drainRate * Time.deltaTime;
            batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);

            if (batteryPercentage <= 0)
            {
                torchLight.enabled = false;

                if (!torchSound.isPlaying)
                    torchSound.Play();
            }
        }
        else
        {
            // Recharge battery
            if (batteryPercentage < 100)
            {
                batteryPercentage += rechargeRate * Time.deltaTime;
                batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);
            }
        }

        batterySlider.value = batteryPercentage;
    }

    private void OnEnable()
    {
        toggleAction.action.performed += OnToggle;
    }

    private void OnDisable()
    {
        toggleAction.action.performed -= OnToggle;
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
        if (batteryPercentage <= 0) return;

        if (!torchSound.isPlaying)
        {
            torchLight.enabled = !torchLight.enabled;
            torchSound.Play();

            playerHealth.SetCalming(torchLight.enabled);
        }
    }
}
