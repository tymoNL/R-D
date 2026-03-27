using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealthScript : MonoBehaviour
{
    public float heartRate = 60f;
    private bool canCalmDown = false;

    [SerializeField] private AudioSource heartbeatAudio;

    [SerializeField] private Volume volume;
    private Vignette vignette;

    void Start()
    {
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
        }
    }

    void Update() {
        if (canCalmDown)
        {
            heartRate = Mathf.Lerp(heartRate, 60f, Time.deltaTime * 0.5f);
        }

        CheckHeartRate();
        UpdateHeartbeatSound();

        UpdateVignette();
    }

    public void IncreaseHeartRate(float amount) {
        heartRate += amount;
    }

    void CheckHeartRate() {
        if (heartRate >= 200) {
            SceneManager.LoadScene("DefeatScene");
        }
    }

    void UpdateHeartbeatSound() {
        if (heartRate > 80) {
            if (!heartbeatAudio.isPlaying)
                heartbeatAudio.Play();

            heartbeatAudio.pitch = Mathf.Lerp(1f, 2f, heartRate / 200f);
            heartbeatAudio.volume = Mathf.Lerp(0.5f, 2f, heartRate / 200f);
        }
        else { heartbeatAudio.Stop(); }
    }

    void UpdateVignette() {
        if (vignette == null) return;

        float normalized = heartRate / 200f;

        vignette.intensity.value = Mathf.Lerp(0.4f, 0.6f, normalized);
    }

    public void SetCalming(bool value)
    {
        canCalmDown = value;
    }
}