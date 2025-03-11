using UnityEngine;

public class BangDan_sCRIPT : MonoBehaviour
{
    [SerializeField] private float _maxBangDan;
    [SerializeField] private float _currenBangDan;

    void Start()
    {
        _currenBangDan = _maxBangDan;
    }

    public float GetCurrenBangDan() => _currenBangDan;
    public float GetMaxBangDan() => _maxBangDan;

    public void TruDan(float damage)
    {
        _currenBangDan -= damage;

    }
    public void CongDan(float health)
    {
        if (_currenBangDan == _maxBangDan)
        {
            _maxBangDan += health;
            _currenBangDan = _maxBangDan;
        }
        else
        {
            _currenBangDan += health;
        }
    }
}
