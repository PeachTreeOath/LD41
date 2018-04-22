using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class KeyFrame : IComparable<KeyFrame> {
    public enum Type { Lerp, Step, Event }

    public Type type = Type.Lerp;
    public float timestamp;
    public float value;
    public String eventName;

    public int CompareTo(KeyFrame other) {
        return timestamp.CompareTo(other.timestamp);
    }
}

public class AiAnimation : ScriptableObject {
    public String id;
    public List<KeyFrame> keyframes; 
}