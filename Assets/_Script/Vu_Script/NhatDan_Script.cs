using UnityEngine;

public class NhatDan_Script : MonoBehaviour
{
    public BangDanScript bangDanScript;
    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "BangDan")
    //    {
    //        Debug.Log("Có băng đạn");
    //        bangDanScript.NhatBangDan();
    //        Destroy(collision.gameObject);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Có băng đạn");
            bangDanScript.NhatBangDan();
            Destroy(gameObject);
        }
    }
}
