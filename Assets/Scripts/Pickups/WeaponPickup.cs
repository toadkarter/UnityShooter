using UnityEngine;

namespace Pickups
{
    public class WeaponPickup : Pickup
    {
        [field: SerializeField] public WeaponSO WeaponDetails { get; private set; }

        protected override void OnPickup(ActiveWeapon activeWeapon)
        {
            if (activeWeapon)
            {
                activeWeapon.SwitchWeapon(WeaponDetails);
            }
        }
    }
}
