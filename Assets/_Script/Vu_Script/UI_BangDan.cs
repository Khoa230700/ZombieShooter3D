using UnityEngine;
using UnityEngine.UI;

public class UI_BangDan : MonoBehaviour
{
    public Text _tongSoDan;
    public Text _soDanHienTai;

    public BangDan_sCRIPT bangDan_SCRIPT;
    void Start()
    {
        _tongSoDan.text = bangDan_SCRIPT.GetMaxBangDan().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _soDanHienTai.text = bangDan_SCRIPT.GetCurrenBangDan().ToString();
    }
}
