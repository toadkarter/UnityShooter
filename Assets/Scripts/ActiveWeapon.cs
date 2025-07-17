using StarterAssets;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponDetails { get; private set; }

    private readonly int _shootAnimationId = Animator.StringToHash("Shoot");
    private StarterAssetsInputs _starterAssetsInputs;

    private Animator _animator;
    private Weapon _currentWeapon;

    private float _cooldownAmount = 0.0f;
    
    void Awake()
    {
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _currentWeapon = GetComponentInChildren<Weapon>();
    }
    
    void Update()
    {
        if (_cooldownAmount > 0.0f) _cooldownAmount -= Time.deltaTime;
        HandleShoot();
    }
    
    private void HandleShoot()
    {
        if (_starterAssetsInputs.shoot && _cooldownAmount <= 0.0f)
        {
            _currentWeapon.Shoot(WeaponDetails);
            _animator.Play(_shootAnimationId, 0, 0);
            _cooldownAmount = WeaponDetails.FireRate;

            if (!WeaponDetails.IsAutomatic)
            {
                _starterAssetsInputs.ShootInput(false);
            }
        }
    }
}
