using UnityEngine;

public class Collectbullet : MonoBehaviour
{
    public BangDanScript Piston;
    public BangDanScript Machinegun;

    public int Pistondanchuanap, Machinegundanchuanap;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piston"))
        {
            Debug.Log("đã chạm băn đạn");
            Piston.NhatBangDan();
            Pistondanchuanap += 30;
        }
        if (other.CompareTag("Machinegun"))
        {
            Debug.Log("đã chạm băn đạn");
            Machinegun.NhatBangDan();
            Machinegundanchuanap += 30;
        }
    }
}
