using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    private FirstPersonController _player;
    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        _player = FindFirstObjectByType<FirstPersonController>();
    }
    
    void Update()
    {
        _agent.SetDestination(_player.transform.position);
    }
}
