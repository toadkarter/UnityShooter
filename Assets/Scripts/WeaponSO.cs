using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject WeaponPrefab = null;
    public int Damage = 1;
    public float FireRate = 0.5f;
    public GameObject HitVFXPrefab = null;
    public bool IsAutomatic = false;
}
