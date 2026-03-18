using UnityEngine;

public class BreathingScript : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private AudioSource breathingSource;
    [SerializeField] private AudioClip[] breathingClips;

    [SerializeField] private AudioSource whisperSource;
    [SerializeField] private AudioClip[] whisperClips;

    [SerializeField] private float whisperMinDelay = 8f;
    [SerializeField] private float whisperMaxDelay = 20f;

    private float whisperTimer;

    void Start()
    {
        PlayRandomBreathing();
        ResetWhisperTimer();
    }

    void Update()
    {
        HandleWhispers();
        HandleBreathing();

        // Breathing volume based on distance
        float distance = Vector3.Distance(transform.position, player.position);
        breathingSource.volume = Mathf.Clamp01(1f - distance / 20f);
    }

    void HandleBreathing()
    {
        // When current breathing clip ends, play another random one
        if (!breathingSource.isPlaying)
        {
            PlayRandomBreathing();
        }
    }

    void PlayRandomBreathing()
    {
        if (breathingClips.Length == 0) return;

        int index = Random.Range(0, breathingClips.Length);

        breathingSource.pitch = Random.Range(0.9f, 1.1f);
        breathingSource.clip = breathingClips[index];
        breathingSource.Play();
    }

    void HandleWhispers()
    {
        whisperTimer -= Time.deltaTime;

        if (whisperTimer <= 0f)
        {
            PlayWhisper();
            ResetWhisperTimer();
        }
    }

    void PlayWhisper()
    {
        if (whisperClips.Length == 0) return;

        int index = Random.Range(0, whisperClips.Length);

        whisperSource.pitch = Random.Range(0.8f, 1.1f);
        whisperSource.PlayOneShot(whisperClips[index]);
    }

    void ResetWhisperTimer()
    {
        whisperTimer = Random.Range(whisperMinDelay, whisperMaxDelay);
    }
}