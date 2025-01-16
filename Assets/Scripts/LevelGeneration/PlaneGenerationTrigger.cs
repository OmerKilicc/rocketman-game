using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerationTrigger : MonoBehaviour
{
    [SerializeField] private VoidGameEvent OnPlaceNewPlane;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Generation Trigger Worked");
            //Raise the event to generate   
            OnPlaceNewPlane.Raise();
        }
    }
}
