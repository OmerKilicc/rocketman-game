using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : BaseGameEventListener<Void, VoidGameEvent, UnityEvent<Void>>
{
    // Convenience method to raise the event without needing Void parameter
    public void OnEventRaised()
    {
        Response.Invoke(new Void());
    }
}