using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PrepareforTheBattle : MonoBehaviour
{
    public Button StarttheBattle, ImnotReady, WaitAMinute;
    private AudioSource ImnotReadySound,StarttheBattlesound,WaitAMinuteSound;
    public Text countdownText;
    public float holdTime = 3f;
    public bool voiceEnd = false;
    public AudioSource audioSource;
    private Coroutine countdownCoroutine;
    public GameObject SettingPanel;

    private void Awake()
    {
        SettingPanel.SetActive(true);
    }

    private void Start()
    {
        ImnotReadySound = ImnotReady.GetComponent<AudioSource>();
        StarttheBattlesound = StarttheBattle.GetComponent<AudioSource>();
        WaitAMinuteSound = WaitAMinute.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.Play();
        StartCoroutine(CheckAudioEnd());

        StarttheBattle.gameObject.SetActive(false);
        WaitAMinute.gameObject.SetActive(false);
        countdownText.text = "";
        ImnotReady.onClick.AddListener(TurningBack);
        StarttheBattle.onClick.AddListener(StartCountdown);
        StarttheBattle.onClick.AddListener(Okimready);
        WaitAMinute.onClick.AddListener(StopCountdown);
        WaitAMinute.onClick.AddListener(Waitforme);
    }

    IEnumerator CheckAudioEnd()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        StarttheBattle.gameObject.SetActive(true);
        voiceEnd = true;
    }

    public void StartCountdown()
    {
        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(CountdownCoroutine());
        }
    }

    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            countdownText.text = "";
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        for (int i = 3; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        TotheBattle();
    }

    public void TotheBattle()
    {
        SceneManager.LoadScene(4);
    }

    private IEnumerator WaitForSoundThenChangeScene(int sceneIndex, AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }
    void TurningBack()
    {
        ImnotReadySound.Play();
        StartCoroutine(WaitForSoundThenChangeScene(2, ImnotReadySound));
    }
    private IEnumerator WaitForSoundThenChangeButton(Button Gameobjecttochange,Button Gameobjecttoclose, AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Gameobjecttochange.gameObject.SetActive(true);
        Gameobjecttoclose.gameObject.SetActive(false);
    }

    void Waitforme()
    {
        WaitAMinuteSound.Play();
        StartCoroutine(WaitForSoundThenChangeButton(StarttheBattle,WaitAMinute,WaitAMinuteSound));
    }

    void Okimready()
    {
        StarttheBattlesound.Play();
        StartCoroutine(WaitForSoundThenChangeButton(WaitAMinute, StarttheBattle,StarttheBattlesound));
    }
}
