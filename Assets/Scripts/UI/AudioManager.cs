using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // ---------------- SAVE KEYS ----------------

    private const string MENU_VOLUME_KEY = "MenuVolume";
    private const string GAME_VOLUME_KEY = "GameVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    // ---------------- AUDIO SOURCES ----------------

    [Header("Menu Music")]
    public AudioSource menuMusicSource;
    public AudioClip[] menuMusicClips;

    [Header("Game Music")]
    public AudioSource gameMusicSource;
    public AudioClip[] gameMusicClips;

    [Header("SFX")]
    public AudioSource[] sfxSources;
    public Sound[] sfxSounds;

    // ---------------- UI ----------------

    [Header("Volume Sliders")]
    public Slider menuMusicSlider;
    public Slider gameMusicSlider;
    public Slider sfxSlider;

    // ---------------- VOLUMES ----------------

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float menuMusicVolume = 1f;

    [Range(0f, 1f)]
    public float gameMusicVolume = 1f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    // ---------------- INTERNAL ----------------

    private Dictionary<string, Sound> sfxDictionary =
        new Dictionary<string, Sound>();

    private int sfxSourceIndex = 0;

    // ---------------- AWAKE ----------------

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolumes();

        foreach (Sound sound in sfxSounds)
        {
            if (!sfxDictionary.ContainsKey(sound.name))
                sfxDictionary.Add(sound.name, sound);
        }
    }

    // ---------------- START ----------------

    void Start()
    {
        SetupSliders();
        ApplyVolumes();
    }

    // ---------------- UPDATE ----------------

    void Update()
    {
        ApplyVolumes();
    }

    // ---------------- SLIDER SETUP ----------------

    void SetupSliders()
    {
        if (menuMusicSlider != null)
        {
            menuMusicSlider.value = menuMusicVolume;
            menuMusicSlider.onValueChanged.AddListener(SetMenuMusicVolume);
        }

        if (gameMusicSlider != null)
        {
            gameMusicSlider.value = gameMusicVolume;
            gameMusicSlider.onValueChanged.AddListener(SetGameMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // ---------------- APPLY VOLUMES ----------------

    void ApplyVolumes()
    {
        if (menuMusicSource != null)
            menuMusicSource.volume = menuMusicVolume;

        if (gameMusicSource != null)
            gameMusicSource.volume = gameMusicVolume;

        if (sfxSources != null)
        {
            foreach (AudioSource source in sfxSources)
            {
                if (source != null)
                    source.volume = sfxVolume;
            }
        }
    }

    // ---------------- MENU MUSIC ----------------

    public void PlayMenuMusic(int index)
    {
        if (menuMusicSource == null)
            return;

        if (index < 0 || index >= menuMusicClips.Length)
            return;

        menuMusicSource.Stop();

        menuMusicSource.clip = menuMusicClips[index];
        menuMusicSource.loop = true;
        menuMusicSource.Play();
    }

    public void StopMenuMusic()
    {
        if (menuMusicSource != null)
            menuMusicSource.Stop();
    }

    // ---------------- GAME MUSIC ----------------

    public void PlayGameMusic(int index)
    {
        if (gameMusicSource == null)
            return;

        if (index < 0 || index >= gameMusicClips.Length)
            return;

        gameMusicSource.Stop();

        gameMusicSource.clip = gameMusicClips[index];
        gameMusicSource.loop = true;
        gameMusicSource.Play();
    }

    public void StopGameMusic()
    {
        if (gameMusicSource != null)
            gameMusicSource.Stop();
    }

    // ---------------- SFX ----------------

    public void PlaySFX(string soundName)
    {
        if (!sfxDictionary.TryGetValue(soundName, out Sound sound))
        {
            Debug.LogWarning("SFX not found: " + soundName);
            return;
        }

        if (sfxSources == null || sfxSources.Length == 0)
            return;

        AudioSource source = sfxSources[sfxSourceIndex];

        if (source != null)
        {
            source.pitch = sound.pitch;

            source.PlayOneShot(
                sound.clip,
                sound.volume * sfxVolume
            );
        }

        sfxSourceIndex++;

        if (sfxSourceIndex >= sfxSources.Length)
            sfxSourceIndex = 0;
    }

    // ---------------- SAVE / LOAD ----------------

    void LoadVolumes()
    {
        menuMusicVolume =
            PlayerPrefs.GetFloat(MENU_VOLUME_KEY, 1f);

        gameMusicVolume =
            PlayerPrefs.GetFloat(GAME_VOLUME_KEY, 1f);

        sfxVolume =
            PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }

    public void SaveVolumes()
    {
        PlayerPrefs.SetFloat(
            MENU_VOLUME_KEY,
            menuMusicVolume
        );

        PlayerPrefs.SetFloat(
            GAME_VOLUME_KEY,
            gameMusicVolume
        );

        PlayerPrefs.SetFloat(
            SFX_VOLUME_KEY,
            sfxVolume
        );

        PlayerPrefs.Save();
    }

    // ---------------- SLIDER FUNCTIONS ----------------

    public void SetMenuMusicVolume(float value)
    {
        menuMusicVolume = value;
        SaveVolumes();
    }

    public void SetGameMusicVolume(float value)
    {
        gameMusicVolume = value;
        SaveVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        SaveVolumes();
    }
}