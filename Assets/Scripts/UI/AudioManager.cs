// Assets/Scripts/Managers/AudioManager.cs
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
    public bool playOnAwake = false;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] sounds;
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // Create audio sources for each sound
        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.playOnAwake = sound.playOnAwake;
            audioSources.Add(sound.name, source);
        }
    }

    public void Play(string name)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found!");
        }
    }

    public void Stop(string name)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.Stop();
        }
    }

    public void PlayOneShot(string name)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.PlayOneShot(source.clip);
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (audioSources.TryGetValue(name, out AudioSource source))
        {
            source.volume = volume;
        }
    }
}