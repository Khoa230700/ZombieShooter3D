using UnityEngine;
using UnityEngine.UI;

public class LosePanelAppear : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject PlayerObject;
    public PlayerHealth playerHealth;
    public Text killText_Lose;
    public Text headshotText_Lose;
    public Text totalPointText_Lose;
    public Text killText_HUD;
    public Text headshotText_HUD;
    public Text totalPointText_HUD;

    void Start()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth <= 0)
        {
            ShowLosePanel();
        }
    }

    void ShowLosePanel()
    {
        if (losePanel != null)
        {
            killText_Lose.text = killText_HUD.text;
            headshotText_Lose.text = headshotText_HUD.text;
            totalPointText_Lose.text = totalPointText_HUD.text;
            losePanel.SetActive(true);
            PlayerObject.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
