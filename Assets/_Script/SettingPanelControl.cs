using UnityEngine;
using UnityEngine.UI;

public class SettingPanelControl : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 onScreenPosition;
    private Vector2 offScreenPosition;
    public Button BttClose, BttSetting;
    private AudioSource BttCloseSound, BttSettingSound;
    private bool isVisible = false;

    [Header("Audio Sliders")]
    public Slider themeSlider;
    public Slider sfxSlider;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        onScreenPosition = rectTransform.anchoredPosition;
        offScreenPosition = onScreenPosition + new Vector2(Screen.width, 0);
        rectTransform.anchoredPosition = offScreenPosition;

        BttClose.onClick.AddListener(TogglePanel);
        BttSetting.onClick.AddListener(TogglePanel);

        BttCloseSound = BttClose.GetComponent<AudioSource>();
        BttSettingSound = BttSetting.GetComponent<AudioSource>();

        themeSlider.value = PlayerPrefs.GetFloat("ThemeVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        themeSlider.onValueChanged.AddListener(SetThemeVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        ApplySavedVolume();
    }

    void ApplySavedVolume()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.FindAudioSources();

            float themeVol = PlayerPrefs.GetFloat("ThemeVolume", 1f);
            float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

            AudioManager.Instance.SetThemeVolume(themeVol);
            AudioManager.Instance.SetSFXVolume(sfxVol);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        if (isVisible)
            BttCloseSound.Play();
        else
            BttSettingSound.Play();

        rectTransform.anchoredPosition = isVisible ? offScreenPosition : onScreenPosition;
        isVisible = !isVisible;
    }

    private void SetThemeVolume(float volume)
    {
        AudioManager.Instance.SetThemeVolume(volume);
        PlayerPrefs.SetFloat("ThemeVolume", volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
