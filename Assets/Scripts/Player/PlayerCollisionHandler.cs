using System;
using System.Collections;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] VoidGameEvent onPlayerDeathEvent;
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Player collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Jumper"))
        {
            // TODO: Make it jump again?
        }
        else if(other.gameObject.CompareTag("Obstacles"))
        {
            onPlayerDeathEvent.Raise();
        }
    }
}
