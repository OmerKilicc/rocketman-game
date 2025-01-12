using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventListener<T>
{
    void RaiseEvent(T parameter);
}

public abstract class BaseGameEventListener<TParameter, TGameEvent, TUnityEvent> : MonoBehaviour,
    IEventListener<TParameter>
    where TGameEvent : BaseGameEvent<TParameter>
    where TUnityEvent : UnityEvent<TParameter>
{
    public TGameEvent GameEvent;
    public TUnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        GameEvent.UnRegisterListener(this);
    }

    public void RaiseEvent(TParameter t)
    {
        Response.Invoke(t);
    }
}