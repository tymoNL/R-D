using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] keyLights;
    [SerializeField] private GameObject[] exitLights;

    void Start()
    {
        SetLights(keyLights, true);
        SetLights(exitLights, false);
    }

    public void ToggleLights()
    {
        SetLights(keyLights, false);
        SetLights(exitLights, true);
    }

    private void SetLights(GameObject[] objects, bool state)
    {
        foreach (GameObject obj in objects)
        {
            Light light = obj.GetComponentInChildren<Light>();
            if (light != null)
            {
                light.enabled = state;
            }

            ParticleSystem fire = obj.GetComponentInChildren<ParticleSystem>();
            if (fire != null)
            {
                if (state)
                    fire.Play();
                else
                    fire.Stop();
            }
        }
    }
}
