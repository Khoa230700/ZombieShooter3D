using UnityEngine;

public class Vu_Test_Script : MonoBehaviour
{
    public BangDan_sCRIPT bangDan_SCRIPT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButton(0)) 
        {
            Debug.Log("Bắn rồi á.........!");
            bangDan_SCRIPT.TruDan(1);
        }
    }
}
