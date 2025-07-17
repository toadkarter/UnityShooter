using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [field: SerializeField] public float RotationSpeed { get; private set; }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = (RotationSpeed * Time.deltaTime) % 360; 
        transform.Rotate(Vector3.up, angle);
    }
}
