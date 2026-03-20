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
    [SerializeField] private float drainRate = 1f;

    private void Update()
    {
        if (torchLight.enabled && batteryPercentage > 0)
        {
            batteryPercentage -= drainRate * Time.deltaTime;
            batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);

            if (batteryPercentage <= 0)
            {
                torchLight.enabled = false;
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
        }
    }
}
