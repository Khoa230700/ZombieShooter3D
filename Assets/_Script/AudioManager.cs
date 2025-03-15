using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> themeSources = new List<AudioSource>();
    private List<AudioSource> sfxSources = new List<AudioSource>();

    void Start()
    {
        FindAudioSources();
        float themeVol = PlayerPrefs.GetFloat("ThemeVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetThemeVolume(themeVol);
        SetSFXVolume(sfxVol);
    }
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

        FindAudioSources();
    }

    public void FindAudioSources()
    {
        themeSources.Clear();
        sfxSources.Clear();

        foreach (var source in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (source.CompareTag("ThemeSound"))
                themeSources.Add(source);
            else if (source.CompareTag("SFX"))
                sfxSources.Add(source);
        }

        Debug.Log($"Found {themeSources.Count} theme sounds, {sfxSources.Count} SFX sounds.");
    }

    public void SetThemeVolume(float volume)
    {
        FindAudioSources();
        foreach (var source in themeSources)
        {
            if (source != null) source.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        FindAudioSources();
        foreach (var source in sfxSources)
        {
            if (source != null) source.volume = volume;
        }
    }
}
