using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameStart;
    
    void Start()
    {
        OnGameStart.Invoke();
    }
    
}
