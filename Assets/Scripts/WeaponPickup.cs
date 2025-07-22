using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float ShrinkRate { get; private set; }
    [field: SerializeField] public WeaponSO WeaponDetails { get; private set; }

    private Collider _collider;

    void Start()
    {
        _collider = GetComponent<Collider>();
    }
    
    void Update()
    {
        Rotate();
        Shrink();
    }

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();
        if (activeWeapon)
        {
            activeWeapon.SwitchWeapon(WeaponDetails);
            _collider.enabled = false;
        }
    }

    private void Rotate()
    {
        float angle = (RotationSpeed * Time.deltaTime) % 360; 
        transform.Rotate(Vector3.up, angle);
    }

    private void Shrink()
    {
        if (!_collider.enabled)
        {
            float newScale = ShrinkRate * Time.deltaTime;

            if (transform.localScale is { x: <= 0.0f, y: <= 0.0f, z: <= 0.0f })
            {
                Destroy(gameObject);
            }
            else
            {
                transform.localScale -= new Vector3(newScale, newScale, newScale);
            }
        }
    }
}
