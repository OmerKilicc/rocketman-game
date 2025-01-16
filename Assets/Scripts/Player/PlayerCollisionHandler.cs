using System;
using System.Collections;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] private VoidGameEvent onPlayerDeathEvent;
    [SerializeField] private float prismJumpMultiplier;
    [SerializeField] private float cylinderJumpMultiplier;
    
    PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Prism"))
        {
            _playerMovement.AddForce(prismJumpMultiplier);
        }
        else if (other.gameObject.CompareTag("Cylinder"))
        {
            _playerMovement.AddForce(cylinderJumpMultiplier);
        }
        else if(other.gameObject.CompareTag("Obstacles"))
        {
            onPlayerDeathEvent.Raise();
            _playerMovement.DeathMovement();
            _playerMovement.SetCanMove(false);
        }
    }
}
