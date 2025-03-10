using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunSO")]
public class GunSO : ScriptableObject
{
    public int Damage = 1;
    public float FireRate = 0.5f;
    [SerializeField] public GameObject bullethit;
}
