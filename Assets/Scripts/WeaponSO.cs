using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject WeaponPrefab = null;
    public int Damage = 1;
    public float FireRate = 0.5f;
    public GameObject HitVFXPrefab = null;
    public bool IsAutomatic = false;
    public bool CanZoom = false;
    public float ZoomAmount = 10.0f;
    public float ZoomRotationSpeed = 0.3f;
    public float ZoomRate = 100.0f;
    public int MagazineSize = 12;
}
