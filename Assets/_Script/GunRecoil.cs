using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Transform cameraTransform;
    public float recoilAmountX = 2f;
    public float recoilAmountY = 0.5f;
    public float maxRecoilX = 15f;

    private float totalRecoilX = 0f;

    public void ApplyRecoil()
    {
        if (cameraTransform == null) return;
        totalRecoilX = Mathf.Clamp(totalRecoilX + recoilAmountX, 0, maxRecoilX);
        float recoilY = Random.Range(-recoilAmountY, recoilAmountY);
        cameraTransform.localRotation *= Quaternion.Euler(-recoilAmountX, recoilY, 0);
    }
}
