using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // ---------------- SAVE KEYS ----------------
    private const string MENU_VOLUME_KEY = "MenuVolume";
    private const string GAME_VOLUME_KEY = "GameVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    // ---------------- MUSIC ----------------
    [Header("Menu Music")]
    public AudioSource menuMusicSource;
    public AudioClip[] menuMusicClips;

    [Header("Game Music")]
    public AudioSource gameMusicSource;
    public AudioClip[] gameMusicClips;

    // ---------------- SFX ----------------
    [Header("SFX")]
    public AudioSource[] sfxSources;
    public Sound[] sfxSounds;

    private Dictionary<string, Sound> sfxDictionary = new Dictionary<string, Sound>();
    private int sfxIndex = 0;

    // ---------------- SLIDERS ----------------
    [Header("Sliders")]
    public Slider menuSlider;
    public Slider gameSlider;
    public Slider sfxSlider;

    // ---------------- VOLUMES ----------------
    public float menuVolume = 1f;
    public float gameVolume = 1f;
    public float sfxVolume = 1f;

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
        BuildDictionary();
    }

    void Start()
    {
        BindSliders();
        ApplyVolumes();
    }

    void Update()
    {
        ApplyVolumes();
    }

    // ---------------- DICTIONARY ----------------
    void BuildDictionary()
    {
        sfxDictionary.Clear();

        if (sfxSounds == null) return;

        foreach (var s in sfxSounds)
        {
            if (s != null && !sfxDictionary.ContainsKey(s.name))
                sfxDictionary.Add(s.name, s);
        }
    }

    // ---------------- SLIDERS ----------------
    void BindSliders()
    {
        if (menuSlider != null)
        {
            menuSlider.value = menuVolume;
            menuSlider.onValueChanged.AddListener(SetMenuVolume);
        }

        if (gameSlider != null)
        {
            gameSlider.value = gameVolume;
            gameSlider.onValueChanged.AddListener(SetGameVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    // ---------------- APPLY VOLUME ----------------
    void ApplyVolumes()
    {
        if (menuMusicSource) menuMusicSource.volume = menuVolume;
        if (gameMusicSource) gameMusicSource.volume = gameVolume;

        if (sfxSources != null)
        {
            foreach (var s in sfxSources)
                if (s != null)
                    s.volume = sfxVolume;
        }
    }

    // ---------------- MUSIC ----------------
    public void PlayMenuMusic(int index)
    {
        if (menuMusicSource == null || index >= menuMusicClips.Length) return;

        menuMusicSource.clip = menuMusicClips[index];
        menuMusicSource.loop = true;
        menuMusicSource.Play();
    }

    public void PlayGameMusic(int index)
    {
        if (gameMusicSource == null || index >= gameMusicClips.Length) return;

        gameMusicSource.clip = gameMusicClips[index];
        gameMusicSource.loop = true;
        gameMusicSource.Play();
    }

    // ---------------- SFX ----------------
    public void PlaySFX(string name)
    {
        if (!sfxDictionary.TryGetValue(name, out Sound s)) return;

        AudioSource source = sfxSources[sfxIndex];

        source.pitch = s.pitch;
        source.PlayOneShot(s.clip, s.volume * sfxVolume);

        sfxIndex = (sfxIndex + 1) % sfxSources.Length;
    }

    // ---------------- SLIDERS ----------------
    public void SetMenuVolume(float v)
    {
        menuVolume = v;
        SaveVolumes();
    }

    public void SetGameVolume(float v)
    {
        gameVolume = v;
        SaveVolumes();
    }

    public void SetSFXVolume(float v)
    {
        sfxVolume = v;
        SaveVolumes();
    }

    // ---------------- SAVE ----------------
    void SaveVolumes()
    {
        PlayerPrefs.SetFloat(MENU_VOLUME_KEY, menuVolume);
        PlayerPrefs.SetFloat(GAME_VOLUME_KEY, gameVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    void LoadVolumes()
    {
        menuVolume = PlayerPrefs.GetFloat(MENU_VOLUME_KEY, 1f);
        gameVolume = PlayerPrefs.GetFloat(GAME_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }
}