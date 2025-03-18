using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public SettingPanelControl settingPanel;
    public GameObject Playergun;

    void Update()
    {
        if (settingPanel != null)
        {
            if (settingPanel.IsVisible())
            {
                Time.timeScale = 0;
                Playergun.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                Playergun.SetActive(true);
            }
        }
    }
}
