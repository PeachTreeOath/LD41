using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slots : MonoBehaviour {

    public List<ObjectSlot> slots;

    public bool anyOpenSlots { get { return slotsUsed < maxSlots; }  }
    public int slotsUsed {  get { return slots.Count(s => !s.open); } }
    public int maxSlots {  get { return slots.Count;  } }

	void Start () {
        if (slots == null) {
            slots = new List<ObjectSlot>();
        }
	}

    public ObjectSlot ClaimASlot(GameObject go) {
        ObjectSlot freeSlot = null;
        foreach(var slot in slots) {
            if(slot.open) {
                freeSlot = slot;
                freeSlot.Claim(go);
                break;
            }
        }

        return freeSlot;
    }
	
}
