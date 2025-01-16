using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFollowsPlayer : MonoBehaviour
{
    float _elapsedTime;
    [SerializeField] GameObject player;
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime >= 2)
        {
            transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            _elapsedTime = 0;
        }
    }
}
