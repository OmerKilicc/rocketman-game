using UnityEngine;

[CreateAssetMenu(menuName = "Game Events/Void Event")]
public class VoidGameEvent : BaseGameEvent<Void>
{
    // Convenience method for raising the event without needing to pass Void parameter
    public void Raise()
    {
        Raise(new Void());
    }
}