using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton;
    public GameObject PanelWarning;
    public Button bttHmm, bttYes;
    private AudioSource HmmSound, YesSound;

    private void Start()
    {
        HmmSound = bttHmm.GetComponent<AudioSource>();
        YesSound = bttYes.GetComponent<AudioSource>();
        PanelWarning.SetActive(false);
        exitButton.onClick.AddListener(TurnWarning);
        bttYes.onClick.AddListener(QuitGame);
        bttHmm.onClick.AddListener(TurnOffPanel);
    }

    void TurnWarning()
    {
        PanelWarning.SetActive(true);
    }

    IEnumerator WaitForSoundThenClosePanel(AudioSource audioSource, GameObject panelToClose)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        panelToClose.SetActive(false);
    }

    void TurnOffPanel()
    {
        HmmSound.Play();
        StartCoroutine(WaitForSoundThenClosePanel(HmmSound, PanelWarning));
    }    

    private void QuitGame()
    {

        Debug.Log("Game is quitting...");

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
