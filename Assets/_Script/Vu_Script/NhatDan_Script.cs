using UnityEngine;

public class NhatDan_Script : MonoBehaviour
{
    private Collectbullet Collectbullet;

    private void Start()
    {
        Collectbullet = FindAnyObjectByType< Collectbullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Có băng đạn");
            Collectbullet.Machinegundanchuanap += 30;
            Collectbullet.Pistondanchuanap += 30;
            Destroy(gameObject);
        }
    }
}
