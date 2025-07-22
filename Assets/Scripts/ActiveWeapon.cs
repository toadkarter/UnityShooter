using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeapon : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponDetails { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
    [field: SerializeField] public Image ZoomVignette { get; private set; }

    private readonly int _shootAnimationId = Animator.StringToHash("Shoot");
    private StarterAssetsInputs _starterAssetsInputs;
    private FirstPersonController _firstPersonController;
    private Animator _animator;
    private Weapon _currentWeapon;

    private float _defaultFOV = 40.0f;
    private float _defaultRotationSpeed = 0.0f;
    private float _cooldownAmount = 0.0f;
    
    void Awake()
    {
        _firstPersonController = GetComponentInParent<FirstPersonController>();
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();

        _defaultRotationSpeed = _firstPersonController.RotationSpeed;
    }

    void Start()
    {
        _currentWeapon = GetComponentInChildren<Weapon>();
        SwitchWeapon(WeaponDetails);
    }
    
    void Update()
    {
        HandleShoot();
        HandleZoom();
    }

    public void SwitchWeapon(WeaponSO weaponDetails)
    {
        if (_currentWeapon)
        {
            Destroy(_currentWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponDetails.WeaponPrefab, transform).GetComponent<Weapon>();
        _currentWeapon = newWeapon;
        WeaponDetails = weaponDetails;
        
        ResetZoom();;
    }
    
    private void HandleShoot()
    {
        if (_cooldownAmount > 0.0f) _cooldownAmount -= Time.deltaTime;
        
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

    void HandleZoom()
    {
        if (WeaponDetails.CanZoom)
        {
            float cameraStartValue = Camera.m_Lens.FieldOfView;
            float cameraEndValue = _starterAssetsInputs.zoom ? WeaponDetails.ZoomAmount : _defaultFOV;

            float zoomVignetteStartValue = ZoomVignette.color.a;
            float zoomVignetteEndValue = _starterAssetsInputs.zoom ? 1.0f : 0.0f;
            
            if (!Mathf.Approximately(Camera.m_Lens.FieldOfView, cameraEndValue))
            {
                Camera.m_Lens.FieldOfView = Mathf.Lerp(cameraStartValue, cameraEndValue, WeaponDetails.ZoomRate * Time.deltaTime);

                Color color = ZoomVignette.color;
                color.a = Mathf.Lerp(zoomVignetteStartValue, zoomVignetteEndValue, WeaponDetails.ZoomRate * Time.deltaTime);
                ZoomVignette.color = color;
            }

            float rotationSpeed = _starterAssetsInputs.zoom ? WeaponDetails.ZoomRotationSpeed : _defaultRotationSpeed;
            _firstPersonController.SetRotationSpeed(rotationSpeed);
        }
    }

    private void ResetZoom()
    {
        Camera.m_Lens.FieldOfView = _defaultFOV;
        
        Color color = ZoomVignette.color;
        color.a = 0.0f;
        ZoomVignette.color = color;
    }
}
