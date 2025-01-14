using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerFunctionality : MonoBehaviour
{
    [SerializeField]
    List<Behaviour> playerComponents = new List<Behaviour>();

    public void ActivateComponents()
    {
        foreach (Behaviour component in playerComponents)
        {
            component.enabled = true;
        }
    }

    public void DeactivateComponents()
    {
        foreach (Behaviour component in playerComponents)
        {
            component.enabled = false;
        }
    }
}
