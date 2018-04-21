using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class ZoneEvent : UnityEvent<String, Card> {
    public const string ENTERED = "entered";
    public const string EXITED = "exited";
}
