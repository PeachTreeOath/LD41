using UnityEngine;
using UnityEngine.Events;

public class Repeater : MonoBehaviour, IGlobalTimedObject {

    private float timer = 0f;

    public float interval = 5f;
    public UnityEvent action;

    public void ManualUpdate(float deltaTime) {
        timer += deltaTime;
        if(timer > interval) {
            timer = 0;
            action.Invoke();
        }
    }

    void Start() {
        GlobalTimer.instance.RegisterObject(this);        
    }

    void OnDestroy() {
        //GlobalTimer.instance.DeregisterObject(this);
    }
}
