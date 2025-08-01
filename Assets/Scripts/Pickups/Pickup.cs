using UnityEngine;

namespace Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        [field: SerializeField] public bool Rotates { get; private set; }
        [field: SerializeField] public bool Shrinks { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float ShrinkRate { get; private set; }

        private Collider _collider;

        private const string PLAYER_STRING = "Player";
        
        void Start()
        {
            _collider = GetComponent<Collider>();
        }
    
        void Update()
        {
            Rotate();
            Shrink();
        }

        protected abstract void OnPickup(ActiveWeapon activeWeapon);
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PLAYER_STRING))
            {
                OnPickup(other.GetComponentInChildren<ActiveWeapon>());
                _collider.enabled = false;
            }
        }
    
        private void Rotate()
        {
            if (Rotates)
            {
                float angle = (RotationSpeed * Time.deltaTime) % 360; 
                transform.Rotate(Vector3.up, angle);
            }
        }

        private void Shrink()
        {
            if (!_collider.enabled && Shrinks)
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
}
