using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlot : MonoBehaviour {

    public enum State { Open, Claimed, Occupied };

    private State state = State.Open;
    private GameObject objectInSlot = null;

    public bool claimed {  get { return state == State.Claimed; } }
    public bool open {  get { return state == State.Open; } }
    public bool occupied {  get { return state == State.Occupied; } }

    public void Claim(GameObject go) {
        state = State.Claimed;
        objectInSlot = go;
    }

    public void Occupy(GameObject go) {
        state = State.Occupied;
        objectInSlot = go;
        
        //TODO an on occupy trigger?
    }

    public void Release() {
        state = State.Open;
        objectInSlot = null;

        //TODO a release trigger?
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
