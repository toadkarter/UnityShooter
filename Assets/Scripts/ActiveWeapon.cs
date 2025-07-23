using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class ActiveWeapon : MonoBehaviour
{
    [field: SerializeField] public WeaponSO StartingWeapon { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera Camera { get; private set; }
    [field: SerializeField] public Image ZoomVignette { get; private set; }
    [field: SerializeField] public TMP_Text AmmoText { get; private set; }

    private readonly int _shootAnimationId = Animator.StringToHash("Shoot");
    private StarterAssetsInputs _starterAssetsInputs;
    private FirstPersonController _firstPersonController;
    private Animator _animator;
    private Weapon _currentWeapon;
    private WeaponSO _currentWeaponDetails;
    
    private readonly float _defaultFOV = 40.0f;
    private float _defaultRotationSpeed = 0.0f;
    private float _cooldownAmount = 0.0f;
    private int _currentAmmo = 0;
    
    void Awake()
    {
        _firstPersonController = GetComponentInParent<FirstPersonController>();
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();

        _defaultRotationSpeed = _firstPersonController.RotationSpeed;
    }

    void Start()
    {
        SwitchWeapon(StartingWeapon);
    }
    
    void Update()
    {
        HandleShoot();
        HandleZoom();
    }

    public void AdjustAmmo(int amount, bool set = false)
    {
        int ammoToSet = set ? amount : _currentAmmo + amount; 
        _currentAmmo = Mathf.Clamp(ammoToSet, 0, _currentWeaponDetails.MagazineSize);
        AmmoText.text = _currentAmmo.ToString("D2");
    }
    
    public void SwitchWeapon(WeaponSO weaponDetails)
    {
        if (_currentWeapon)
        {
            Destroy(_currentWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponDetails.WeaponPrefab, transform).GetComponent<Weapon>();
        _currentWeapon = newWeapon;
        _currentWeaponDetails = weaponDetails;
        AdjustAmmo(_currentWeaponDetails.MagazineSize, true);
        
        ResetZoom();
    }
    
    private void HandleShoot()
    {
        if (_cooldownAmount > 0.0f) _cooldownAmount -= Time.deltaTime;
        
        if (_starterAssetsInputs.shoot && _cooldownAmount <= 0.0f && _currentAmmo > 0)
        {
            _currentWeapon.Shoot(_currentWeaponDetails);
            _animator.Play(_shootAnimationId, 0, 0);
            _cooldownAmount = _currentWeaponDetails.FireRate;
            AdjustAmmo(-1);

            if (!_currentWeaponDetails.IsAutomatic)
            {
                _starterAssetsInputs.ShootInput(false);
            }
        }
    }

    void HandleZoom()
    {
        if (_currentWeaponDetails.CanZoom)
        {
            float cameraStartValue = Camera.m_Lens.FieldOfView;
            float cameraEndValue = _starterAssetsInputs.zoom ? _currentWeaponDetails.ZoomAmount : _defaultFOV;

            float zoomVignetteStartValue = ZoomVignette.color.a;
            float zoomVignetteEndValue = _starterAssetsInputs.zoom ? 1.0f : 0.0f;
            
            if (!Mathf.Approximately(Camera.m_Lens.FieldOfView, cameraEndValue))
            {
                Camera.m_Lens.FieldOfView = Mathf.Lerp(cameraStartValue, cameraEndValue, _currentWeaponDetails.ZoomRate * Time.deltaTime);

                Color color = ZoomVignette.color;
                color.a = Mathf.Lerp(zoomVignetteStartValue, zoomVignetteEndValue, _currentWeaponDetails.ZoomRate * Time.deltaTime);
                ZoomVignette.color = color;
            }

            float rotationSpeed = _starterAssetsInputs.zoom ? _currentWeaponDetails.ZoomRotationSpeed : _defaultRotationSpeed;
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
