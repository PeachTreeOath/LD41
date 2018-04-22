using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class KeyFrame {
    public enum Type { Lerp, Step, Event }

    public Type type = Type.Lerp;
    public float timestamp;
    public float value;
    public String eventName;
}

public class AiAnimation : ScriptableObject {
    public String id;
    public List<KeyFrame> keyframes; 
}