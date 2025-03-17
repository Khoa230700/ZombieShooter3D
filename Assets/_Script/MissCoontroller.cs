using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MissController : MonoBehaviour
{
    public Text[] Texts;
    public Text[] NumberTexts;
    public bool[] Missions = new bool[8];
    private int currentMissionIndex;
    private int shotsRequired;
    private int shotsMade;
    public TotalScore_Script Point;

    void Start()
    {
        ActivateRandomText();
    }

    void ActivateRandomText()
    {
        for (int i = 0; i < Texts.Length; i++)
        {
            if (Texts[i] != null)
                Texts[i].gameObject.SetActive(false);
            if (NumberTexts[i] != null)
                NumberTexts[i].gameObject.SetActive(false);
            Missions[i] = false;
        }

        currentMissionIndex = Random.Range(0, Texts.Length);
        shotsRequired = Random.Range(3, 6); 
        shotsMade = 0;

        if (Texts[currentMissionIndex] != null)
        {
            Texts[currentMissionIndex].gameObject.SetActive(true);
            NumberTexts[currentMissionIndex].gameObject.SetActive(true);
            NumberTexts[currentMissionIndex].text = $"0/{shotsRequired}";
        }
        Missions[currentMissionIndex] = true;
    }

    public void UpdateMissionProgress(string hitType, string hitObjectName, string gunName)
    {
        if (gunName == "Weapon1") gunName = "Pastrol";
        if (gunName == "Weapon2") gunName = "M16";
        hitObjectName = hitObjectName.Replace(" (Clone)", "(Clone)");

        string[] missionConditions = {
        "Pastrol_Headshot_ZombieNormal(Clone)",
        "Pastrol_Bodyshot_ZombieNormal(Clone)",
        "Pastrol_Bodyshot_Zombie2(Clone)",
        "Pastrol_Headshot_Zombie2(Clone)",
        "M16_Headshot_ZombieNormal(Clone)",
        "M16_Bodyshot_ZombieNormal(Clone)",
        "M16_Bodyshot_Zombie2(Clone)",
        "M16_Headshot_Zombie2(Clone)"
    };

        string shotCondition = $"{gunName}_{hitType}_{hitObjectName}";

        Debug.Log($"Kiểm tra: {shotCondition} vs {missionConditions[currentMissionIndex]}");

        if (shotCondition == missionConditions[currentMissionIndex])
        {
            shotsMade++;
            NumberTexts[currentMissionIndex].text = $"{shotsMade}/{shotsRequired}";

            Debug.Log($"Số lần bắn: {shotsMade}/{shotsRequired}");

            if (shotsMade >= shotsRequired)
            {
                Point.MissionPoint += 1000;
                Debug.Log("Mission Completed!");
                ActivateRandomText();
            }
        }
    }
}
