using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PrepareforTheBattle : MonoBehaviour
{
    public Button StarttheBattle, ImnotReady, WaitAMinute;
    public Text countdownText;
    public float holdTime = 3f;
    public bool voiceEnd = false;
    public AudioSource audioSource;
    private Coroutine countdownCoroutine;

    private void Start()
    {
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

    void TurningBack()
    {
        SceneManager.LoadScene(2);
    }

    void Waitforme()
    {
        StarttheBattle.gameObject.SetActive(true);
        WaitAMinute.gameObject.SetActive(false);
    }

    void Okimready()
    {
        StarttheBattle.gameObject.SetActive(false);
        WaitAMinute.gameObject.SetActive(true);
    }
}
