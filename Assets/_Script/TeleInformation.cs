using UnityEngine;

public class TeleInformation : MonoBehaviour
{
    public GunTypeController gunTypeController;
    public RaycastShooter[] raycastShooters;
    public MissController missController;

    void Start()
    {
        foreach (var shooter in raycastShooters)
        {
            shooter.OnObjectShot += HandleShotInfo;
        }
    }

    void HandleShotInfo(string hitType, string hitObjectName, string gunName)
    {
        Debug.Log($"{hitType} - Bắn vào: {hitObjectName} - Súng: {gunName}");
        if (missController != null)
        {
            missController.UpdateMissionProgress(hitType, hitObjectName, gunName);
        }
    }
}
