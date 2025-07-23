using System;
using UnityEngine;

namespace Pickups
{
    
    public class AmmoPickup : Pickup
    {
        [field: SerializeField] public int AmmoAmount { get; private set; }
        protected override void OnPickup(ActiveWeapon activeWeapon)
        {
            activeWeapon.AdjustAmmo(AmmoAmount);
        }
    }
}
