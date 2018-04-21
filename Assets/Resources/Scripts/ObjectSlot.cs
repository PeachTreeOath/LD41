using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlot : MonoBehaviour {

    public enum State { Open, Claimed, Occupied };

    private Slots slots;
    private State state = State.Open;

    public GameObject objectInSlot { get; private set; } 

    public bool claimed {  get { return state == State.Claimed; } }
    public bool open {  get { return state == State.Open; } }
    public bool occupied {  get { return state == State.Occupied; } }

    private void Start() {
        slots = transform.parent.GetComponent<Slots>();
    }

    public void Claim(GameObject go) {
        state = State.Claimed;
        objectInSlot = go;
        slots.OnClaim(this, go);
    }

    public void Occupy(GameObject go) {
        state = State.Occupied;
        objectInSlot = go;
        slots.OnOccupy(this, go);
    }

    public void Release() {
        state = State.Open;
        slots.OnRelease(this, objectInSlot);
        objectInSlot = null;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        var dims = new Vector3(1f, 1.5f);
        Gizmos.DrawWireCube(transform.position, dims);

        if(claimed) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, objectInSlot.transform.position);
        }
    }
}
