using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem MuzzleFlash { get; private set; } 
    
    public void Shoot(WeaponSO weaponDetails)
    {
        MuzzleFlash.Play();
        
        RaycastHit hit;
        bool hitFound = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);

        if (hitFound)
        {
            Instantiate(weaponDetails.HitVFXPrefab, hit.point, Quaternion.identity);
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            enemy?.TakeDamage(weaponDetails.Damage);
        }
    }
}
