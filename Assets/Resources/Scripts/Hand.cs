using UnityEngine;

public class Hand : Singleton<Hand> {

    private Slots slots;
    public ZoneEvent handZoneEvent;
    public GameObject cardPrefab;

    public bool canDrawCards {  get { return slots.anyOpenSlots;  } }

    public void Start() {
        slots = GetComponent<Slots>();
        slots.occupyEvent.AddListener(OnOccupySlot);
        slots.releaseEvent.AddListener(OnReleaseSlot);
    }

    public ObjectSlot ClaimASlot(Card cardObject) {
        return slots.ClaimASlot(cardObject.gameObject); //TODO this is sooo wonky...
    }

    public void Update() {
        if (Input.GetKey(KeyCode.LeftShift)) return;

        //TODO remove with prejudice after typing works
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            PlayFromSlot(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            PlayFromSlot(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            PlayFromSlot(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            PlayFromSlot(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            PlayFromSlot(4);
        }
    }

    //TODO remove when typing works
    private void PlayFromSlot(int index) {
        var slot = slots.slots[index];
        if(slot.occupied) {
            var card = slot.objectInSlot.GetComponent<Card>();
            card.Play();
        }
    }

    private void OnOccupySlot(ObjectSlot slot, GameObject go) {
        handZoneEvent.Invoke(ZoneEvent.ENTERED, go.GetComponent<Card>());
    }

    private void OnReleaseSlot(ObjectSlot slot, GameObject go) {
        handZoneEvent.Invoke(ZoneEvent.EXITED, go.GetComponent<Card>());
    }
}
