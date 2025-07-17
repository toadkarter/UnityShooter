using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [field: SerializeField] public int StartingHealth { get; private set; }
    private int _currentHealth;
    
    void Awake()
    {
        _currentHealth = StartingHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
