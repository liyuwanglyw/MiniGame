using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    //这个dic用来管理需要监听的各类事件
    private Dictionary<string, UnityEvent> eventDictionary;
    
    private static EventManager instance;

    public static EventManager getInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        eventDictionary = new Dictionary<string, UnityEvent>();
    }
    void Start()
    {
    }

    public void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    //在不需要监听的时候停止监听
    public void StopListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void StopAllListening(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveAllListeners();
        }
    }

    public void StopAllListening()
    {
        foreach(string eventName in eventDictionary.Keys)
        {
            StopAllListening(eventName);
        }
    }

    //触发某个事件
    public void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}