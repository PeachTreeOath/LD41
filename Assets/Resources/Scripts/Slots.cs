using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Slots : MonoBehaviour {

    public List<ObjectSlot> slots;
    public SlotEvent claimEvent;
    public SlotEvent occupyEvent;
    public SlotEvent releaseEvent;

    public bool anyOpenSlots { get { return slotsUsed < maxSlots; }  }
    public int slotsUsed {  get { return slots.Count(s => !s.open); } }
    public int maxSlots {  get { return slots.Count;  } }

	void Start () {
        if (slots == null) {
            slots = new List<ObjectSlot>();
        }
	}

    public ObjectSlot ClaimSlot(GameObject go, int index) {
        ObjectSlot freeSlot = null;
        if(index < slots.Count && index >= 0) {
            var slot = slots[index];
            if(slot.open) {
                freeSlot = slot;
                freeSlot.Claim(go);
            }
        }

        return freeSlot;
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

    public bool IsOpenAt(int index) {
        if (index < slots.Count && index >= 0) {
            var slot = slots[index];
            return slot.open;
        }

        return false;
    }

    public void OnClaim(ObjectSlot slot, GameObject objectEntering) {
        claimEvent.Invoke(slot, objectEntering);
    }
	
    public void OnOccupy(ObjectSlot slot, GameObject objectInSlot) {
        occupyEvent.Invoke(slot, objectInSlot);
    }

    public void OnRelease(ObjectSlot slot, GameObject objectLeaving) {
        releaseEvent.Invoke(slot, objectLeaving);
    }

    public bool AreAllSlotsBlocked()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (IsOpenAt(i))
                return false;
        }

        return true;
    }
}
